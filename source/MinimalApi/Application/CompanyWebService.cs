using MinimalApi.Domain.Models;
using MinimalApi.Infrastructure;

namespace MinimalApi.Application;

public interface ICompanyService
{
    Task<IList<Company>> GetAllAsync();
    Task<Company?> GetByIdAsync(Guid id);
}

public class CompanyWebService : ICompanyService
{
    private readonly ICompaniesRepository _projectRepository;

    public CompanyWebService(ICompaniesRepository projectRepository)
    {
        _projectRepository = projectRepository;
    }

    public Task<IList<Company>> GetAllAsync()
    {
        return _projectRepository.GetAllAsync();
    }

    public Task<Company?> GetByIdAsync(Guid id)
    {
        return _projectRepository.GetByIdAsync(id);
    }
}
