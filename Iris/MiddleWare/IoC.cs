namespace Iris.MiddleWare
{
    using Businnes;
    using Businnes.Interfaces;
    using Data;
    using Microsoft.EntityFrameworkCore;
    using Repository;
    public static class IoC
    {
        public static IServiceCollection AddDependency(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddScoped<IUnitOfWork, UnitOfWork>();
            services.AddDbContext<ContextDb>(db => db.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));
            services.AddScoped<IUnitOfWork>(sp => new UnitOfWork(
                sp.GetRequiredService<DbContextOptions<ContextDb>>(),
                sp.GetRequiredService<IConfiguration>().GetConnectionString("DefaultConnection")));
            services.AddScoped<IToDoBusiness, TodoBusiness>();
            return services;
        }
    }
}
