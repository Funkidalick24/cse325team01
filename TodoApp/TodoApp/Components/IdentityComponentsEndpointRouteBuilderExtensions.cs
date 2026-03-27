using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;

namespace TodoApp.Components
{
    internal static class IdentityComponentsEndpointRouteBuilderExtensions
    {
        public static IEndpointConventionBuilder MapAdditionalIdentityEndpoints(this IEndpointRouteBuilder endpoints)
        {
            var accountGroup = endpoints.MapGroup("/Account");

            accountGroup.MapPost("/Logout", async (
                ClaimsPrincipal user,
                SignInManager<ApplicationUser> signInManager,
                [FromForm] string returnUrl) =>
            {
                await signInManager.SignOutAsync();
                return TypedResults.LocalRedirect($"~/{returnUrl}");
            });

            accountGroup.MapPost("/Login", async (
                SignInManager<ApplicationUser> signInManager,
                [FromForm] string email,
                [FromForm] string password,
                [FromForm] string returnUrl) =>
            {
                var result = await signInManager.PasswordSignInAsync(email, password, isPersistent: true, lockoutOnFailure: false);
                if (result.Succeeded)
                {
                    return TypedResults.LocalRedirect($"~/{returnUrl ?? "tasks"}");
                }

                // On failure, go back to login with error
                return TypedResults.LocalRedirect($"~/login?error=Invalid login attempt.");
            });

            accountGroup.MapPost("/Register", async (
                UserManager<ApplicationUser> userManager,
                SignInManager<ApplicationUser> signInManager,
                [FromForm] string email,
                [FromForm] string password,
                [FromForm] string confirmPassword,
                [FromForm] string returnUrl) =>
            {
                var user = new ApplicationUser { UserName = email, Email = email };
                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await signInManager.SignInAsync(user, isPersistent: true);
                    return TypedResults.LocalRedirect($"~/{returnUrl ?? "tasks"}");
                }

                // On failure, go back to register with error
                return TypedResults.LocalRedirect($"~/register?error=Registration failed.");
            });

            return accountGroup;
        }
    }
}
