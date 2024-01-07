namespace Marina.BusinessLogic.Accounts;

public class CookieUserItem
{
    public int UserId { get; set; }
    public string DistributorCode { get; set; }
    public string Province { get; set; }
    public string Line { get; set; }
    public DateTime CreatedUtc { get; set; } = DateTime.UtcNow;
}
