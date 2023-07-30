using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace PotOfGold.Services.Server.Beting.Models
{
    internal class User
    {
        public int Price { get; }
        public int BlockNumber { get; }
        public string Address { get; }
        public List<int> SelectedSteps { get; }

        public User(int price, int blockNumber, string address, List<int> selectedSteps)
        {
            Check(price, blockNumber, address, selectedSteps);
            this.Price = price;
            this.BlockNumber = blockNumber;
            this.Address = address;
            this.SelectedSteps = selectedSteps;
        }

        private void Check(int price, int blockNumber, string address, List<int> selectedSteps)
        {
            if (price <= 0) throw new ArgumentOutOfRangeException("Price is <= 0");
            if (blockNumber <= 0) throw new ArgumentOutOfRangeException("Block Number is <= 0");
            if (string.IsNullOrEmpty(address)) throw new ArgumentOutOfRangeException("Address is empty");
            if (selectedSteps.Max() > 21 || selectedSteps.Min() < 1) throw new ArgumentOutOfRangeException("Error Steps, steps should be in the range 1-21");
        }
    }


}
