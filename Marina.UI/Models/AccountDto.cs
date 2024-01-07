namespace Marina.UI.Models;

public class AccountDto
{
    public int Id { get; set; }
    public string DistributorName { get; set; } = null!;
    public int RegionId { get; set; }
    public int RSMId { get; set; }
    public string UserName { get; set; } = null!;
    public int DistributorId { get; set; }
    public int LineId { get; set; }
    public int ProvinceId { get; set; }
    public string PhoneNumber { get; set; }
    public string PasswordHash { get; set; } = null!;
    public string Salt { get; set; } = null!;

    #region  Audit Fiels 
    public bool IsDeleted { get; set; }
    public bool IsActive { get; set; }
    public DateTime CreateDate { get; set; }
    public long? UpdaterUserId { get; set; }
    public DateTime? UpdateTime { get; set; }
    #endregion
}
