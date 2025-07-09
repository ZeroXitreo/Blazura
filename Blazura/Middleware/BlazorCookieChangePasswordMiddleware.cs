using System.Collections.Concurrent;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;

namespace Blazura.Middleware;

public class BlazorCookieChangePasswordMiddleware<T>(RequestDelegate next) where T : IdentityUser
{
    public static IDictionary<Guid, EditAccountModel> PasswordChanges { get; private set; } = new ConcurrentDictionary<Guid, EditAccountModel>();

    public async Task Invoke(HttpContext context, UserManager<T> userManager)
    {
        if (context.Request.Query.ContainsKey("changePassword") && Guid.TryParse(context.Request.Query["changePassword"], out Guid token))
        {
            bool success = await ChangePasswordAsync(userManager, token);
            if (success)
            {
                context.Response.Redirect(context.Request.Path);
                return;
            }
        }

        await next.Invoke(context);
    }

    private static async Task<bool> ChangePasswordAsync(UserManager<T> userManager, Guid token)
    {
        if (!PasswordChanges.TryGetValue(token, out var input) || input is null) return false;
        PasswordChanges.Remove(token);

        var user = await userManager.FindByIdAsync(input.UserId!);
        if (user is null) return false;

        IdentityResult result = await userManager.ChangePasswordAsync(user, input.CurrentPassword!, input.NewPasswordOne!);
        return result.Succeeded;
    }
}
