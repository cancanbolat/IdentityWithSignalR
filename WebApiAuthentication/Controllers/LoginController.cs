using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WebApiAuthentication.Data;

namespace WebApiAuthentication.Controllers
{
    [Route("api/[controller]/[action]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly IConfiguration _configuration;
        private readonly TokendbContext _tokendbContext;

        public LoginController(IConfiguration configuration, TokendbContext tokendbContext)
        {
            _configuration = configuration;
            _tokendbContext = tokendbContext;
        }

        //kaydolma
        [HttpPost]
        public async Task<bool> Register([FromForm] User user)
        {
            _tokendbContext.Users.Add(user);
            await _tokendbContext.SaveChangesAsync();
            return true;
        }

        //giriş
        /*
        [HttpPost]
        public async Task<Token> Login([FromForm] UserLogin userLogin)
        {
            User user = await _tokendbContext.Users.FirstOrDefaultAsync(x => x.Email == userLogin.Email && x.Password == userLogin.Password);

            if (user is not null)
            {
                //Token üretiliyor
                Handlers.TokenHandler tokenHandler = new Handlers.TokenHandler(_configuration);
                Token token = tokenHandler.CreateAccessToken(user);

                //Refresh token users tablosuna ekleniyor
                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenEndDate = token.Expiration.AddMinutes(3);
                await _tokendbContext.SaveChangesAsync();

                return token;
            }

            return null;
        }
        */
    }
}
