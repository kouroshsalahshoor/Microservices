using System.Security.Claims;
using System.Text.Json;

namespace BlazorWasm.Utilities
{
    public static class JwtParser
    {
        public static List<Claim> ParseClaims(string jwt)
        {
            var claims = new List<Claim>();
            if (string.IsNullOrEmpty(jwt) == false)
            {
                var payload = jwt.Split('.')[1];
                var jsonBytes = parseBase64WithoutPadding(payload);
                var pairs = JsonSerializer.Deserialize<Dictionary<string, object>>(jsonBytes);
                claims.AddRange(pairs.Select(x => new Claim(x.Key, x.Value.ToString())));
            }
            return claims;
        }

        private static byte[] parseBase64WithoutPadding(string base64)
        {
            switch (base64.Length % 4)
            {
                case 2: base64 += "=="; break;
                case 3: base64 += "="; break;
            }
            return Convert.FromBase64String(base64);
        }
    }
}
