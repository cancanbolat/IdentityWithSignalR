using System.Threading.Tasks;

namespace WebApiAuthentication.Hubs
{
    public interface IMessageHub
    {
        Task ReceiverMessage(string message);
    }
}
