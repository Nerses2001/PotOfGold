using PotOfGold.Services.GameTickets.ViewModel;
using PotOfGold.Services.Server;
using System.Threading.Tasks;

namespace PotOfGold
{
    internal class Program
    {
        static async Task Main()
        {
            var serverProvider = new ServerProvider();
            Task startServer = Task.Run(() => serverProvider.StartServer(8080));

            var ticketsViewModel = new TicketsViewModel();
            Task getTickets = Task.Run(() => ticketsViewModel.MakeTicket());
            await Task.WhenAny(startServer, getTickets);


        }
    }
}
