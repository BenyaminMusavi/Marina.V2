using Marina.BusinessLogic.Regions;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marina.UI.Controllers;

public class RegionController : Controller
{
    private readonly IRegionService _service;

    public RegionController(IRegionService service)
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
