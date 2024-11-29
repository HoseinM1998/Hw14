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

        public List<Transactiion> GetTransactionsByCardNumber(string cardNumber);


    }
}
