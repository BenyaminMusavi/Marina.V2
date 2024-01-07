using Marina.BusinessLogic.Distributors;
using Marina.BusinessLogic.Lines;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marina.UI.Controllers;

public class LineController : Controller
{
    private readonly ILineService _service;

    public LineController(ILineService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        return View();
    }
    public async Task<List<SelectListItem>> GetDropDown()
    {
        var result = await _service.GetDropDown();
        return result.Select(x => new SelectListItem
        {
            Text = x.Name,
            Value = x.Id.ToString(),
        }).ToList();
    }
}
