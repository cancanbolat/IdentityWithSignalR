using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Cryptography;
using System.Text;
using WebApiAuthentication.Data;

namespace WebApiAuthentication.Handlers
{
    public class TokenHandler
    {
        private readonly IConfiguration _configuration;

        public TokenHandler(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Data.Token CreateAccessToken(int duration)
        {
            Data.Token tokenInstance = new Data.Token();

            //security key'in simetriğini alıyoruz
            SymmetricSecurityKey securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Token:SecurityKey"]));

            //Şifrelenmiş kimliği oluşturuyoruz
            SigningCredentials signingCredentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            //oluşturulacak token ayarlarını veriyoruz
            tokenInstance.Expiration = System.DateTime.Now.AddMinutes(duration);
            JwtSecurityToken jwtSecurityToken = new JwtSecurityToken(
                issuer: _configuration["Token:Issuer"],
                audience: _configuration["Token:Audience"],
                expires: tokenInstance.Expiration,
                notBefore: System.DateTime.Now, //token üretildikten kaç dk sonra devreye girecek ? 
                signingCredentials: signingCredentials
                );

            //token oluşturucu sınıfında bir örnek alıyoruz
            JwtSecurityTokenHandler tokenHandler = new JwtSecurityTokenHandler();

            //Token üretiyoruz
            tokenInstance.AccessToken = tokenHandler.WriteToken(jwtSecurityToken);

            //Refresh token üretiyoruz
            tokenInstance.RefreshToken = CreateRefreshToken(); //GUID de dönülebilir.

            return tokenInstance;
        }

        //refresh token üretecek metot
        public string CreateRefreshToken()
        {
            byte[] number = new byte[32];
            using (RandomNumberGenerator random = RandomNumberGenerator.Create())
            {
                random.GetBytes(number);
                return Convert.ToBase64String(number);
            }
        }
    }
}
