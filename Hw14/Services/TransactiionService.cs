using Hw14.Contracts;
using Hw14.Entities;
using Hw14.Repositories;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static Hw14.Repositories.TransactionRepository;

namespace Hw14.Services
{
    public class TransactionService : ITransactiionService
    {
        private readonly CardRepository _cardRepository;
        private readonly TransactionRepository _transactionRepository;
        public TransactionService(TransactionRepository transactionRepository, CardRepository cardRepository)
        {
            _transactionRepository = transactionRepository;
            _cardRepository = cardRepository;
        }
        public bool TransferFunds(string sourceCardNumber, string destinationCardNumber, float amount)
        {
            if (string.IsNullOrEmpty(sourceCardNumber) || string.IsNullOrEmpty(destinationCardNumber))
            {
                throw new ArgumentException("Card numbers cannot be null or empty");
            }
            var totalToday = GetTotalTransactionsForToday(sourceCardNumber);

            if (totalToday + amount > 250)
            {
                throw new InvalidOperationException("Maximum Amount Allowed Per Day 250$");
            }

            if (amount <= 0)
            {
                throw new ArgumentException("Transfer amount must be greater than zero");
            }

            var sourceCard = _cardRepository.GetCard(sourceCardNumber);
            var destinationCard = _cardRepository.GetCard(destinationCardNumber);

            if (sourceCard == null || destinationCard == null)
            {
                throw new InvalidOperationException("One or both cards are not found");
            }

            if (!sourceCard.IsActive || !destinationCard.IsActive)
            {
                throw new InvalidOperationException("One or both cards are inactive");
            }

            if (sourceCard.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds on source card");
            }

            try
            {
                sourceCard.Balance -= amount;
                destinationCard.Balance += amount;

                var transactionRecord = new Transactiion
                {
                    SourceCardNumber = sourceCardNumber,
                    DestinationCardNumber = destinationCardNumber,
                    Amount = amount,
                    TransactionDate = DateTime.Now,
                    IsSuccessful = true
                };

                _transactionRepository.AddTransaction(transactionRecord);
                _cardRepository.UpdateCard(sourceCard);
                _cardRepository.UpdateCard(destinationCard);
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");

                Console.WriteLine($"Inner Exception: {ex.InnerException.Message}");

                sourceCard.Balance += amount;
                _cardRepository.UpdateCard(sourceCard);
                throw new InvalidOperationException("Error Amount returned");

            }
        }

        public List<Transactiion> GetTransactionsByCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                throw new ArgumentException("Card number cannot be null or empty", nameof(cardNumber));
            }

            return _transactionRepository.GetTransactionsByCardNumber(cardNumber);
        }

        public float GetTotalTransactionsForToday(string cardNumber)
        {
            var today = DateTime.Today;
            var transactions = _transactionRepository.GetTransactionsByCardNumber(cardNumber)
                                                      .Where(t => t.TransactionDate.Date == today)
                                                      .Sum(t => t.Amount);

            return transactions;
        }
    }
}
