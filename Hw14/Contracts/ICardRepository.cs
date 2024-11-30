using Hw14.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw14.Contracts
{
    public interface ICardRepository
    {
        public Card GetCard(string cardNumber);
        public void UpdateCard(Card card);
        public float GetCardBalance(string cardNumber);



    }
}
