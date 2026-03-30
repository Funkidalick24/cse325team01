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
                    var destination = string.IsNullOrWhiteSpace(returnUrl) ? "tasks" : returnUrl.TrimStart('/');
                    return TypedResults.LocalRedirect($"~/{destination}");
                }

                // On failure, go back to login with error
                return TypedResults.LocalRedirect($"~/login?error=Invalid login attempt.");
            });

            accountGroup.MapPost("/Register", async (
                UserManager<ApplicationUser> userManager,
                [FromForm] string name,
                [FromForm] string email,
                [FromForm] string password,
                [FromForm] string confirmPassword,
                [FromForm] string returnUrl) =>
            {
                var trimmedName = (name ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(trimmedName))
                {
                    return TypedResults.LocalRedirect("~/register?error=Name is required.");
                }

                if (!string.Equals(password, confirmPassword, StringComparison.Ordinal))
                {
                    return TypedResults.LocalRedirect("~/register?error=Passwords do not match.");
                }

                var user = new ApplicationUser { UserName = email, Email = email };
                var result = await userManager.CreateAsync(user, password);

                if (result.Succeeded)
                {
                    await userManager.AddClaimAsync(user, new Claim(ClaimTypes.GivenName, trimmedName));
                    var redirectToLogin = string.IsNullOrWhiteSpace(returnUrl)
                        ? "~/login"
                        : $"~/login?ReturnUrl={Uri.EscapeDataString(returnUrl)}";

                    return TypedResults.LocalRedirect(redirectToLogin);
                }

                // On failure, go back to register with error
                return TypedResults.LocalRedirect($"~/register?error=Registration failed.");
            });

            return accountGroup;
        }
    }
}
