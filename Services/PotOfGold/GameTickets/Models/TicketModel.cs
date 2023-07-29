using System;
using Newtonsoft.Json;

namespace PotOfGold.Services.GameTickets.Models
{
    internal class TicketModel
    {
        public bool IsActictive { get; set; } = true;
        public int Number { get; }
        public  string Hash { get; }


        public TicketModel(int  number, string hash) 
        {
            if (string.IsNullOrEmpty(hash))
                throw new ArgumentException("hash cannot be empty or null");

            this.Number = number;
            this.Hash = hash;
        }
      /*  public static TicketModel FromJson(string json)
        {
            return JsonConvert.DeserializeObject<TicketModel>(json);
        }
      */


    }
}
