using System.Data;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Marina.DataAccess.Tools;

public interface ITableChecker
{
    Task<bool> TableExistsAsync(string tableName);
    Task<int> DeleteEntitiesAsync(string tableName, string date);
    Task<bool> CreateTableAsync(string query);
    List<string> SelectColumnNameTable(string tableName);
    bool ColumnMapping(DataTable sourceDataTable, string tableName);
    Task<bool> SaveAsync(string tableName, DataTable dataTable);


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

    //public async Task<bool> TableExistsAsync(string tableName)
    //{
    //    var query = $"SELECT 1 FROM INFORMATION_SCHEMA.TABLES WHERE TABLE_NAME = {tableName}";
    //    var result = await _context.Database.ExecuteSqlRawAsync(query);
    //    var tableExists = result > 0;
    //    return tableExists;
    //}
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
        var sql = $"SELECT * FROM INFORMATION_SCHEMA.COLUMNS WHERE TABLE_NAME = '{tableName}'";
        using (var command = _context.Database.GetDbConnection().CreateCommand())
        {
            command.CommandText = sql;
            command.CommandType = CommandType.Text;
            _context.Database.OpenConnection();
            using (var result = command.ExecuteReader())
            {
                var columnNames = Enumerable.Range(0, result.FieldCount).Select(i => result.GetName(i)).ToList();
                return columnNames;
            }
        }
    }

    public bool ColumnMapping(DataTable sourceDataTable, string tableName)
    {
        try
        {
            var destinationColumns = SelectColumnNameTable(tableName);
            var bulkCopy = new SqlBulkCopy(_context.Database.GetDbConnection().ConnectionString);

            foreach (DataColumn column in sourceDataTable.Columns)
            {
                var sourceColumn = column.ColumnName.Replace(" ", "");
                var isValid = destinationColumns.Contains(sourceColumn);
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
        try
        {
            await _context.Database.OpenConnectionAsync();
            await using (var transaction = _context.Database.BeginTransaction())
            {
                try
                {
                    await DeleteEntitiesAsync(tableName, GetPersianDate());

                    // ایجاد جدول موقت
                    await _context.Database.ExecuteSqlRawAsync($"CREATE TABLE #TempTable AS SELECT  FROM {tableName} WHERE 1 = 0");

                    // نسخه اطلاعات از DataTable به جدول موقت
                    await BulkCopyToTempTableAsync(dataTable);

                    // اجرای دستور SQL برای اضافه کردن اطلاعات از جدول موقت به جدول اصلی
                    var sql = $"INSERT INTO {tableName} SELECT  FROM #TempTable";
                    await _context.Database.ExecuteSqlRawAsync(sql);

                    // حذف جدول موقت
                    await _context.Database.ExecuteSqlRawAsync("DROP TABLE #TempTable");
                    transaction.Commit();
                }
                catch (Exception ex)
                {
                    transaction.Rollback();
                    Console.WriteLine("خطا در ذخیره سازی داده ها: " + ex.Message);
                    return false;
                }
            }
            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine("خطا در اتصال به دیتابیس: " + ex.Message);
            return false;
        }
    }

    private async Task BulkCopyToTempTableAsync(DataTable dataTable)
    {
        using (var bulkCopy = new SqlBulkCopy(_context.Database.GetDbConnection().ConnectionString))
        {
            bulkCopy.DestinationTableName = "#TempTable";
            await bulkCopy.WriteToServerAsync(dataTable);
        }
    }
    private static string GetPersianDate()
    {
        System.Globalization.PersianCalendar persianCalandar = new();
        var dateTime = DateTime.Now;
        string year = persianCalandar.GetYear(dateTime).ToString().Substring(2, 2);
        string month = persianCalandar.GetMonth(dateTime).ToString("0#");
        return $"{year}{month}";
    }
}
