using PotOfGold.Services.GameTickets.ViewModel;
using PotOfGold.Services.Server.Beting.Models;
using PotOfGold.Services.Server.Beting.ViewModel;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace PotOfGold.Services.Server.Beting.ViewModels
{
    internal class PotOfGoldViewModel
    {
        public static Dictionary<int, List<User>> PlayingGame { get; } = new Dictionary<int, List<User>>();
        private static List<int> _activeTicetsNumbers = new List<int>();

        private readonly ServerViewModel _serverViewModel;

        public static List<int> ActiveTicetsNumbers
        {
            get=> _activeTicetsNumbers;
        }
        public  PotOfGoldViewModel() 
        {
            _serverViewModel = new ServerViewModel();
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
                    List<User> users = new List<User>();
                    users.Add(betUser);
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
    }
}
