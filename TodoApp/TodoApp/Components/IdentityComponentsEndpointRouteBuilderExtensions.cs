using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using TodoApp.Models;
using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Routing;
using System.Text.RegularExpressions;

namespace TodoApp.Components
{
    internal static class IdentityComponentsEndpointRouteBuilderExtensions
    {
        private static readonly Regex UsernameRegex = new("^[a-zA-Z0-9._-]{3,32}$", RegexOptions.Compiled);

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
                UserManager<ApplicationUser> userManager,
                [FromForm] string loginIdentifier,
                [FromForm] string password,
                [FromForm] string returnUrl) =>
            {
                var trimmedLogin = (loginIdentifier ?? string.Empty).Trim();
                if (string.IsNullOrWhiteSpace(trimmedLogin))
                {
                    return TypedResults.LocalRedirect("~/login?error=Email or username is required.");
                }

                ApplicationUser? user = null;
                if (trimmedLogin.Contains('@'))
                {
                    user = await userManager.FindByEmailAsync(trimmedLogin);
                }

                user ??= await userManager.FindByNameAsync(trimmedLogin);

                var loginName = user?.UserName ?? trimmedLogin;
                var result = await signInManager.PasswordSignInAsync(loginName, password, isPersistent: true, lockoutOnFailure: false);
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
                [FromForm] string username,
                [FromForm] string email,
                [FromForm] string password,
                [FromForm] string confirmPassword,
                [FromForm] string returnUrl) =>
            {
                var trimmedName = (name ?? string.Empty).Trim();
                var trimmedUserName = (username ?? string.Empty).Trim();
                var trimmedEmail = (email ?? string.Empty).Trim();

                if (string.IsNullOrWhiteSpace(trimmedName))
                {
                    return TypedResults.LocalRedirect("~/register?error=Name is required.");
                }

                if (string.IsNullOrWhiteSpace(trimmedUserName))
                {
                    return TypedResults.LocalRedirect("~/register?error=Username is required.");
                }

                if (!UsernameRegex.IsMatch(trimmedUserName))
                {
                    return TypedResults.LocalRedirect("~/register?error=Username must be 3-32 characters and use only letters, numbers, ., _, or -.");
                }

                if (string.IsNullOrWhiteSpace(trimmedEmail))
                {
                    return TypedResults.LocalRedirect("~/register?error=Email is required.");
                }

                if (!string.Equals(password, confirmPassword, StringComparison.Ordinal))
                {
                    return TypedResults.LocalRedirect("~/register?error=Passwords do not match.");
                }

                if (await userManager.FindByNameAsync(trimmedUserName) is not null)
                {
                    return TypedResults.LocalRedirect("~/register?error=Username is already taken.");
                }

                if (await userManager.FindByEmailAsync(trimmedEmail) is not null)
                {
                    return TypedResults.LocalRedirect("~/register?error=Email is already in use.");
                }

                var user = new ApplicationUser { UserName = trimmedUserName, Email = trimmedEmail };
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
                var firstError = result.Errors.FirstOrDefault()?.Description ?? "Registration failed.";
                return TypedResults.LocalRedirect($"~/register?error={Uri.EscapeDataString(firstError)}");
            });

            return accountGroup;
        }
    }
}
