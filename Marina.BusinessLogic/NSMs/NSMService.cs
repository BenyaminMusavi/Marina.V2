using Marina.BusinessLogic.Tools;
using Marina.DataAccess.Entities;
using Marina.DataAccess.Repositories;

namespace Marina.BusinessLogic.NSMs;

public interface INSMService
{
    Task<List<SelectListItem>> GetDropDown();

}


public class NSMService : INSMService
{
    private readonly IEFCoreGenericRepository<NSM> _nsmRepository;

    public NSMService(IEFCoreGenericRepository<NSM> nsmRepository)
    {
        _nsmRepository = nsmRepository;
    }
    public async Task<List<SelectListItem>> GetDropDown()
    {
        var response = await _nsmRepository.GetAll();
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
