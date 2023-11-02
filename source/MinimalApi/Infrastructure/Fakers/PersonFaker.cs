using Bogus;

namespace MinimalApi.Infrastructure.Fakers;

public class PersonFaker : Faker<Grpc.Person.Person>
{
    public PersonFaker()
    {
        RuleFor(p => p.Id, f => f.UniqueIndex);
        RuleFor(p => p.Name, f => f.Person.FullName);
        RuleFor(p => p.Age, f => f.Random.Number(18, 40));
        RuleFor(p => p.Address, f => f.Address.FullAddress());
    }
}
