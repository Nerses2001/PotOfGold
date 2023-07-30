using PotOfGold.Services.Api.Constants;
using PotOfGold.Services.Server.Beting.Models;
using PotOfGold.Services.Server.Beting.ViewModel;
using System;
using System.Net;

namespace PotOfGold.Services.Server
{
    internal class ServerProvider
    {
        private readonly HttpListener _listener;
        private readonly ServerViewModel _viewModel;

        //  private readonly TicketsViewModel _ticketsViewModel;

        public ServerProvider()
        {
            _listener = new HttpListener();
            _viewModel = new ServerViewModel();
         //   _ticketsViewModel = new TicketsViewModel();
        }

        public void StartServer(int port) 
        {
            _listener.Prefixes.Add($"{UrlConst.ServerUrl}{port}/");
            Console.WriteLine($"Server listening on http://localhost:{port}/startgame");

            _listener.Start();
           
            while (true)
            {
                var context = _listener.GetContext();
                ProcessRequest(context);
            }
        }

        private void ProcessRequest(HttpListenerContext context)
        {
            //     _ticketsViewModel.MakeTicket().ConfigureAwait(false);

            _viewModel.HandlePostRequest<User>(context);

        }



        ~ServerProvider()
        {
            _listener?.Stop();
        }
    }
}
