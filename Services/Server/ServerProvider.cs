using PotOfGold.Services.GameTickets.ViewModel;
using System;
using System.Net;

namespace PotOfGold.Services.Server
{
    internal class ServerProvider
    {
        private readonly HttpListener _listener;
      //  private readonly TicketsViewModel _ticketsViewModel;

        public ServerProvider()
        {
            _listener = new HttpListener();
         //   _ticketsViewModel = new TicketsViewModel();
        }

        public void StartServer(int port) 
        {
            _listener.Prefixes.Add($"http://localhost:{port}/");
            Console.WriteLine($"Server listening on http://localhost:{port}/");

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

            HttpListenerResponse response = context.Response;

            string responseString = "Hello, this is the server response!";
            byte[] buffer = System.Text.Encoding.UTF8.GetBytes(responseString);
            response.ContentLength64 = buffer.Length;
            response.OutputStream.Write(buffer, 0, buffer.Length);
            response.OutputStream.Close();
        }

        

        ~ServerProvider()
        {
            _listener?.Stop();
        }
    }
}
