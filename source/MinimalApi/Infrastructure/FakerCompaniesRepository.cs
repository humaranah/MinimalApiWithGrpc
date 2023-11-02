using MinimalApi.Domain.Models;
using MinimalApi.Infrastructure.Fakers;

namespace MinimalApi.Infrastructure;

public interface ICompaniesRepository
{
    Task<IList<Company>> GetAllAsync();
    Task<Company?> GetByIdAsync(Guid id);
}

public class FakerCompaniesRepository : ICompaniesRepository
{
    private readonly List<Company> _companies;

    public FakerCompaniesRepository()
    {
        var faker = new CompanyFaker();
        _companies = faker.Generate(20);
    }

    public Task<IList<Company>> GetAllAsync()
    {
        return Task.FromResult<IList<Company>>(_companies);
    }

    public Task<Company?> GetByIdAsync(Guid id)
    {
        return Task.FromResult(_companies.FirstOrDefault(p => p.Id == id));
    }
}
