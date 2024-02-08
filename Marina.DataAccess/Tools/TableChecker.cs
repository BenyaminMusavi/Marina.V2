using System.Data;
using System.Reflection.PortableExecutable;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory.Database;

namespace Marina.DataAccess.Tools;

public interface ITableChecker
{
    Task<bool> TableExistsAsync(string tableName);
    Task<int> DeleteEntitiesAsync(string tableName, string date);
    Task<bool> CreateTableAsync(string query);
    List<string> SelectColumnNameTable(string tableName);
    Task<bool> SaveAsync(string tableName, DataTable dataTable);
    Task<bool> SaveAsync2(string tableName, DataTable dataTable, List<string>? strings);


}

public class TableChecker : ITableChecker
{
    private readonly MarinaDbContext _context;
    private readonly IConfiguration _configuration;

    public TableChecker(MarinaDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    public async Task<bool> TableExistsAsync(string tableName)
    {
        try
        {
            var conn = _configuration["ConnectionStrings:MarinaConnectionString"];

            using SqlConnection connection = new(conn);
            connection.Open();
            var query = $"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = '{tableName}'";
            SqlCommand command = new(query, connection);
            var result = await command.ExecuteScalarAsync();
            return result != null;
        }
        catch (Exception)
        {
            // Log or handle the exception if needed
            return false;
        }
    }

    public async Task<int> DeleteEntitiesAsync(string tableName, string date)
    {
        var query = $"DELETE FROM {tableName} WHERE PerDate = @Date";
        var parameters = new SqlParameter("@Date", date);
        return await _context.Database.ExecuteSqlRawAsync(query, parameters);
    }

    public async Task<bool> CreateTableAsync(string query)
    {
        var result = await _context.Database.ExecuteSqlRawAsync(query);
        var tableExists = result > 0;
        return tableExists;
    }

    public List<string> SelectColumnNameTable(string tableName)
    {
        var sql = $"SELECT TOP (0) * FROM {tableName}";
        using (var command = _context.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            _context.Database.OpenConnection();
            using (var result = command.ExecuteReader())
            {
                var columnNames = Enumerable.Range(0, result.FieldCount)
                    .Select(i => result.GetName(i))
                    .ToList();
                return columnNames;
            }
        }
    }

    private static bool ColumnMapping(DataTable dataTable, SqlBulkCopy bulkCopy, List<string> destinationColumnNames)
    {
        try
        {
            //        //bool areEqual = list1.Count == list2.Count && list1.All(x => list2.Contains(x));

            foreach (DataColumn column in dataTable.Columns)
            {
                //var sourceColumn = column.ColumnName.Replace(" ", "");
                var sourceColumn = column.ColumnName;
                var isValid = destinationColumnNames.Contains(sourceColumn);
                if (isValid)
                    bulkCopy.ColumnMappings.Add(sourceColumn, sourceColumn);
                else
                    return false;
            }
            return true;
        }
        catch (Exception ex)
        {
            throw new Exception("Error mapping columns: " + ex.Message);
        }
    }
    public async Task<bool> SaveAsync(string tableName, DataTable dataTable)
    {
        var conn = _configuration["ConnectionStrings:MarinaConnectionString"];

        using (SqlConnection con = new(conn))
        {
            using (SqlBulkCopy bulkCopy = new(con))
            {
                try
                {
                    con.Open();

                    var destinationColumnName = SelectColumnNameTable(tableName);
                    var isValidColumnMapping = ColumnMapping(dataTable, bulkCopy, destinationColumnName);
                    if (!isValidColumnMapping)
                        return false;

                    var Date = GetPersianDate();
                    var queryDeleted = $"DELETE FROM {tableName} WHERE PerDate = @Date";
                    using (SqlCommand commandDeleted = new(queryDeleted, con))
                    {
                        commandDeleted.Parameters.AddWithValue("@Date", Date);
                        var rowsAffected = commandDeleted.ExecuteNonQuery();
                    }
                    bulkCopy.DestinationTableName = tableName;
                    await bulkCopy.WriteToServerAsync(dataTable);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("خطا در ذخیره سازی داده ها: " + ex.Message);
                    return false;
                }
            }
        }
        return true;
    }

    private static string GetPersianDate()
    {
        System.Globalization.PersianCalendar persianCalandar = new();
        var dateTime = DateTime.Now;
        string year = persianCalandar.GetYear(dateTime).ToString().Substring(2, 2);
        string month = persianCalandar.GetMonth(dateTime).ToString("0#");
        return $"{year}{month}";
    }


    public async Task<bool> SaveAsync2(string tableName, DataTable dataTable, List<string>? strings)
    {
        var conn = _configuration["ConnectionStrings:MarinaConnectionString"];

        using (SqlConnection con = new(conn))
        {
            using (SqlBulkCopy bulkCopy = new(con))
            {
                try
                {
                    con.Open();

                    var destinationColumnName = new List<string>();
                    var sql = $"SELECT TOP (0) * FROM {tableName}";
                    using (var command = _context.Database.GetDbConnection().CreateCommand())
                    {
                        command.CommandText = sql;
                        command.CommandType = CommandType.Text;
                        _context.Database.OpenConnection();
                        using (var result = command.ExecuteReader())
                        {
                            destinationColumnName = Enumerable.Range(0, result.FieldCount)
                                .Select(i => result.GetName(i))
                                .ToList();
                        }
                    }

                    if (strings is not null)
                    {
                        foreach (string column in dataTable.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(column, column);
                        }
                    }

                    var Date = GetPersianDate();
                    var queryDeleted = $"DELETE FROM {tableName} WHERE PerDate = @Date";
                    using (SqlCommand commandDeleted = new(queryDeleted, con))
                    {
                        commandDeleted.Parameters.AddWithValue("@Date", Date);
                        var rowsAffected = commandDeleted.ExecuteNonQuery();
                    }
                    bulkCopy.DestinationTableName = tableName;
                    await bulkCopy.WriteToServerAsync(dataTable);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("خطا در ذخیره سازی داده ها: " + ex.Message);
                    return false;
                }
            }
        }
        return true;
    }
}

