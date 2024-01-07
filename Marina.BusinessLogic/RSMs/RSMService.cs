using Marina.BusinessLogic.Tools;
using Marina.DataAccess.Entities;
using Marina.DataAccess.Repositories;

namespace Marina.BusinessLogic.RSMs;

public interface IRSMService
{
    Task<List<SelectListItem>> GetDropDown();

}

public class RSMService: IRSMService
{
    private readonly IEFCoreGenericRepository<RSM> _rsmRepository;

    public RSMService(IEFCoreGenericRepository<RSM> rsmRepository)
    {
        _rsmRepository = rsmRepository;
    }
    public async Task<List<SelectListItem>> GetDropDown()
    {
        var response = await _rsmRepository.GetAll();
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
