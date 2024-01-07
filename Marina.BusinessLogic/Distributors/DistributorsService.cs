using Marina.BusinessLogic.Tools;
using Marina.DataAccess.Entities;
using Marina.DataAccess.Repositories;

namespace Marina.BusinessLogic.Distributors;

public interface IDistributorsService
{
    Task<List<SelectListItem>> GetDropDown();

}

public class DistributorsService : IDistributorsService
{
    private readonly IEFCoreGenericRepository<Distributor> _distributorRepository;

    public DistributorsService(IEFCoreGenericRepository<Distributor> distributorRepository)
    {
        _distributorRepository = distributorRepository;
    }
    public async Task<List<SelectListItem>> GetDropDown()
    {
        var response = await _distributorRepository.GetAll();
        var items = new List<SelectListItem>();

        if (response != null)
        {
            foreach (var item in response)
            {
                items.Add(new SelectListItem { Id = item.Id, Name = item.Code });
            }
        }
        return items;
    }
}
