using Microsoft.AspNetCore.Authorization;
using Marina.BusinessLogic.Accounts;
using Microsoft.AspNetCore.Mvc;
using Marina.UI.Models;
using Marina.UI.General;

namespace Marina.UI.Controllers;

[Authorize]
public class AccountController : BaseController
{
    private readonly IAccountService _service;

    public AccountController(IAccountService service)
    {
        _service = service;
    }

    public IActionResult Index()
    {
        return View();
    }

    [AllowAnonymous]
    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Login(LoginVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var validationResult = ValidateModel(model, new LoginValidator());
        if (!validationResult.IsValid)
            return HandleValidationResult(model, validationResult);

        var dto = model.ToDto();

        var user = _service.Validate(dto);
        if (user is not null)
        {
            await _service.SignIn(this.HttpContext, user, false);
            //var redirectPath = user.UserId == 1 ? "~/Account/list" : "~/";
            var redirectPath = "/";
            return LocalRedirect(redirectPath);
        }
        return View(model);
    }

    public async Task<IActionResult> LogoutAsync()
    {
        await _service.SignOut(this.HttpContext);
        return RedirectPermanent("~/Home/Index");
    }

    [Authorize(Roles = "admin")]
    public async Task<IActionResult> List()
    {
        var res = await _service.GetAll();
        return View(res);
    }


    [AllowAnonymous]
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    [AllowAnonymous]
    public async Task<IActionResult> Register(RegisterVm model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var validationResult = ValidateModel(model, new RegisterValidator());
        if (!validationResult.IsValid)
            return HandleValidationResult(model, validationResult);

        var tblName = CalculateUserNameTable(model.DistributorCode, model.ProvinceName, model.LineName);

        if (await _service.TableExists(tblName))
        {
            AddModelError("", "User already exists.");
            return View(model);
        }

        var dto = model.ToDto();

        (var user, var message) = await _service.Register(dto);

        if (!user)
        {
            AddModelError(model.UserName, message);
            return View(model);
        }

        if (user)
        {
            AddModelError(model.UserName, message);
            return LocalRedirect("~/Account/Login");
        }

        return View();
    }

    private static string CalculateUserNameTable(string distributorCode, string province, string line)
    {
        var userNameTable = $"{distributorCode}_{province}_{line}";
        return userNameTable;
    }

    [HttpPost]
    public async Task<ActionResult> Active(int id)
    {
        var IsSuccess = await _service.SetStatus(id);

        if (IsSuccess)
            ViewBag.IsSuccess = true;
        else
            ViewBag.IsSuccess = false;
        return LocalRedirect("~/Account/list");
    }

    [HttpPost]
    public async Task<ActionResult> Delete(int id)
    {
        var IsSuccess = await _service.Delete(id);

        if (IsSuccess)
            ViewBag.IsSuccess = true;
        else
            ViewBag.IsSuccess = false;
        return LocalRedirect("~/Account/list");
    }
}
