using System.ComponentModel.DataAnnotations;

namespace Marina.DataAccess.Entities;

public class BaseEntity
{
    [Key]
    public int Id { get; set; }
}
