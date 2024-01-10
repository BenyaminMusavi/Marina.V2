using System.Data;
using System.Text;

namespace Marina.UI.Models;

public class CustomeDataTable : DataTable
{
    public CustomeDataTable() : base()
    {
    }

    public List<string> ColumnNames
    {
        get
        {
            return this.Columns.Cast<DataColumn>().Select(column => column.ColumnName).ToList();
        }
    }

    public bool IsValid(List<string> destinationColumns, out string errorMessage)
    {
        ColumnNames.Add("Id");
        //var itemsInList1NotInList2 = sourceColumns.Except(destinationColumns);
        var itemsInList1NotInList2 = this.ColumnNames;
        var itemsInList2NotInList1 = destinationColumns.Except(this.ColumnNames);
        var allUniqueItems = itemsInList2NotInList1.Union(itemsInList1NotInList2);

        var stringBuilder = new StringBuilder();
        foreach (var item in allUniqueItems)
        {
            stringBuilder.AppendLine($"The column '{item}' does not exist in the database table.\n");
        }
        errorMessage = stringBuilder.ToString().Trim();
        return string.IsNullOrEmpty(errorMessage);
    }
}
