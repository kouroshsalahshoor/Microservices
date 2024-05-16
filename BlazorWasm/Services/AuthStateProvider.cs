using BlazorWasm.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Shared.Front;
using System.Security.Claims;

namespace BlazorWasm.Services
{
    public class AuthStateProvider : AuthenticationStateProvider
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly IJSRuntime _jS;

        public AuthStateProvider(IHttpClientFactory httpClientFactory, IJSRuntime jS)
        {
            _httpClientFactory = httpClientFactory;
            _jS = jS;
        }
        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            //var client = _httpClientFactory.CreateClient("xClient");
            var token = await _jS.InvokeAsync<string>("localStorage.getItem", ApplicationConstants.Local_Token);
            if (string.IsNullOrEmpty(token) || token == "null")
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
                ////for testing
                //return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(new[]
                //{
                //    new Claim(ClaimTypes.Name, "XXX")
                //},
                //    "jwtAuthType")));
            }
            //client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaims(token), "jwtAuthType")));
        }

        public void NotifyUserLogin(string token)
        {
            var authenticatedUser = new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaims(token), "jwtAuthType"));
            var authState = Task.FromResult(new AuthenticationState(authenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }

        public void NotifyUserLogout()
        {
            var unAuthenticatedUser = new ClaimsPrincipal(new ClaimsIdentity());
            var authState = Task.FromResult(new AuthenticationState(unAuthenticatedUser));
            NotifyAuthenticationStateChanged(authState);
        }
    }
}
