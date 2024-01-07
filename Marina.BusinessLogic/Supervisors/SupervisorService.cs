using Marina.BusinessLogic.Tools;
using Marina.DataAccess.Entities;
using Marina.DataAccess.Repositories;

namespace Marina.BusinessLogic.Supervisors;

public interface ISupervisorService
{
    Task<List<SelectListItem>> GetDropDown();

}

public class SupervisorService : ISupervisorService
{
    private readonly IEFCoreGenericRepository<Supervisor> _supervisorRepository;

    public SupervisorService(IEFCoreGenericRepository<Supervisor> supervisorRepository)
    {
        _supervisorRepository = supervisorRepository;
    }
    public async Task<List<SelectListItem>> GetDropDown()
    {
        var response = await _supervisorRepository.GetAll();
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
