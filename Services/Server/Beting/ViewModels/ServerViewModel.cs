using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;

namespace PotOfGold.Services.Server.Beting.ViewModel
{
    class ServerViewModel
    {
        public T HandlePostRequestAsync<T>(HttpListenerContext context, string endUrl)
        {
            try
            {
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                if (request.HttpMethod == "POST")
                {
                    if (request.Url.AbsolutePath == endUrl)
                    {
                        using (Stream body = request.InputStream)
                        {
                            using (StreamReader reader = new StreamReader(body, request.ContentEncoding))
                            {
                                string requestBody = reader.ReadToEnd();
                                Console.WriteLine("Received POST request body:");
                                Console.WriteLine(requestBody);
                                T data = JsonConvert.DeserializeObject<T>(requestBody);
                                if (data == null)
                                {
                                    SendBadRequestResponse(context, "Bad Request: Invalid user model data.");
                                    return default; // or throw an exception here if necessary
                                }
                                else
                                {
                                    return data;
                                }
                            }
                        }
                    }
                    else
                    {
                        SendBadRequestResponse(context, "POST request failed. Status code: 404 (Not Found)");
                        return default; // or throw an exception here if necessary
                    }
                }
                else
                {
                    Console.WriteLine(request.HttpMethod);
                    SendBadRequestResponse(context, "POST request failed. Status code: 404 ");
                    return default; // or throw an exception here if necessary
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Respond with a 500 Internal Server Error status code
                SendServerErrorResponse(context, "Internal Server Error.");
                return default; // or throw an exception here if necessary
            }
        }

        public void HandleGetRequestAsync<T>(HttpListenerContext context, string endUrl, List<T> dataList)
        {
            try
            {
                HttpListenerRequest request = context.Request;
                HttpListenerResponse response = context.Response;

                if (request.HttpMethod == "GET")
                {
                    if (request.Url.AbsolutePath == endUrl)
                    {
                        string jsonResponse = JsonConvert.SerializeObject(dataList);
                        SendResponse(context, 200, jsonResponse);
                    }
                    else
                    {
                        SendBadRequestResponse(context, "GET request failed. Status code: 404 (Not Found)");
                    }
                }
                else
                {
                    Console.WriteLine(request.HttpMethod);
                    SendBadRequestResponse(context, "GET request failed. Status code: 404");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
                // Respond with a 500 Internal Server Error status code
                SendServerErrorResponse(context, "Internal Server Error.");
            }
        }

        public void SendResponse(HttpListenerContext context, int statusCode, string responseString)
        {
            try 
            {
                byte[] buffer = Encoding.UTF8.GetBytes(responseString);
                context.Response.StatusCode = statusCode;
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);
                context.Response.Close();

            }
            catch (Exception ex)
            {
      
              
                Console.WriteLine(ex.Message);
            }
        }

        public void SendBadRequestResponse(HttpListenerContext context, string responseString)
        {
            SendResponse(context, 400, responseString);
        }
      

        private void SendServerErrorResponse(HttpListenerContext context, string responseString)
        {
            SendResponse(context, 500, responseString);
        }



    }
}
