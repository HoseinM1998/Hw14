using Hw14.Contracts;
using Hw14.Entities;
using Hw14.Repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw14.Services
{
    public class CardService : ICardService
    {
        private readonly ICardRepository _cardRepository;
        public CardService()
        {
            _cardRepository = new CardRepository();
        }

        public bool Login(string cardNumber, string password)
        {
            var tryCount = _cardRepository.GetWrongPasswordTry(cardNumber);

            if (tryCount > 3)
            {
                return false;
            }

            var passwordIsValid = _cardRepository.PasswordIsValid(cardNumber, password);

            if (passwordIsValid == false)
            {
                _cardRepository.SetWrongPasswordTry(cardNumber);
                return false;
            }
            else
            {
                _cardRepository.ClearWrongPasswordTry(cardNumber);
                return true;
            }
        }


        public float GetCardBalance(string cardNumber)
        {
            var balance = _cardRepository.GetCardBalance(cardNumber);
            if (balance == 0)
            {
                throw new Exception("Balance is Zero");
            }
            return balance;
        }

        public bool GetCardOnline(string cardNumber)
        {
            _cardRepository.GetCard(cardNumber);
            if(cardNumber == null)
            {
                return false;
                throw new Exception("Not Found");

            }
            return true;
        }

        public string ChangePassword(string cardNumber, string newPass)
        {
            if(newPass == null)
            {
                return "Password Not Null";
            }
            if(newPass.Length>4 && newPass.Length<4)
            {
                return "Password in == 4Digit";
            }
             _cardRepository.Changepassword(cardNumber, newPass);
                return "Change password Successful";
        }

        public Card GetCardByCardNumber(string cardNumber)
        {
            var card = _cardRepository.GetCardByCardNumber(cardNumber);
            if (card == null)
            {
                Console.WriteLine("Card not found!");
                return null;
            }
            return card;
        }


    }
}


