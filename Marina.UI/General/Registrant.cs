using Marina.BusinessLogic.Accounts;
using Marina.BusinessLogic.Distributors;
using Marina.BusinessLogic.Files;
using Marina.BusinessLogic.Lines;
using Marina.BusinessLogic.NSMs;
using Marina.BusinessLogic.Provinces;
using Marina.BusinessLogic.Regions;
using Marina.BusinessLogic.RSMs;
using Marina.BusinessLogic.Supervisors;
using Marina.DataAccess;
using Marina.DataAccess.Entities;
using Marina.DataAccess.Repositories;
using Marina.DataAccess.Tools;
using Microsoft.EntityFrameworkCore;

namespace Marina.UI.General
{
    public static class Registrant
    {
        public static void RegisterTypes(IServiceCollection serviceCollection, IConfiguration Configuration)
        {
            serviceCollection.AddDbContext<MarinaDbContext>(c => c.UseSqlServer(Configuration.GetConnectionString("MarinaConnectionString")));


            serviceCollection.AddTransient(typeof(IEFCoreGenericRepository<>), typeof(EFCoreGenericRepository<>));
            serviceCollection.AddTransient(typeof(IEFCoreGenericRepository<User>), typeof(EFCoreGenericRepository<User>));

            serviceCollection.AddTransient<IAccountService, AccountService>();
            serviceCollection.AddTransient<IExcelFileProcessor, ExcelFileProcessor>();
            serviceCollection.AddTransient<ITableChecker, TableChecker>();

            serviceCollection.AddTransient<IRegionService, RegionService>();
            serviceCollection.AddScoped<IDistributorsService, DistributorsService>();
            serviceCollection.AddScoped<ILineService, LineService>();
            serviceCollection.AddScoped<INSMService, NSMService>();
            serviceCollection.AddScoped<IProvinceService, ProvinceService>();
            serviceCollection.AddScoped<IRSMService, RSMService>();
            serviceCollection.AddScoped<ISupervisorService, SupervisorService>();


        }
    }
}
