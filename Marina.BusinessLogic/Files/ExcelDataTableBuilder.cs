using ExcelDataReader;
using System.Data;
using System.Security.Claims;

namespace Marina.BusinessLogic.Files;

public class ExcelDataTableBuilder
{
    public DataTable BuildDataTable(ClaimsPrincipal user, IExcelDataReader reader)
    {
        DataTable dt = CreateDataTable(reader);
        AddColumnsFromExcel(dt, reader);
        AddRowsFromExcel(dt, reader,user);
        return dt;
    }

    private static DataTable CreateDataTable(IExcelDataReader reader)
    {
        DataTable dt = new();
        dt.Columns.Add("UserId");
        dt.Columns.Add("PerDate");
        return dt;
    }

    private static void AddColumnsFromExcel(DataTable dt, IExcelDataReader reader)
    {
        DataTable dt_ = reader.AsDataSet().Tables[0];
        for (int i = 0; i < dt_.Columns.Count; i++)
        {
            dt.Columns.Add(dt_.Rows[0][i].ToString());
        }
    }

    private void AddRowsFromExcel(DataTable dt, IExcelDataReader reader, ClaimsPrincipal user)
    {
        using (reader)
        {
            DataTable dt_ = reader.AsDataSet().Tables[0];
            var Date = GetPersianDate();
            string? userId = user.Identity?.ToString();
            for (int row_ = 1; row_ < dt_.Rows.Count; row_++)
            {
                DataRow row = dt.NewRow();
                row["UserId"] = userId;
                row["PerDate"] = Date;
                for (int col = 0; col < dt_.Columns.Count; col++)
                {
                    row[col + 2] = dt_.Rows[row_][col].ToString();
                }
                dt.Rows.Add(row);
            }
        }
    }

    public static string GetPersianDate()
    {
        System.Globalization.PersianCalendar persianCalandar = new();
        var dateTime = DateTime.Now;
        string year = persianCalandar.GetYear(dateTime).ToString().Substring(2, 2);
        string month = persianCalandar.GetMonth(dateTime).ToString("0#");
        return $"{year}{month}";
    }
}