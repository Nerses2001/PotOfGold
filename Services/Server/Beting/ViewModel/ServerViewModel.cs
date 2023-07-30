using Newtonsoft.Json;
using System;
using System.IO;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Text;

namespace PotOfGold.Services.Server.Beting.ViewModel
{
    class ServerViewModel
    {
        
        
        public void HandlePostRequest<T>(HttpListenerContext context) 
        {
            try 
            {
                HttpListenerRequest request = context.Request;
                using (Stream body = request.InputStream)
                {
                    using (StreamReader reader = new StreamReader(body, request.ContentEncoding))
                    {
                        string requestBody = reader.ReadToEnd();
                        Console.WriteLine("Received POST request body:");
                        Console.WriteLine(requestBody);
                        T data = JsonConvert.DeserializeObject<T>(requestBody);
                        if(data == null)
                        {
                            SendBadRequestResponse(context, "Bad Request: Invalid user model data.");

                        }
                        else
                        {
                            SendResponse(context, 200, "StartGame request received successfully.");

                        }

                    }
                }
            }catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Respond with a 500 Internal Server Error status code
                SendServerErrorResponse(context, "Internal Server Error.");
            }
            

        }
        private void SendResponse(HttpListenerContext context, int statusCode, string responseString)
        {
            byte[] buffer = Encoding.UTF8.GetBytes(responseString);
            context.Response.StatusCode = statusCode;
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);
            context.Response.Close();
        }

        private void SendBadRequestResponse(HttpListenerContext context, string responseString)
        {
            SendResponse(context, 400, responseString);
        }

        private void SendServerErrorResponse(HttpListenerContext context, string responseString)
        {
            SendResponse(context, 500, responseString);
        }

    }
}
