using Marina.BusinessLogic.Provinces;
using Marina.BusinessLogic.RSMs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Marina.UI.Controllers;

public class RSMController : Controller
{
    private readonly IRSMService _service;

    public RSMController(IRSMService service)
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
