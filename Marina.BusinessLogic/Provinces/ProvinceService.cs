using Marina.BusinessLogic.Tools;
using Marina.DataAccess.Entities;
using Marina.DataAccess.Repositories;

namespace Marina.BusinessLogic.Provinces;

public interface IProvinceService
{
    Task<List<SelectListItem>> GetDropDown();

}

public class ProvinceService : IProvinceService
{
    private readonly IEFCoreGenericRepository<Province> _provinceRepository;

    public ProvinceService(IEFCoreGenericRepository<Province> provinceRepository)
    {
        _provinceRepository = provinceRepository;
    }
    public async Task<List<SelectListItem>> GetDropDown()
    {
        var response = await _provinceRepository.GetAll();
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
