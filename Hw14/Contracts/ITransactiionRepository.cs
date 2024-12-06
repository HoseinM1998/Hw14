using Hw14.Dto;
using Hw14.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw14.Contracts
{
    public interface ITransactiionRepository
    {
        public void AddTransaction(Transactiion transaction);
        float DailyWithdrawal(string cardNumber);
        public List<GetTransactionsDto> GetListOfTransactions(string cardNumber);
<<<<<<< HEAD
=======
        public void UpdateTransactionFee(string cardNumber, float fee);



>>>>>>> 0b262331e15618efd570bc0889617ef631a4a130
    }
}
