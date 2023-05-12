using IdentityServerDemo;
using IdentityServerDemo.Data;
using IdentityServerDemo.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);
{
    //get connection
    var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

    //AspNet Identity
    builder.Services.AddDbContext<EfContext>(options =>
    {
        options.UseSqlServer(connectionString);
    });
    
    builder.Services.AddIdentity<IdentityUser, IdentityRole>()
                    .AddEntityFrameworkStores<EfContext>();

    builder.Services.AddControllersWithViews();

    //Identity server
    string migrationsAssembly = Assembly.GetExecutingAssembly().FullName ?? "";
    var identityServerBuilder = builder.Services.AddIdentityServer();

    identityServerBuilder
        // this adds the config data from DB (clients, resources)
        .AddConfigurationStore(options =>
        {
            options.ConfigureDbContext = (dbContextOptionsBuilder) =>
            {
                dbContextOptionsBuilder.UseSqlServer(connectionString,
                    (sqlServerOptions) => { sqlServerOptions.MigrationsAssembly(migrationsAssembly); });
            };
            options.DefaultSchema = "idc";
        })
        // this adds the operational data from DB (codes, tokens, consents)
        .AddOperationalStore((options) =>
        {
            options.ConfigureDbContext = (dbContextOptionsBuilder) =>
            {
                dbContextOptionsBuilder.UseSqlServer(connectionString,
                    (sqlServerOptions) => { sqlServerOptions.MigrationsAssembly(migrationsAssembly); });
            };
            options.DefaultSchema = "idp";
            // this enables automatic token cleanup. this is optional.
            options.EnableTokenCleanup = true;
        });

    if (builder.Environment.IsDevelopment())
    {
        identityServerBuilder.AddDeveloperSigningCredential();
    }
    else
    {
        //For production add certificate manually
    }
    
}

var app = builder.Build();
{
    //seed 
    using (var scope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope())
    {
        SeedData.EnsureSeedData(scope.ServiceProvider);
    }

    // set configure
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseIdentityServer();
    app.MapDefaultControllerRoute();
}

app.Run();
