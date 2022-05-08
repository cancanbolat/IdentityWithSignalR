using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WebApiAuthentication.Data;
using WebApiAuthentication.Handlers;

namespace WebApiAuthentication.Hubs
{
    public class LoginHub : Hub<ILoginHub>
    {
        private readonly IConfiguration _configuration;
        private readonly TokendbContext _tokendbContext;

        public LoginHub(IConfiguration configuration, Data.TokendbContext tokendbContext)
        {
            _configuration = configuration;
            _tokendbContext = tokendbContext;
        }

        public async Task Register(string userName, string password)
        {
            await _tokendbContext.Users.AddAsync(new User
            {
                Password = password,
                UserName = userName,
            });

            //burdaki Caller = ILoginHub
            await Clients.Caller.Register(await _tokendbContext.SaveChangesAsync() > 0);
        }

        public async Task Login(string userName, string password)
        {
            User user = await _tokendbContext.Users.FirstOrDefaultAsync(x => x.UserName == userName && x.Password == password);
            Token token = null;
            if (user != null)
            {
                TokenHandler tokenHandler = new TokenHandler(_configuration);
                token = tokenHandler.CreateAccessToken(5);
                user.RefreshToken = token.RefreshToken;
                user.RefreshTokenEndDate = token.Expiration.AddMinutes(3);
                await _tokendbContext.SaveChangesAsync();
            }

            await Clients.Caller.Login(user != null ? token : null);
        }
    }
}
