using FluentValidation;
using FluentValidation.Results;
using Marina.UI.Models;
using Microsoft.AspNetCore.Mvc;

namespace Marina.UI.General;

public class BaseController : Controller
{
    protected IActionResult HandleValidationResult<TModel>(TModel model, ValidationResult validationResult) where TModel : class
    {
        AddValidationErrorsToModelState(validationResult.Errors);
        return View(model);
    }

    protected void AddValidationErrorsToModelState(IEnumerable<ValidationFailure> errors)
    {
        foreach (var error in errors)
        {
            AddModelError(error.PropertyName, error.ErrorMessage);
        }
    }

    protected void AddModelError(string key, string errorMessage)
    {
        ModelState.AddModelError(key, errorMessage);
    }

    protected ValidationResult ValidateModel<TModel>(TModel model, AbstractValidator<TModel> validator) where TModel : class
    {
        return validator.Validate(model);
    }
}



//protected void HandleLoginValidation(ValidationResult validationResult)
//{
//    var loginResult = validationResult.Errors.SingleOrDefault(error => error.PropertyName == "Password");
//    if (loginResult != null)
//    {
//        AddModelError("Password", loginResult.ErrorMessage);
//    }
//    else
//    {
//        AddModelError("Username", "Invalid username or password");
//        AddModelError("Password", "Invalid username or password");
//    }
//}
