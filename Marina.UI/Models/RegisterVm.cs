namespace Marina.UI.Models;

public class RegisterVm
{
    public string UserName { get; set; }
    public int LineId { get; set; }
    public string LineName { get; set; }
    public int ProvinceId { get; set; }
    public string ProvinceName { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string DistributorName { get; set; } = null!;
    public int RegionId { get; set; }
    public int RSMId { get; set; }
    public int DistributorId { get; set; }
    public string DistributorCode { get; set; }
    public string PhoneNumber { get; set; }
    public int SupervisorId { get; set; }
    public int NsmId { get; set; }
}
