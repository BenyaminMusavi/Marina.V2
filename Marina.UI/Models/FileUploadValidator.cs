using FluentValidation;

namespace Marina.UI.Models
{
    public class FileUploadValidator : AbstractValidator<IFormFile>
    {
        public FileUploadValidator()
        {
            RuleFor(file => file).Must(file => file != null && file.Length > 0)
                                 .WithMessage("Please upload your file")
                                 .Must(file => file.FileName.EndsWith(".xls") || file.FileName.EndsWith(".xlsx"))
                                 .WithMessage("This file format is not supported");
        }
    }
}
