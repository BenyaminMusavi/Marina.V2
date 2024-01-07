using Marina.BusinessLogic.Tools;
using Marina.DataAccess.Entities;
using Marina.DataAccess.Repositories;

namespace Marina.BusinessLogic.Regions;

public interface IRegionService
{
    Task<List<SelectListItem>> GetDropDown();
}
public class RegionService : IRegionService
{
    private readonly IEFCoreGenericRepository<Region> _regionRepository;
    
    public RegionService(IEFCoreGenericRepository<Region> regionRepository)
    {
        _regionRepository = regionRepository;
    }

    public async Task<List<SelectListItem>> GetDropDown()
    {
        var response =  await _regionRepository.GetAll();
        var items = new List<SelectListItem>();

        if (response != null)
        {
            foreach (var item in response)
            {
                items.Add(new SelectListItem { Id = item.Id, Name = item.Name });
            }
        }
        return items;
    }
}
