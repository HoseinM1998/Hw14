using Hw14.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw14.Contracts
{
    public interface ICardService
    {
        public bool Login(string cardNumber, string password);
        public void Logout();
        public Card GetCurrentCard();
        public float GetCardBalance(string cardNumber);

        public bool UpdateCardBalance(string cardNumber, float amount);
    }
}
