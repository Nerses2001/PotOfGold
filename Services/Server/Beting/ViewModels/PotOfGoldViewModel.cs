using PotOfGold.Services.GameTickets.Models;
using PotOfGold.Services.GameTickets.ViewModel;
using PotOfGold.Services.Server.Beting.Models;
using PotOfGold.Services.Server.Beting.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace PotOfGold.Services.Server.Beting.ViewModels
{
    internal class PotOfGoldViewModel
    {
        public static Dictionary<int, List<User>> PlayingGame { get; } = new Dictionary<int, List<User>>();
        private static List<int> _activeTicetsNumbers = new List<int>();
        private  static List<User> _winUsers = new List<User>();
        private static List<User> _losers = new List<User>();

        private readonly ServerViewModel _serverViewModel;

        public static List<int> ActiveTicetsNumbers
        {
            get=> _activeTicetsNumbers;
        }
        public  PotOfGoldViewModel() 
        {
            _serverViewModel = new ServerViewModel();
        }
        public static List<User> WinUsers
        {
            get => _winUsers;
        }
        public static List<User> Losers 
        {
            get => _losers;
        }
        public void ChakeSendigstartGame(User betUser, HttpListenerContext context)
        {
            if (betUser.SelectedSteps.Min() < 1 || betUser.SelectedSteps.Max() > 21)
            {
                _serverViewModel.SendBadRequestResponse(context,"Steps dont Corect");

            }
            else if (betUser.Price <= 0)
            {
                _serverViewModel.SendBadRequestResponse(context, "Mony <= 0");

            }
            else if (!ActiveTicetsNumbers.Any())
            {
                 _serverViewModel.SendBadRequestResponse(context, "Dont Generat Ticets");
            }
            else if (betUser.BlockNumber < ActiveTicetsNumbers.First()) 
            {
                _serverViewModel.SendBadRequestResponse(context, "BlockNumber is less than the first active ticket number.");

            }
            else
                {
                _serverViewModel.SendResponse(context, 200, "StartGame request received successfully.");
                if (PlayingGame.ContainsKey(betUser.BlockNumber))
                {
                    PlayingGame[betUser.BlockNumber].Add(betUser);
                }
                else
                {
                    List<User> users = new List<User>
                    {
                        betUser
                    };
                    PlayingGame.Add(betUser.BlockNumber, users);
                }

            }
        }
        public void GenerateActiveTicets() 
        {
            if (!TicketsViewModel.TicketModels.Any())
            {
                _activeTicetsNumbers = Enumerable.Range(2, 52).ToList();
            }
            else
            {
                _activeTicetsNumbers = Enumerable.Range(TicketsViewModel.TicketModels.Last().TicketNumber + 2, TicketsViewModel.TicketModels.Last().TicketNumber + 52).ToList();

            }
        }
        
        public void GetTicetWiners(int ticketNumber) 
        {
            if(PlayingGame.ContainsKey(ticketNumber) && TicketsViewModel.TicketModels.Count >= ticketNumber)
            {

                Console.WriteLine(TicketsViewModel.TicketModels.ElementAtOrDefault(ticketNumber - 1).Hash);
                var ticket = TicketsViewModel.TicketModels.ElementAtOrDefault(ticketNumber - 1).Hash;
                    List<int> steps = GenerateSteps(ticket.Substring(ticket.Length - 5));

                    GetWinersAndLosers(PlayingGame[ticketNumber], steps);
                    foreach(var step in _losers) 
                    {
                        Console.WriteLine(step.Address);
                    }
                    foreach (var step in _winUsers)
                    {
                        Console.WriteLine(step.Address);
                    }
                

            }
        }

        private void GetWinersAndLosers(List<User> beting, List<int> correctSteps)  
        {
            _winUsers = beting.Where(user => user.SelectedSteps.Intersect(correctSteps).Any()).ToList();
            _losers = beting.Where(user => !user.SelectedSteps.Intersect(correctSteps).Any()).ToList();


        }
        private List<int> GenerateSteps(string s) 
        {
            List<int> res = new List<int>();
            int tmp = 1;
            int j = 0;

            for(int i = 0; i < s.Length; ++i) 
            {

                if ((int)s[i] % 2 == 0) 
                {
                    tmp +=  1;
                    res.Add(tmp);
                }
                else
                {

                    tmp += 6 - j++;
                    res.Add(tmp);
                }
            }
            return res;
        }
       
    
    }
}
