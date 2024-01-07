using System.ComponentModel.DataAnnotations;

namespace Marina.DataAccess.Entities;

public partial class Province : BaseEntity
{
    public string Name { get; set; } = null!;
}
