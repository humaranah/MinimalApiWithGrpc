using MinimalApi.Grpc.Person;
using MinimalApi.Infrastructure.Fakers;

namespace MinimalApi.Infrastructure;

public interface IPersonRepository
{
    Task<IList<Person>> GetAllAsync();
}

public class FakerPersonRepository : IPersonRepository
{
    private readonly List<Person> _persons;

    public FakerPersonRepository()
    {
        var faker = new PersonFaker();
        _persons = faker.Generate(10);
    }

    public Task<IList<Person>> GetAllAsync()
    {
        return Task.FromResult<IList<Person>>(_persons);
    }
}
