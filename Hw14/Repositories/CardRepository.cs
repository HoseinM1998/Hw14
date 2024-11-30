using Hw14.Configuration;
using Hw14.Contracts;
using Hw14.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw14.Repositories
{
    public class CardRepository : ICardRepository
    {
        private readonly BankDbContext _context;

        public CardRepository()
        {
            _context = new BankDbContext();
        }

        public Card GetCard(string cardNumber)
        {
            return _context.Cards
                .FirstOrDefault(c => c.CardNumber == cardNumber);
        }
        public float GetCardBalance(string cardNumber)
        {
            return _context.Cards
                .Where(c => c.CardNumber == cardNumber)
                .Select(c => c.Balance)
                .FirstOrDefault();
        }

        public void UpdateCard(Card card)
        {
            _context.Cards.Update(card);
            _context.SaveChanges();
        }
    }
}
