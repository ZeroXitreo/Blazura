using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blazura.Middleware;

public class BlazorCookieLoginMiddleware<T>(RequestDelegate next) where T : IdentityUser
{
    public static IDictionary<Guid, LoginInput> Logins { get; private set; } = new ConcurrentDictionary<Guid, LoginInput>();

    public async Task Invoke(HttpContext context, SignInManager<T> signInMgr)
    {
        if (context.Request.Query.ContainsKey("key"))
        {
            if (Guid.TryParse(context.Request.Query["key"], out Guid key))
            {
                if (Logins.TryGetValue(key, out var value))
                {
                    await signInMgr.PasswordSignInAsync(value.Email, value.Password, true, false);
                    Logins.Remove(key);
                }
            }
            else
            {
                await signInMgr.SignOutAsync();
            }
            context.Response.Redirect(context.Request.Path);
            return;
        }

        await next.Invoke(context);
    }
}
