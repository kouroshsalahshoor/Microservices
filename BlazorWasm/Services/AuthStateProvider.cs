using BlazorWasm.Services.IServices;
using BlazorWasm.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;
using Shared.Front;
using System.Net.Http.Headers;
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
            var client = _httpClientFactory.CreateClient("xClient");
            var token = await _jS.InvokeAsync<string>("localstorage.getitem", ApplicationConstants.Local_Token);
            if (token == null)
            {
                return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity()));
            }
            client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("bearer", token);
            return new AuthenticationState(new ClaimsPrincipal(new ClaimsIdentity(JwtParser.ParseClaims(token), "jwtAuthType")));
        }
    }
}
