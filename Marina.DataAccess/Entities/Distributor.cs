using System.ComponentModel.DataAnnotations;

namespace Marina.DataAccess.Entities;

public partial class Distributor: BaseEntity
{
    public string Code { get; set; } = null!;
}
