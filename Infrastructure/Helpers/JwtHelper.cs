using Hal.Infrastructure.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;

namespace Hal.Infrastructure.Helpers
{
    public class JwtHelper
    {
        /// <summary>
        /// Encrypt
        /// </summary>
        /// <param name="tokenModel">tokenModel</param>
        /// <param name="option">option</param>
        /// <returns></returns>
        public static string Encrypt(TokenModelJwt tokenModel, JwtBearerOption option)
        {
            var claims = new List<Claim>
            {
                // save uer id to claim
                 new Claim(JwtRegisteredClaimNames.Jti,tokenModel.Uid.ToString()),
                 new Claim(JwtRegisteredClaimNames.Iat,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}"),
                 new Claim(JwtRegisteredClaimNames.Nbf,$"{new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds()}") ,
                 // Expiration time
                 new Claim (JwtRegisteredClaimNames.Exp,$"{new DateTimeOffset(DateTime.Now.AddSeconds(1000)).ToUnixTimeSeconds()}"),
                 new Claim(JwtRegisteredClaimNames.Iss,option.Issuer),
                 new Claim(JwtRegisteredClaimNames.Aud,option.Audience),
            };

            // add roles for sign credentials
            claims.AddRange(tokenModel.Role.Split(',').Select(s => new Claim(ClaimTypes.Role, s)));

            // it can't lower than 16bit
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(option.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var jwt = new JwtSecurityToken(
                issuer: option.Issuer,
                claims: claims,
                signingCredentials: creds);
            var jwtHandler = new JwtSecurityTokenHandler();
            var encodedJwt = jwtHandler.WriteToken(jwt);
            return encodedJwt;
        }

        /// <summary>
        /// DeEncrypt
        /// </summary>
        /// <param name="jwtStr"></param>
        /// <returns></returns>
        public static TokenModelJwt DeEncrypt(string jwtStr)
        {
            var jwtHandler = new JwtSecurityTokenHandler();
            JwtSecurityToken jwtToken = new JwtSecurityToken(jwtStr);
            object role;
            try
            {
                jwtToken.Payload.TryGetValue(ClaimTypes.Role, out role);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw;
            }

            var tm = new TokenModelJwt
            {
                Uid = long.Parse(jwtToken.Id),
                Role = role != null ? role.ToString() : "",
            };

            return tm;
        }
    }

    /// <summary>
    /// token
    /// </summary>
    public class TokenModelJwt
    {
        /// <summary>
        /// Id
        /// </summary>
        public long Uid { get; set; }
        /// <summary>
        /// Role
        /// </summary>
        public string Role { get; set; }
    }
}
