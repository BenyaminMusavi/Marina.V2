using Marina.BusinessLogic.Tools;
using Marina.DataAccess.Entities;
using Marina.DataAccess.Repositories;

namespace Marina.BusinessLogic.Lines;

public interface ILineService
{
    Task<List<SelectListItem>> GetDropDown();

}

public class LineService : ILineService
{
    private readonly IEFCoreGenericRepository<Line> _lineRepository;

    public LineService(IEFCoreGenericRepository<Line> lineRepository)
    {
        _lineRepository = lineRepository;
    }

    public async Task<List<SelectListItem>> GetDropDown()
    {
        var response = await _lineRepository.GetAll();
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
