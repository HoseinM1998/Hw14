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
        private Card _currentCard;
        private int _failedCount = 0;
        public CardService()
        {
            _cardRepository = new CardRepository();
        }

        public bool Login(string cardNumber, string password)
        {
            if (cardNumber.Length != 16)
            {
                return false;
            }

            var card = _cardRepository.GetCard(cardNumber);

            if (card == null)
            {
                return false;
            }

            if (!card.IsActive)
            {
                return false; 
            }

            if (card.Password == password)
            {
                _currentCard = card;
                _failedCount = 0;
                return true;
            }
            else
            {
                _failedCount++;

                if (_failedCount == 3)
                {
                    card.IsActive = false; 
                    _cardRepository.UpdateCard(card);
                }

                return false;
            }
        }

        public Card GetCard(string cardNumber)
        {
            return _cardRepository.GetCard(cardNumber); 
        }

        public void Logout()
        {
            _currentCard = null;
        }

        public Card GetCurrentCard()
        {
            return _currentCard;
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

        public bool UpdateCardBalance(string cardNumber, float amount)
        {
            var card = _cardRepository.GetCard(cardNumber);
            if (card == null || !card.IsActive)
            {
                throw new InvalidOperationException("Card Not Found");
            }

            if (amount <= 0)
            {
                throw new InvalidOperationException("Wrong|Balance> 0");
            }
            card.Balance += amount;
            _cardRepository.UpdateCard(card);
            return true;
        }
    }
}


