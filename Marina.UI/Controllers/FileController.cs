using Marina.BusinessLogic.Files;
using Microsoft.AspNetCore.Mvc;
using Marina.UI.General;
using Marina.UI.Models;
using System.Data;
using System.Security.Claims;

namespace Marina.UI.Controllers
{
    public class FileController : BaseController
    {
        private readonly IExcelFileProcessor _excelFileProcessor;
        private static DataTable dataTable;
        public FileController(IExcelFileProcessor excelFileProcessor)
        {
            _excelFileProcessor = excelFileProcessor;
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Upload(IFormFile upload)
        {
            var validationResult = ValidateModel(upload, new FileUploadValidator());
            if (!validationResult.IsValid)
                return HandleValidationResult(upload, validationResult);

            dataTable = _excelFileProcessor.ProcessExcelFile(User, upload);

            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> Import()
        {
            var tableName = SetTableName();

            var tableExists = await _excelFileProcessor.TableExists(tableName);

            //if (tableExists)
            //    await _excelFileProcessor.DeleteRows(tableName);

            if (!tableExists)
                await _excelFileProcessor.CreateTable(dataTable, tableName);

            if (_excelFileProcessor.IsValid(dataTable, tableName))
                await _excelFileProcessor.Save(dataTable, tableName);

            return RedirectToAction("Index");
        }

        private static string SetTableName()
        {
            var httpContextAccessor = new HttpContextAccessor();
            var province = httpContextAccessor.HttpContext?.User.FindFirstValue("Province");
            var distributorCode = httpContextAccessor.HttpContext?.User.FindFirstValue("DistributorCode");
            var line = httpContextAccessor.HttpContext?.User.FindFirstValue("Line");
            var tblName = $"{distributorCode}_{province}_{line}";
            return tblName;
        }
    }
}
