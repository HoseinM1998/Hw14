﻿using Hw14.Configuration;
using Hw14.Contracts;
using Hw14.Dto;
using Hw14.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw14.Repositories
{

    public class TransactionRepository : ITransactiionRepository
    {
        private readonly BankDbContext _context;

        public TransactionRepository()
        {
            _context = new BankDbContext();
        }

        public void AddTransaction(Transactiion transaction)
        {
            _context.Transactions.Add(transaction);
            _context.SaveChanges();
        }

        public List<GetTransactionsDto> GetListOfTransactions(string cardNumber)
        {
            return _context.Transactions
                .Where(x => x.SourceCard.CardNumber == cardNumber || x.DestinationCard.CardNumber == cardNumber)
                .Select(x => new GetTransactionsDto
                {
                    SourceCardNumber = x.SourceCard.CardNumber,
                    DestinationsCardNumber = x.DestinationCard.CardNumber,
                    TransactionDate = x.TransactionDate,
                    Amount = x.Amount,
                    IsSuccess = x.IsSuccessful,
                }).ToList();
        }

        public float DailyWithdrawal(string cardNumber)
        {
            var amountOfTransactions = _context.Transactions
                .Where(x => x.TransactionDate == DateTime.Now.Date && x.SourceCard.CardNumber == cardNumber)
                .Sum(x => x.Amount);
            return amountOfTransactions;
        }
    }

}

