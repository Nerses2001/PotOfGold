using System;
using System.Net;
using System.Text;

namespace PotOfGold.Services.Server.Beting.ViewModel
{
    internal class ServerViewModelBase
    {

        private void SendResponse(HttpListenerContext context, int statusCode, string responseString)
        {
            try
            {
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                context.Response.StatusCode = statusCode;
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);

            }
            catch (Exception ex)
            {
                Console.WriteLine(statusCode);
                Console.WriteLine(context.Response.StatusCode);
                Console.WriteLine(context.Response.ContentLength64);

                Console.WriteLine(ex.Message);
            }
            context.Response.Close();
        }
    }
}