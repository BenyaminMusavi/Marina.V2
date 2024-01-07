using System.ComponentModel.DataAnnotations;

namespace Marina.DataAccess.Entities;

public partial class User : BaseEntity
{
    public string DName { get; set; } = null!;
    public int RegionId { get; set; }
    public int RSMId { get; set; }
    public string UserName { get; set; } = null!;
    public int DistributorId { get; set; }
    public int LineId { get; set; }
    public int ProvinceId { get; set; }
    public int NsmId { get; set; }
    public int SupervisorId { get; set; }
    public string PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;

    #region  Audit Fiels 
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    public bool HasImported { get; set; } = false;
    public DateTime CreateDate { get; set; }
    public long? UpdaterUserId { get; set; }
    public DateTime? UpdateTime { get; set; }
    #endregion

    public virtual Distributor Distributor { get; set; }
    public virtual Line Line { get; set; }
    public virtual Province Province { get; set; }
    public virtual IList<Supervisor> Supervisor { get; set; }
    public virtual Region Region { get; set; }
    public virtual RSM RSM { get; set; }
    public virtual NSM NSM { get; set; }
}
