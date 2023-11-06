# Minimal Api with Grpc
This is just a project to test C# Minimal Api with Grpc coexistence.

## Project structure

```mermaid
flowchart LR
    A["Asp.Net request<br>(port 48000)"]
    B["Grpc request<br>(port 48001)"]

    subgraph G1["MinimalApi"]
        subgraph G11["Application"]
            C["Asp.Net request mappers"]
            D["Grpc services"]
        end
        subgraph G12["Domain"]
            E["Models"]
        end
        subgraph G13["Infrastructure"]
            F["Repositories"]
        end
    end

    subgraph G2["MinimalApi.Grpc"]
        H[".proto files"]
        G["Autogenerated<br>C# files"]
    end

    A --> C --- G12
    B --> D --- G12
    C --- G13 --- G12
    D --- G13
    H -.-> G
    D <-.- G
    B <-.- H
```

The project structure is composed of two projects:

### MinimalApi.Grpc
This project contains the .proto files definitions.

When this project is built, it will autogenerate the C# code through [Grpc.Tools](https://www.nuget.org/packages/Grpc.Tools/) Nuget package.

The generated Grpc code will be used by the Grpc services, in the main project (ie.: [PersonGrpcService](source/MinimalApi/Application/PersonGrpcService.cs)).

### MinimalApi
This project contains all the interaction logic of the API and Grpc services. It is composed of a very basic DDD folder structure (just for testing).

The sample data is autogenerated using [Bogus](https://www.nuget.org/packages/Bogus/) faker package.

## Working with ASP.Net and Grpc together
There are some considerations about working with both technologies in the same project:

From my experience with this research, Grpc calls require using Http/2 protocol exclusively. In the other hand, ASP.Net endpoints have some issues trying to work in that mode.

The quicker way I found to fix this was by listening two different ports:
- The first listener with `Http1AndHttp2` mode, for ASP.Net requests.
- The second listener with `Http2` mode exclusively, for Grpc requests.

The following code can be found in [Program.cs](source/MinimalApi/Program.cs):
```csharp
builder.WebHost.ConfigureKestrel(options =>
{
    // Asp.Net calls should use 'http://localhost:48000'
    // and Grpc calls should use 'http://localhost:48001'
    options.ListenLocalhost(48000, config => config.Protocols = HttpProtocols.Http1AndHttp2);
    options.ListenLocalhost(48001, config => config.Protocols = HttpProtocols.Http2);
});
```

**Note:** I disabled SSL/TLS in this project since it is intended just for testing that Asp.Net can work with Grpc together. That configuration **should not** be used in a production environment for security reasons.

### Person Service
Person service is an example of how we can request the same information through both Asp.Net and Grpc requests.

#### Web request
A web request can be done using the following url:
```http://localhost:48000/persons```

That request will do the following:
1. Request will be handled by "Get mapper" defined in [Program.cs](source/MinimalApi/Program.cs).
2. It will call [FakerPersonRepository.cs](source/MinimalApi/Infrastructure/FakerPersonRepository.cs) to get sample data.
3. The response will be forwarded to the client (this is a very small application just for testing).

#### Grpc request
A Grpc request can be done using any Grpc client (ie: BloomRPC or Postman) by selecting the [Person.proto](source/MinimalApi.Grpc/Protos/Person.proto) file and using the following url:
```http://localhost:48001```

That request will do the following:
1. Request will be handled by "Grpc mapper" defined in [Program.cs](source/MinimalApi/Program.cs).
2. It will call the [implementation](source/MinimalApi/Application/PersonGrpcService.cs) of `PersonServiceBase` (class generated by [Grpc.Tools](https://www.nuget.org/packages/Grpc.Tools/) using [Person.proto](source/MinimalApi.Grpc/Protos/Person.proto) file during compilation).
3. It will call [FakerPersonRepository.cs](source/MinimalApi/Infrastructure/FakerPersonRepository.cs) to get sample data.
4. Response of the repository will be translated to Grpc response in [PersonGrpcService](source/MinimalApi/Application/PersonGrpcService.cs).
5. Grpc response will be forwarded to the client.

In both cases, the response will be the same for this example.
