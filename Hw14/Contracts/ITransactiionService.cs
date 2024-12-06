using Hw14.Dto;
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
        public List<GetTransactionsDto> GetTransactionsByCardNumber(string cardNumber);
        public float GetTotalTransactionsForToday(string cardNumber);
<<<<<<< HEAD
=======
        public float CalculateFee(string cardNumber, float amount);
>>>>>>> 0b262331e15618efd570bc0889617ef631a4a130
 
    }
}
