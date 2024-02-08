using Marina.BusinessLogic.Files;
using Microsoft.AspNetCore.Mvc;
using Marina.UI.General;
using Marina.UI.Models;
using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.IdentityModel.Tokens;

namespace Marina.UI.Controllers;
[Authorize]
public class FileController : BaseController
{
    private readonly IExcelFileProcessor _excelFileProcessor;
    private static DataTable dataTable;
    private readonly string tableName;
    public FileController(IExcelFileProcessor excelFileProcessor)
    {
        _excelFileProcessor = excelFileProcessor;
        tableName = SetTableName();
    }
    public IActionResult Index()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Upload(IFormFile file)
    {
        var validationResult = ValidateModel(file, new FileUploadValidator());
        if (!validationResult.IsValid)
            return HandleValidationResult(file, validationResult);

        dataTable = _excelFileProcessor.ProcessExcelFile(User, file);
        var tableExists = await _excelFileProcessor.TableExists(tableName);

        if (tableExists)
        {
            var sourceColumns = dataTable.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToList();
            var destinationColumn = _excelFileProcessor.SelectColumnNameTable(tableName);

            _excelFileProcessor.IsValid(sourceColumns, destinationColumn, out string errorMessage);
            if (!string.IsNullOrEmpty(errorMessage))
                ViewBag.ErrorMessage = errorMessage;
        }

        return PartialView("_DataTablePartialView", dataTable);
    }

    [HttpPost]
    public async Task<IActionResult> Import()
    {

        //var tableExists = await _excelFileProcessor.TableExists(tableName);

        //DataColumnCollection dataColumn = dataTable.Columns;

        //if (!tableExists)
        //    await _excelFileProcessor.CreateTable(dataColumn, tableName);


        //if (tableExists)
        //{
        //    var destinationColumn = _excelFileProcessor.SelectColumnNameTable(tableName);

        //    await _excelFileProcessor.Save(dataTable, tableName, destinationColumn);
        //}
        await _excelFileProcessor.Save2(dataTable, tableName);

        return RedirectToAction("Index");
    }

    public async Task<IActionResult> DownloadExcel()
    {
        var tableExists = await _excelFileProcessor.TableExists(tableName);

        if (tableExists)
        {
            var columnNames = _excelFileProcessor.SelectColumnNameTable(tableName).Skip(3).ToArray();

            var fileName = $"File-{GetDateMonthName(DateTime.Now).Replace("-", "")}.xlsx";

            var excelBytes = _excelFileProcessor.CreateExcelFileResult(columnNames);

            return File(excelBytes, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", fileName);
        }
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
    private static string GetDateMonthName(DateTime date)
    {
        return date.ToString("MMM dd HH:mm");
    }
}
