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
                throw new ArgumentException("Can Not Null");
            }
            if (sourceCardNumber.Length != 16 )
            {
                throw new ArgumentException("SourceCardNumber Is Not Valid");
            }

            if (destinationCardNumber.Length != 16 )
            {
                throw new ArgumentException("DestinationCardNumber Is Not Valid");
            }
            var totalToday = GetTotalTransactionsForToday(sourceCardNumber);

            if (totalToday + amount > 250)
            {
                throw new InvalidOperationException("Maximum Amount Allowed Per Day 250$");
            }

            if (amount == 0)
            {
                throw new ArgumentException("Transfer Amount Must Be Zero");
            }

            var sourceCard = _cardRepository.GetCard(sourceCardNumber);
            var destinationCard = _cardRepository.GetCard(destinationCardNumber);

            if (sourceCard == null || destinationCard == null)
            {
                throw new InvalidOperationException("Not Found");
            }

            if (!sourceCard.IsActive || !destinationCard.IsActive)
            {
                throw new InvalidOperationException("Blocked Your Account");
            }
            float fee = CalculateFee(sourceCardNumber, amount); 
            var amountfee = fee + amount;
            if (sourceCard.Balance <amountfee)
            {
                throw new InvalidOperationException("Insufficient Funds On Source Card");
            }

            _cardRepository.Withdraw(sourceCardNumber, amountfee);

            try
            {
                _cardRepository.Deposit(destinationCardNumber, amountfee);
                isSuccess = true;
                return true;

            }
            catch (Exception e)
            {
                _cardRepository.Deposit(sourceCardNumber, amountfee);
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
                    Fee = fee,
                    IsSuccessful = isSuccess
                };

                _transactionRepository.AddTransaction(transaction);
            }
        }

        public List<GetTransactionsDto> GetTransactionsByCardNumber(string cardNumber)
        {
            if (string.IsNullOrEmpty(cardNumber))
            {
                throw new ArgumentException("Can Not Null", nameof(cardNumber));
            }

            return _transactionRepository.GetListOfTransactions(cardNumber);
        }

        public float GetTotalTransactionsForToday(string cardNumber)
        {
            var transactions = _transactionRepository.DailyWithdrawal(cardNumber);

            return transactions;
        }

        public float CalculateFee(string cardNumber, float amount)
        {
            var card = _cardRepository.GetCard(cardNumber);
            if (card == null)
            {
                throw new InvalidOperationException("Card Not Found");
            }
            float calFee;
            if (amount >= 1000)
            {
                calFee = amount * 0.015f; 
            }
            else
            {
                calFee = amount * 0.005f; 
            }
            _transactionRepository.UpdateTransactionFee(cardNumber, calFee);
            return calFee;
        }

    }
}
