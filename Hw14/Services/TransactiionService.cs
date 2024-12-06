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
<<<<<<< HEAD

            if (string.IsNullOrEmpty(sourceCardNumber) || string.IsNullOrEmpty(destinationCardNumber))
            {
                throw new ArgumentException("Card numbers cannot be null or empty");
            }

            if (sourceCardNumber.Length < 16 || sourceCardNumber.Length > 16)
            {
                throw new ArgumentException("sourceCardNumber is not valid");
            }

            if (destinationCardNumber.Length < 16 || destinationCardNumber.Length > 16)
            {
                throw new ArgumentException("destinationCardNumber is not valid");
            }

=======
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
>>>>>>> 0b262331e15618efd570bc0889617ef631a4a130
            var totalToday = GetTotalTransactionsForToday(sourceCardNumber);

            if (totalToday + amount > 250)
            {
                throw new InvalidOperationException("Maximum Amount Allowed Per Day 250$");
            }

<<<<<<< HEAD
            if (amount <= 0)
            {
                throw new ArgumentException("Transfer amount must be greater than zero");
=======
            if (amount == 0)
            {
                throw new ArgumentException("Transfer Amount Must Be Zero");
>>>>>>> 0b262331e15618efd570bc0889617ef631a4a130
            }

            var sourceCard = _cardRepository.GetCard(sourceCardNumber);
            var destinationCard = _cardRepository.GetCard(destinationCardNumber);

            if (sourceCard == null || destinationCard == null)
            {
<<<<<<< HEAD
                throw new InvalidOperationException("One or both cards are not found");
=======
                throw new InvalidOperationException("Not Found");
>>>>>>> 0b262331e15618efd570bc0889617ef631a4a130
            }

            if (!sourceCard.IsActive || !destinationCard.IsActive)
            {
                throw new InvalidOperationException("Blocked Your Account");
            }
<<<<<<< HEAD
            float fee = 0;
            if (amount > 1000)
            {
                fee = amount * 0.015f; 
            }
            else
            {
                fee = amount * 0.005f; 
            }

            var totalAmount = amount + fee;

            if (sourceCard.Balance < totalAmount)
            {
                throw new InvalidOperationException("Insufficient funds on source card");
            }

            _cardRepository.Withdraw(sourceCardNumber, totalAmount);

            try
            {
                _cardRepository.Deposit(destinationCardNumber, amount);
=======
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
>>>>>>> 0b262331e15618efd570bc0889617ef631a4a130
                isSuccess = true;
                return true;
            }
            catch (Exception e)
            {
<<<<<<< HEAD
                _cardRepository.Deposit(sourceCardNumber, totalAmount); 
                isSuccess = false;
                throw new InvalidOperationException("Failed to transfer, amount returned to source card");
=======
                _cardRepository.Deposit(sourceCardNumber, amountfee);
                isSuccess = false;
                throw new InvalidOperationException("Filed|Return Amont");
>>>>>>> 0b262331e15618efd570bc0889617ef631a4a130
            }
            finally
            {
                var transaction = new Transactiion()
                {
<<<<<<< HEAD
                    SourceCardNumber = sourceCard.Id,
                    DestinationCardNumber = destinationCard.Id,
                    Amount = amount,
                    TransactionDate = DateTime.Now,
                    Fee = fee, 
                    IsSuccessful = isSuccess
                };

=======
                    SourceCardNumber = sourceCard.Id,  
                    DestinationCardNumber = destinationCard.Id,
                    Amount = amount,
                    TransactionDate = DateTime.Now,
                    Fee = fee,
                    IsSuccessful = isSuccess
                };
>>>>>>> 0b262331e15618efd570bc0889617ef631a4a130
                _transactionRepository.AddTransaction(transaction);
            }
        }

<<<<<<< HEAD


=======
>>>>>>> 0b262331e15618efd570bc0889617ef631a4a130
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

<<<<<<< HEAD
=======
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

>>>>>>> 0b262331e15618efd570bc0889617ef631a4a130
    }
}
