using Grpc.Core;
using MinimalApi.Grpc.Person;
using MinimalApi.Infrastructure;

namespace MinimalApi.Application;

public class PersonGrpcService : PersonService.PersonServiceBase
{
    private readonly IPersonRepository _personRepository;

    public PersonGrpcService(IPersonRepository personRepository)
    {
        _personRepository = personRepository;
    }

    public override async Task<PersonListResponse> GetPersonList(PersonListRequest request, ServerCallContext context)
    {
        var response = new PersonListResponse();
        
        var persons = await _personRepository.GetAllAsync();
        foreach (var person in persons)
        {
            response.Data.Add(person);
        }

        return response;
    }
}
