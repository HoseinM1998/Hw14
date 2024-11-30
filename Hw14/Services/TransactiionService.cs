using Hw14.Contracts;
using Hw14.Dto;
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
        public TransactionService()
        {
            _transactionRepository = new TransactionRepository();
            _cardRepository = new CardRepository();
        }
        public bool TransferFunds(string sourceCardNumber, string destinationCardNumber, float amount)
        {
            var isSuccess = false;
            if (string.IsNullOrEmpty(sourceCardNumber) || string.IsNullOrEmpty(destinationCardNumber))
            {
                throw new ArgumentException("Card numbers cannot be null or empty");
            }
            if (sourceCardNumber.Length < 16 || sourceCardNumber.Length > 16)
            {
                throw new ArgumentException("sourceCardNumber is not validy");
            }

            if (destinationCardNumber.Length < 16 || destinationCardNumber.Length > 16)
            {
                throw new ArgumentException("sourceCardNumber is not validy");
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
                throw new InvalidOperationException("Blocked Your Account");
            }

            if (sourceCard.Balance < amount)
            {
                throw new InvalidOperationException("Insufficient funds on source card");
            }

            _cardRepository.Withdraw(sourceCardNumber, amount);

            try
            {
                _cardRepository.Deposit(destinationCardNumber, amount);
                isSuccess = true;
                return true;

            }
            catch (Exception e)
            {
                _cardRepository.Deposit(sourceCardNumber, amount);
                isSuccess = false;
                return false;
                throw new InvalidOperationException("Filed|Return Amont");

            }
            finally
            {
                var transaction = new Transactiion()
                {
                    SourceCardNumber = sourceCard.Id,
                    DestinationCardNumber = destinationCard.Id,
                    Amount = amount,
                    TransactionDate = DateTime.Now,
                    IsSuccessful = isSuccess
                };

                _transactionRepository.AddTransaction(transaction);
            }
        }

        public List<GetTransactionsDto> GetTransactionsByCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                throw new ArgumentException("Card number cannot be null or empty", nameof(cardNumber));
            }

            return _transactionRepository.GetListOfTransactions(cardNumber);
        }

        public float GetTotalTransactionsForToday(string cardNumber)
        {
            var transactions = _transactionRepository.DailyWithdrawal(cardNumber);

            return transactions;
        }
    }
}
