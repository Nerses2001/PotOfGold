using PotOfGold.Services.Api.Constants;
using PotOfGold.Services.GameTickets.ViewModel;
using PotOfGold.Services.Server.Beting.Models;
using PotOfGold.Services.Server.Beting.ViewModel;
using PotOfGold.Services.Server.Beting.ViewModels;
using System;
using System.Linq;
using System.Net;

namespace PotOfGold.Services.Server
{
    internal class ServerProvider: IDisposable
    {
        private readonly HttpListener _listener;
        private readonly ServerViewModel _viewModel;
        private readonly PotOfGoldViewModel _potOfGoldViewModel;

      //  private readonly TicketsViewModel _ticketsView= new TicketsViewModel();

        public ServerProvider()
        {
            _listener = new HttpListener();
            _viewModel = new ServerViewModel();
            _potOfGoldViewModel = new PotOfGoldViewModel();
        }

     


        public void Start(int port)
        {
            if (!HttpListener.IsSupported)
            {
                Console.WriteLine("HttpListener is not supported on this platform.");
                return;
            }

            _listener.Prefixes.Add($"{UrlConst.ServerUrl}:{port}/");
            _listener.Start();
            Console.WriteLine("Server started.");
            Console.WriteLine($"{UrlConst.ServerUrl}:{port}/");
            HandleRequests();


        }
        private void HandleRequests()
        {
            while (_listener.IsListening)
            {
                try
                {
                    HttpListenerContext context =  _listener.GetContext();
                    ProcessRequest(context);
                }
                catch (HttpListenerException ex)
                {
                    Console.WriteLine($"Error: {ex.Message}");
                }
            }
        }
        private void ProcessRequest(HttpListenerContext context)
        {
            //request.Url.AbsolutePath
            Console.WriteLine(context.Request.Url.AbsolutePath);
            switch (context.Request.Url.AbsolutePath)
            {
                case UrlConst.endUrlStartGame:
                    var betUser = _viewModel.HandlePostRequestAsync<User>(context, UrlConst.endUrlStartGame);
                        _potOfGoldViewModel.ChakeSendigstartGame(betUser, context);
                    
                    break;
                case UrlConst.endUrlEndBlocks:
                    _viewModel.HandleGetRequestAsync(context, UrlConst.endUrlEndBlocks, TicketsViewModel.TicketModels);
                    break;
                case UrlConst.activTicets:
                    _potOfGoldViewModel.GenerateActiveTicets();
                    _viewModel.HandleGetRequestAsync(context, UrlConst.activTicets, PotOfGoldViewModel.ActiveTicetsNumbers);

                    break;



            }

            context.Response.Close();
            
        }

        

        private  void Stop()
        {
            _listener.Stop();
            _listener.Close();
            Console.WriteLine("Server stopped.");
        }
        public void Dispose()
        {
            Stop();
        }

      

       
    }
}
