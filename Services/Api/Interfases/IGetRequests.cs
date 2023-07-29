

using System.Threading.Tasks;

namespace PotOfGold.Services.Api.Interfases
{
    interface IGetRequests
    {
        Task<T> Get<T>(string url);
    }
}
