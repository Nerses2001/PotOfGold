using PotOfGold.Services.Api;
using PotOfGold.Services.Api.Constants;
using PotOfGold.Services.GameTickets.Models;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace PotOfGold.Services.GameTickets.ViewModel
{
    class TicketsViewModel
    {
        private  List<TicketModel> TicketModels { get; } = new List<TicketModel>();
        private readonly BlockInfo _blockInfo;
       

        public TicketsViewModel() 
        {
            _blockInfo = new BlockInfo();
        }

        public async Task  MakeTicket() {
            var timer = new System.Threading.Timer(
               async _ => await GetLatestBlockData(),
               null,
               TimeSpan.Zero,
               TimeSpan.FromSeconds(10));
            await Task.Delay(Timeout.Infinite);
        }

        private  async Task GetLatestBlockData()
        {
            TicketModel blockData = await _blockInfo.Get<TicketModel>($"{UrlConst.BaseUrlInLastBlock}{UrlConst.EndUrlInLastBlock}");

            if (blockData != null)
            {
                TicketModels.Add(blockData);

                if (TicketModels.Count > 0) 
                {
                    Console.WriteLine(TicketModels[TicketModels.Count - 1].Number);
                    Console.WriteLine(TicketModels[TicketModels.Count - 1].Hash);

                }
            }
        }

      
    }
}
