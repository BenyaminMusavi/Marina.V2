﻿using System.Data;
using System.Security.Claims;
using System.Text;
using ExcelDataReader;
using Marina.DataAccess.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;

namespace Marina.BusinessLogic.Files;

public interface IExcelFileProcessor
{
    DataTable ProcessExcelFile(ClaimsPrincipal user, IFormFile upload);
    Task<bool> Save(DataTable dataTable, string tableName, List<string> a);
    Task<bool> TableExists(string tableName);
    Task<bool> CreateTable(DataColumnCollection columns, string tableName);
    Task DeleteRows(string tableName);
    bool IsValid(List<string> column, List<string> destinationColumn, out string errorMessage);
    List<string> SelectColumnNameTable(string tableName);
}

public class ExcelFileProcessor : IExcelFileProcessor
{
    private readonly ITableChecker _tableChecker;
    private readonly IConfiguration _configuration;
    public ExcelFileProcessor(ITableChecker tableChecker, IConfiguration configuration)
    {
        _tableChecker = tableChecker;
        _configuration = configuration;
    }

    public DataTable ProcessExcelFile(ClaimsPrincipal user, IFormFile upload)
    {
        using var reader = GetExcelDataReader(upload);
        var excelDataTableBuilder = new ExcelDataTableBuilder();
        return excelDataTableBuilder.BuildDataTable(user, reader);
    }

    private static IExcelDataReader GetExcelDataReader(IFormFile upload)
    {
        Stream stream = upload.OpenReadStream();
        if (upload.FileName.EndsWith(".xls"))
            return ExcelReaderFactory.CreateBinaryReader(stream);
        else return ExcelReaderFactory.CreateOpenXmlReader(stream);
    }

    public async Task<bool> TableExists(string tableName)
    {
        return await _tableChecker.TableExistsAsync(tableName);
    }

    public async Task DeleteRows(string tableName)
    {
        var date = GetPersianDate();
        await _tableChecker.DeleteEntitiesAsync(tableName, date);
    }

    public async Task<bool> CreateTable(DataColumnCollection columns, string tableName)
    {
        var query = CreateQueryTable(columns, tableName);
        return await _tableChecker.CreateTableAsync(query);
    }

    public bool IsValid(List<string> sourceColumns, List<string> destinationColumns, out string errorMessage)
    {
        sourceColumns.Add("Id");
        var itemsInList1NotInList2 = sourceColumns.Except(destinationColumns);
        var itemsInList2NotInList1 = destinationColumns.Except(sourceColumns);
        var allUniqueItems = itemsInList2NotInList1.Union(itemsInList1NotInList2);

        var stringBuilder = new StringBuilder();
        foreach (var item in allUniqueItems)
        {
            stringBuilder.AppendLine($"The column '{item}' does not exist in the database table.");
        }
        errorMessage = stringBuilder.ToString().Trim();
        return string.IsNullOrEmpty(errorMessage);
    }

    public async Task<bool> Save(DataTable dataTable, string tableName, List<string> a)
    {
        return await _tableChecker.SaveAsync2(tableName, dataTable, a);
    }

    private string CreateQueryTable(DataColumnCollection columns, string tableName)
    {
        var databaseName = _configuration["Database:Name"];

        var queryBuilder = new StringBuilder();
        queryBuilder.Append($"USE [{databaseName}]; CREATE TABLE DBO.[{tableName}] (Id INT IDENTITY(1, 1), ");

        foreach (string column in columns)
        {
            string columnName = column.Replace(" ", "");
            queryBuilder.Append($"{columnName} nvarchar(100) NULL,");
        }

        queryBuilder.Remove(queryBuilder.Length - 1, 1);
        queryBuilder.Append(");");

        return queryBuilder.ToString();
    }

    private static string GetPersianDate()
    {
        System.Globalization.PersianCalendar persianCalandar = new();
        var dateTime = DateTime.Now;
        string year = persianCalandar.GetYear(dateTime).ToString().Substring(2, 2);
        string month = persianCalandar.GetMonth(dateTime).ToString("0#");
        return $"{year}{month}";
    }

    public List<string> SelectColumnNameTable(string tableName)
    {
        return _tableChecker.SelectColumnNameTable(tableName);
    }
}
