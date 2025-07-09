using Blazura.Extensions;
using Blazura.Middleware;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Blazura.Extensions;

public struct TypicalRoles
{
    public const string Administrator = "Administrator";
}

public static class WebApplicationExtension
{
    public static IHost Migrate<T>(this IHost app) where T : DbContext
    {
        using IServiceScope serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using DbContext context = serviceScope.ServiceProvider.GetRequiredService<T>();
        context.Database.Migrate();
        return app;
    }

    public static IHost CreateRoles(this IHost app, string[] roles)
    {
        using IServiceScope serviceScope = app.Services.GetRequiredService<IServiceScopeFactory>().CreateScope();
        using RoleManager<IdentityRole> roleManager = serviceScope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();

        foreach (string roleName in roles.Where(o => !string.IsNullOrWhiteSpace(o)))
        {
            Task.Run(async () =>
            {
                if (!await roleManager.RoleExistsAsync(roleName))
                {
                    await roleManager.CreateAsync(new IdentityRole(roleName));
                }
            }).Wait();
        }
        return app;
    }

    public static IApplicationBuilder UseStandard(this IApplicationBuilder app)
    {
        app.UseHttpsRedirection();
        app.UseWebOptimizer();
        app.UseStaticFiles();
        app.UseAntiforgery();
        return app;
    }

    public static IApplicationBuilder UseDefault<T>(this IApplicationBuilder app) where T : IdentityUser
    {
        app.UseMiddleware<BlazorCookieLoginMiddleware<T>>();
        app.UseMiddleware<BlazorCookieChangePasswordMiddleware<T>>();
        app.UseMiddleware<BlazorCookieResetPasswordMiddleware<T>>();
        return app;
    }

    [Obsolete]
    public static IEndpointRouteBuilder MapDefault(this IEndpointRouteBuilder app)
    {
        app.MapControllers();
        app.MapBlazorHub();
        return app;
    }
}
