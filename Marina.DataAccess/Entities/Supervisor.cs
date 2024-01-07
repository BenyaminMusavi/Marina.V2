using System.ComponentModel.DataAnnotations;

namespace Marina.DataAccess.Entities;

public partial class Supervisor : BaseEntity
{
    public string Name { get; set; } = null!;
    public string Email { get; set; } = null!;
    public virtual IList<User> Users { get; set; }

}
