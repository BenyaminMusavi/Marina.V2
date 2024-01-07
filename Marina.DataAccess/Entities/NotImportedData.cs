using System.ComponentModel.DataAnnotations;

namespace Marina.DataAccess.Entities;

public partial class NotImportedData : BaseEntity
{
    public NotImportedData(int supervisorId, string personName)
    {
        SupervisorId = supervisorId;
        PersonName = personName;
    }

    public int SupervisorId { get; set; }
    public string PersonName { get; set; } = null!;
    public DateTime DateTime { get; set; }
    public virtual Supervisor Supervisor { get; set; }

}
