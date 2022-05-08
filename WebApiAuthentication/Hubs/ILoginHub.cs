using System.Threading.Tasks;

namespace WebApiAuthentication.Hubs
{
    public interface ILoginHub
    {
        Task Login(Data.Token token);
        Task Register(bool result);
    }
}
