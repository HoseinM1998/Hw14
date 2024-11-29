using Hw14.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw14.Contracts
{
    public interface ITransactiionService
    {
        public bool TransferFunds(string sourceCardNumber, string destinationCardNumber, float amount);

        public List<Transactiion> GetTransactionsByCardNumber(string cardNumber);
        public float GetTotalTransactionsForToday(string cardNumber);


    }
}
