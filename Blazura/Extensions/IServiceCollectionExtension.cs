using System.Reflection;
using JavaScriptEngineSwitcher.Extensions.MsDependencyInjection;
using JavaScriptEngineSwitcher.V8;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.FileProviders;
using NetCore.AutoRegisterDi;
using NUglify.JavaScript;
using WebOptimizer;
using WebOptimizer.Processors;


namespace Blazura.Extensions;

public static class IServiceCollectionExtension
{
    public static IServiceCollection AddBlazuraOptimizer(this IServiceCollection services, bool verbose = false)
    {
        if (verbose)
        {
            Console.WriteLine("============");
            foreach (var item in Assembly.GetCallingAssembly().GetManifestResourceNames() ?? [])
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("============");
            foreach (var item in typeof(IServiceCollectionExtension).Assembly.GetManifestResourceNames() ?? [])
            {
                Console.WriteLine(item);
            }
            Console.WriteLine("============");
        }

        services.AddJsEngineSwitcher(options => options.DefaultEngineName = V8JsEngine.EngineName).AddV8();
        ManifestEmbeddedFileProvider libraryFP = new(typeof(IServiceCollectionExtension).Assembly, "/");
        ManifestEmbeddedFileProvider callingFP = new(Assembly.GetCallingAssembly(), "/");
        JsSettings codeSettings = new()
        {
            CodeSettings = new()
            {
                MinifyCode = true,
                LocalRenaming = LocalRenaming.KeepAll,
                //AmdSupport = true,
            },
        };
        services.AddWebOptimizer(pipeline =>
        {
            IAsset libraryCss = pipeline.AddScssBundle(
                "/css/blazura.css",
                "/Styles/Common.scss",
                "/Styles/**/*.scss",
                "/Scripts/**/*.scss",
                "/Components/**/*.scss"
                );
            IAsset callingCss = pipeline.AddScssBundle(
                "/css/site.css",
                "/Styles/**/*.scss",
                "/Scripts/**/*.scss",
                "/Components/**/*.scss");
            IAsset libraryJS = pipeline.AddJavaScriptBundle(
                "/js/blazura.js",
                codeSettings,
                "/Scripts/ClassBringer.js",
                "/**/*.js");
            IAsset callingJS = pipeline.AddJavaScriptBundle(
                "/js/site.js",
                codeSettings,
                "/Scripts/ClassBringer.js",
                "/Scripts/**/*.js");

            libraryCss.UseFileProvider(libraryFP);
            libraryJS.UseFileProvider(libraryFP);
            callingCss.UseFileProvider(callingFP);
            callingJS.UseFileProvider(callingFP);
        });
        return services;
    }

    public static IServiceCollection AddProjectServices(this IServiceCollection services)
    {
        services.RegisterAssemblyPublicNonGenericClasses([Assembly.GetCallingAssembly()])
            .Where(c => c.Name.EndsWith("Service"))
            .AsPublicImplementedInterfaces();
        return services;
    }

    public static IServiceCollection AddBlazuraServices(this IServiceCollection services)
    {
        services.RegisterAssemblyPublicNonGenericClasses()
            .Where(c => c.Name.EndsWith("Service"))
            .AsPublicImplementedInterfaces();
        return services;
    }

    public static IServiceCollection AddBlazuraIdentity<TUser, TDbContext>(this IServiceCollection services) where TUser : IdentityUser where TDbContext : DbContext
    {
        services.Configure<IdentityOptions>(options =>
        {
            // Password
            options.Password.RequireDigit = false;
            options.Password.RequireLowercase = false;
            options.Password.RequireNonAlphanumeric = false;
            options.Password.RequireUppercase = false;
            options.Password.RequiredUniqueChars = 0;

            // User
            options.User.RequireUniqueEmail = true;

            // SignIn
            options.SignIn.RequireConfirmedEmail = true;
        });
        services.AddIdentityCore<TUser>(options => options.SignIn.RequireConfirmedAccount = true)
            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<TDbContext>()
            .AddSignInManager()
            .AddDefaultTokenProviders();

        return services;
    }
}
