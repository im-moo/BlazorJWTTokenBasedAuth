using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace BlazorSimpleApplications.Data
{
    public class TokenManager
    {
        public static string SecretKey = "DQpUZXN0DQoNCkNvbmZpZGVudGlhbGl0eSBOb3RpY2U6IFRoaXMgZS1tYWlsLCBhbmQgYW55IGF0dGFjaG1lbnQgdG8gaXQsIGNvbnRhaW5zIHByaXZpbGVnZWQgYW5kIGNvbmZpZGVudGlhbCBpbmZvcm1hdGlvbiBpbnRlbmRlZCBvbmx5IGZvciB0aGUgdXNlIG9mIHRoZSBpbmRpdmlkdWFsKHMpIG9yIGVudGl0eSBuYW1lZCBvbiB0aGUgZS1tYWlsLiBJZiB0aGUgcmVhZGVyIG9mIHRoaXMgZS1tYWlsIGlzIG5vdCB0aGUgaW50ZW5kZWQgcmVjaXBpZW50LCBvciB0aGUgZW1wbG95ZWUgb3IgYWdlbnQgcmVzcG9uc2libGUgZm9yIGRlbGl2ZXJpbmcgaXQgdG8gdGhlIGludGVuZGVkIHJlY2lwaWVudCwgeW91IGFyZSBoZXJlYnkgbm90aWZpZWQgdGhhdCByZWFkaW5nIHRoaXMgZS1tYWlsIGlzIHN0cmljdGx5IHByb2hpYml0ZWQuIElmIHlvdSBoYXZlIHJlY2VpdmVkIHRoaXMgZS1tYWlsIGluIGVycm9yLCBwbGVhc2UgaW1tZWRpYXRlbHkgcmV0dXJuIGl0IHRvIHRoZSBzZW5kZXIgYW5kIGRlbGV0ZSBpdCBmcm9tIHlvdXIgc3lzdGVtLg0K";

        public static  string GenerateToken(string username)
        {
            byte[] key = Convert.FromBase64String(SecretKey);
            SymmetricSecurityKey symmetricSecurityKey = new SymmetricSecurityKey(key);
            SecurityTokenDescriptor descriptor = new SecurityTokenDescriptor()
            {
                Subject = new ClaimsIdentity(new List<Claim>()
                {
                    new Claim(ClaimTypes.Name,username),
                }),
                Expires = DateTime.UtcNow.AddMinutes(45),
                SigningCredentials = new SigningCredentials(symmetricSecurityKey,SecurityAlgorithms.HmacSha256Signature)
            };
            JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
            JwtSecurityToken securityToken = handler.CreateJwtSecurityToken(descriptor);
            return handler.WriteToken(securityToken);
        }

        public static ClaimsPrincipal GetPrincipal(string token)
        {
            try
            {
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                JwtSecurityToken jwtToken = (JwtSecurityToken) handler.ReadToken(token);
                if (jwtToken == null) return null;
                byte[] key = Convert.FromBase64String(SecretKey);
                TokenValidationParameters parameters = new TokenValidationParameters()
                {
                    RequireExpirationTime = true,
                    ValidateIssuer = false,
                    ValidateAudience=false,
                    IssuerSigningKey = new SymmetricSecurityKey(key)
                };
                SecurityToken securityToken;
                ClaimsPrincipal claimsPrincipal = handler.ValidateToken(token,parameters,out securityToken);
                return claimsPrincipal;
            }
            catch (Exception )
            {
                return null;
            }
        }

        public static string ValidateToken(string token)
        {
            string username = null;

            ClaimsPrincipal claimsPrincipal = GetPrincipal(token);
            if (claimsPrincipal == null) return null;
            ClaimsIdentity identity;
            try
            {
                identity = (ClaimsIdentity) claimsPrincipal.Identity;
            }
            catch (NullReferenceException)
            {
                return null;
            }

            Claim claim = identity.FindFirst(ClaimTypes.Name);
            username = claim.Value;
            return username;
        }
    }
}
