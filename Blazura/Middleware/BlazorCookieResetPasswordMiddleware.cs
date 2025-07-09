using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blazura.Middleware;

public class BlazorCookieResetPasswordMiddleware<T>(RequestDelegate next) where T : IdentityUser
{
    public static ConcurrentDictionary<Guid, ResetPasswordInput> PasswordResets { get; } = [];

    public async Task Invoke(HttpContext context, SignInManager<T> signInMgr, UserManager<T> userManager)
    {
        if (context.Request.Query.ContainsKey("resetToken") && Guid.TryParse(context.Request.Query["resetToken"], out Guid token))
        {
            bool success = await ResetPasswordAsync(signInMgr, userManager, token);
            if (success)
            {
                //context.Response.Redirect(context.Request.Path);
                context.Response.Redirect("/account");
                return;
            }
        }

        await next.Invoke(context);
    }

    private static async Task<bool> ResetPasswordAsync(SignInManager<T> signInMgr, UserManager<T> userManager, Guid token)
    {
        if (!PasswordResets.TryRemove(token, out ResetPasswordInput? input) || input is null) return false;

        T? user = await userManager.FindByIdAsync(input.UserId);
        if (user is null) return false;

        IdentityResult result = await userManager.ResetPasswordAsync(user, input.Token, input.Password);
        if (!result.Succeeded) return false;

        await signInMgr.SignInAsync(user, false);
        return true;
    }
}
