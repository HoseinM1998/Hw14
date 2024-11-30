using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Hw14.Entities
{
    public class Card
    {
        public string CardNumber { get; set; }
        public string HolderName { get; set; }
        public float Balance { get; set; }
        public bool IsActive { get; set; }
        public string Password { get; set; }
        //public int WrongPasswordTries { get; set; } = 0;

        public List<Transactiion> Transactions { get; set; } = new List<Transactiion>();
    }
}
