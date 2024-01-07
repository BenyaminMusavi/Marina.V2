using Marina.BusinessLogic.Tools;
using Marina.DataAccess.Entities;
using Marina.DataAccess.Repositories;
using Marina.DataAccess.Tools;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;

    
namespace Marina.BusinessLogic.Accounts;

public interface IAccountService
{
    Task SignIn(HttpContext httpContext, CookieUserItem user, bool isPersistent = false);
    Task SignOut(HttpContext httpContext);
    Task<List<AccountDto>> GetAll();
    CookieUserItem Validate(LoginDto model);
    Task<(bool,string)> Register(RegisterDto model);
    Task<bool> TableExists(string tableName);
    //Task<bool> TableExists(int distributorCode, int provinceId, int lineId);
    Task<bool> SetStatus(int id);
    Task<bool> Delete(int id);

}

public class AccountService : IAccountService
{
    IEFCoreGenericRepository<User> _userRepository;
    private readonly ITableChecker _tableChecker;

    public AccountService(IEFCoreGenericRepository<User> userRepository, ITableChecker tableChecker)
    {
        _userRepository = userRepository;
        _tableChecker = tableChecker;
    }

    public Task<List<AccountDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public async Task SignIn(HttpContext httpContext, CookieUserItem user, bool isPersistent = false)
    {
        string authenticationScheme = CookieAuthenticationDefaults.AuthenticationScheme;

        // Generate Claims from DbEntity
        var claims = GetUserClaims(user);

        // Add Additional Claims from the Context
        // which might be useful
        // claims.Add(httpContext.User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.Name));

        ClaimsIdentity claimsIdentity = new ClaimsIdentity(claims, authenticationScheme);
        ClaimsPrincipal claimsPrincipal = new ClaimsPrincipal(claimsIdentity);

        var authProperties = new AuthenticationProperties
        {

            // AllowRefresh = <bool>,
            // Refreshing the authentication session should be allowed.
            // ExpiresUtc = DateTimeOffset.UtcNow.AddMinutes(10),
            // The time at which the authentication ticket expires. A 
            // value set here overrides the ExpireTimeSpan option of 
            // CookieAuthenticationOptions set with AddCookie.
            // IsPersistent = true,
            // Whether the authentication session is persisted across 
            // multiple requests. Required when setting the 
            // ExpireTimeSpan option of CookieAuthenticationOptions 
            // set with AddCookie. Also required when setting 
            // ExpiresUtc.
            // IssuedUtc = <DateTimeOffset>,
            // The time at which the authentication ticket was issued.
            // RedirectUri = "~/Account/Index"
            // The full path or absolute URI to be used as an http 
            // redirect response value.
        };

        await httpContext.SignInAsync(authenticationScheme, claimsPrincipal, authProperties);
    }

    public async Task SignOut(HttpContext httpContext)
    {
        await httpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        await httpContext.ChallengeAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    }

    private static List<Claim> GetUserClaims(CookieUserItem user)
    {
        List<Claim> claims = new List<Claim>
        {
            new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
            //new Claim(ClaimTypes.Name, user.DistributorCode),
            new Claim("DistributorCode" , user.DistributorCode),
            new Claim("Line", user.Line),
            new Claim("Province", user.Province),
        };
        if (user.UserId == 1)
            claims.Add(new Claim(ClaimTypes.Role, "admin"));

        return claims;
    }

    public CookieUserItem Validate(LoginDto model)
    {
        var userRecords = (_userRepository.AsQueryable());
        //.Where(x => x.UserName == model.Username && x.IsActive)
        //.Include(x => x.Distributor)
        //.Include(x => x.Line)
        //.Include(x => x.Province);

        var results = userRecords //.AsEnumerable()
        .Where(m => m.PasswordHash == Hasher.GenerateHash(model.Password, m.Salt))
        .Select(m => new CookieUserItem
        {
            DistributorCode = m.Distributor.Code,
            Line = m.Line.Name,
            Province = m.Province.Name,
            CreatedUtc = m.CreateDate,
            UserId = m.Id
        });

        return results.FirstOrDefault();
    }

    public async Task<(bool, string)> Register(RegisterDto model)
    {
        var userRecords = await (_userRepository.AsQueryable()).Where(x => x.UserName == model.UserName.Trim()).FirstOrDefaultAsync();

        if (userRecords is not null)
            return (false, "User already exists.");

        var salt = Hasher.GenerateSalt();
        var hashedPassword = Hasher.GenerateHash(model.Password, salt);

        var user = FromUserRegistrationModelToUser(model, hashedPassword, salt);

        await _userRepository.Add(user);

        return (true, "The operation was successful");
    }

    private static User FromUserRegistrationModelToUser(RegisterDto userRegistration, string hashedPassword, string salt)
    {
        return new User
        {
            DName = userRegistration.DistributorName,
            RegionId = userRegistration.RegionId,
            PasswordHash = hashedPassword,
            Salt = salt,
            UserName = userRegistration.UserName.Trim(),
            RSMId = userRegistration.RSMId,
            LineId = userRegistration.LineId,
            ProvinceId = userRegistration.ProvinceId,
            DistributorId = userRegistration.DistributorId,
            PhoneNumber = userRegistration.PhoneNumber,
            SupervisorId = userRegistration.SupervisorId,
            NsmId = userRegistration.NsmId
        };
    }

    //private string CalculateUserNameTable(int distributorCode, int provinceId, int lineId)
    //{
    //    var distributor = _db.Distributors.FirstOrDefault(x => x.Id == distributorCode);
    //    var distributorName = distributor?.Code;
    //    var province = _db.Provinces.FirstOrDefault(x => x.Id == provinceId);
    //    var provinceName = province?.Name;
    //    var line = _db.Lines.FirstOrDefault(x => x.Id == lineId);
    //    var lineName = line?.Name;
    //    var userNameTable = $"{distributorName}_{provinceName}_{lineName}";
    //    return userNameTable;
    //}

    public Task<bool> SetStatus(int id)
    {
        throw new NotImplementedException();
    }

    public Task<bool> Delete(int id)
    {
        throw new NotImplementedException();
    }
    public async Task<bool> TableExists(string tableName)
    {
        return await _tableChecker.TableExistsAsync(tableName);
    }
}
