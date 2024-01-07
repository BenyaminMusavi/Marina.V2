using System.ComponentModel.DataAnnotations;

namespace Marina.DataAccess.Entities;

public partial class Region : BaseEntity
{
    public string Name { get; set; } = null!;
}
