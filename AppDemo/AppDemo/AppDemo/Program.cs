using AppDemo.Controllers;

var builder = WebApplication.CreateBuilder(args);
{
    builder.Services.AddControllersWithViews();

    builder.Services.AddHttpClient<HomeController>();

    builder.Services.AddAuthentication(options =>
    {
        options.DefaultScheme = "Cookie";
        options.DefaultChallengeScheme = "oidc";
    })
    .AddCookie("Cookie")
    .AddOpenIdConnect("oidc", options =>
    {
        options.Authority = "http://localhost:1700";

        options.ClientId = "client_mvc";
        options.ClientSecret = "74ba35a3-e6ee-470f-b8d6-9c27f670025a";

        options.Scope.Add("demo.service");
        options.Scope.Add("demo.service2");

        options.ResponseType = "code";
        options.SaveTokens = true;

        options.RequireHttpsMetadata = false;
    });
}

var app = builder.Build();
{
    app.UseHttpsRedirection();
    app.UseStaticFiles();
    app.UseRouting();
    app.UseAuthentication();
    app.UseAuthorization();
    app.MapControllerRoute(
        name: "default",
        pattern: "{controller=Home}/{action=Index}/{id?}");
    app.Run();
}
