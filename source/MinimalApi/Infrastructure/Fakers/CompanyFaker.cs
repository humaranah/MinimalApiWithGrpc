using Bogus;
using MinimalApi.Domain.Models;

namespace MinimalApi.Infrastructure.Fakers;

public class CompanyFaker : Faker<Company>
{
    public CompanyFaker()
    {
        RuleFor(p => p.Id, f => f.Random.Guid());
        RuleFor(p => p.Name, f => f.Company.CompanyName());
    }
}
