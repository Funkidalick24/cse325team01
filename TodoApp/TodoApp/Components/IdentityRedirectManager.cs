using Microsoft.AspNetCore.Components;
using System;

namespace TodoApp.Components
{
    public sealed class IdentityRedirectManager
    {
        private readonly NavigationManager _navigationManager;

        public IdentityRedirectManager(NavigationManager navigationManager)
        {
            _navigationManager = navigationManager;
        }

        public void RedirectTo(string? uri)
        {
            uri ??= "";

            // Prevent open redirects.
            if (!Uri.IsWellFormedUriString(uri, UriKind.Relative))
            {
                uri = _navigationManager.ToBaseRelativePath(uri);
            }

            _navigationManager.NavigateTo(uri);
        }
    }
}
