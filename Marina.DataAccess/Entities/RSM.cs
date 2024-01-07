using System.ComponentModel.DataAnnotations;

namespace Marina.DataAccess.Entities;

public partial class RSM : BaseEntity
{
    public string Name { get; set; } = null!;
}
