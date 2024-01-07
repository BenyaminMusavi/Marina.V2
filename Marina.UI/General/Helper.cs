using Marina.BusinessLogic.Regions;
using Marina.DataAccess;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Marina.UI.General;

public static class Helper
{
    public static void EnsureDatabaseMigrated(this IApplicationBuilder app)
    {
        using var scope = app.ApplicationServices.GetService<IServiceScopeFactory>().CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<MarinaDbContext>();
        var pendingMigrations = dbContext.Database.GetPendingMigrations();

        if (pendingMigrations.Any())
        {
            dbContext.Database.Migrate();
        }
    }

    public static async Task<List<SelectListItem>> GetRegionDropDown()
    {
        using (HttpClient client = new())
        {
            client.BaseAddress = new Uri("https://localhost:44315/");
            var result = await client.GetStringAsync("Region/GetDropDown");
            var finalResult = JsonConvert.DeserializeObject<List<SelectListItem>>(result);
            return finalResult;
        }
    }
}
