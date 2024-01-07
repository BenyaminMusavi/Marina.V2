namespace Marina.BusinessLogic.Accounts;

public class RegisterDto
{
    public RegisterDto(string userName, int lineId, int provinceId, string password, string distributorName, int regionId, int rSMId, int distributorId, string phoneNumber, int supervisorId, int nsmId)
    {
        UserName = userName;
        LineId = lineId;
        ProvinceId = provinceId;
        Password = password;
        DistributorName = distributorName;
        RegionId = regionId;
        RSMId = rSMId;
        DistributorId = distributorId;
        PhoneNumber = phoneNumber;
        SupervisorId = supervisorId;
        NsmId = nsmId;
    }

    public string UserName { get; set; }
    public int LineId { get; set; }
    public int ProvinceId { get; set; }
    public string Password { get; set; }
    public string ConfirmPassword { get; set; }
    public string DistributorName { get; set; } = null!;
    public int RegionId { get; set; }
    public int RSMId { get; set; }
    public int DistributorId { get; set; }
    public string PhoneNumber { get; set; }
    public int SupervisorId { get; set; }
    public int NsmId { get; set; }
}
