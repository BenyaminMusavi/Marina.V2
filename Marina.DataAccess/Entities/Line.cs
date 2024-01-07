using System.ComponentModel.DataAnnotations;

namespace Marina.DataAccess.Entities;

public partial class Line : BaseEntity
{
    public string Name { get; set; } = null!;
}
