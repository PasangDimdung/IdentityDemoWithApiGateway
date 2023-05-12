using IdentityServer4.EntityFramework.DbContexts;
using IdentityServer4.EntityFramework.Mappers;
using IdentityServer4.Models;
using Microsoft.EntityFrameworkCore;

namespace IdentityServerDemo;
public static class SeedData
{
    public static void EnsureSeedData(IServiceProvider provider)
    {
        provider.GetRequiredService<PersistedGrantDbContext>().Database.Migrate();
        provider.GetRequiredService<ConfigurationDbContext>().Database.Migrate();

        {
            ConfigurationDbContext context = provider.GetRequiredService<ConfigurationDbContext>();
            if (context.Clients.Any() == false)
            {
                context.Clients.AddRange(Config.GetClients().Select(x => x.ToEntity()));
                context.SaveChanges();
            }

            if (context.IdentityResources.Any() == false)
            {
                context.IdentityResources.AddRange(Config.GetIdentityResources().Select(x => x.ToEntity()));
                context.SaveChanges();
            }

            if (context.ApiResources.Any() == false)
            {
                context.ApiResources.AddRange(Config.GetApis().Select(x => x.ToEntity()));
                context.SaveChanges();
            }
            if (context.ApiResources.Any() == false)
            {
                context.ApiResources.AddRange(Config.GetApis().Select(x => x.ToEntity()));
                context.SaveChanges();
            }
            if (context.ApiScopes.Any() == false)
            {
                context.ApiScopes.AddRange(Config.GetApiScopes().Select(x => x.ToEntity()));
                context.SaveChanges();
            }
        }
    }
}
public static class Config
{
    public static IEnumerable<IdentityResource> GetIdentityResources()
    {
        return new List<IdentityResource>
            {
                new IdentityResources.OpenId(),
                new IdentityResources.Profile()
            };
    }

    public static IEnumerable<ApiScope> GetApiScopes()
    {
        return new List<ApiScope>
            {
                new ApiScope("demo.service"),
                new ApiScope("demo.service2")
            };
    }

    public static IEnumerable<ApiResource> GetApis()
    {
        return new List<ApiResource>
            {
                new ApiResource("demo.service")
                {
                    Scopes = { "demo.service" }
                },
                 new ApiResource("demo.service2")
                {
                    Scopes = { "demo.service2" }
                }
            };
    }

    public static IEnumerable<Client> GetClients()
    {
        return new List<Client>
            {
                new Client
                {
                    ClientId = "client_mvc",
                    ClientName = "MVC Client",
                    AllowedGrantTypes = GrantTypes.Code,
                    RequireConsent = false,
                    RequirePkce = false,
                    ClientSecrets =
                    {
                        new Secret("74ba35a3-e6ee-470f-b8d6-9c27f670025a".Sha256())
                    },
                    RedirectUris =
                    {
                        "http://localhost:1701/signin-oidc"
                    },   
                    PostLogoutRedirectUris =
                    {
                        "http://localhost:1701/signout-callback-oidc"
                    },
                    AllowedScopes = {"openid", "profile", "demo.service", "demo.service2"}
                }
            };
    }
}

