using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBank
{
    internal interface IBankServiceProvider
    {
        void CreateAccount(Customer customer, string accountType, decimal balance);
        void ListAccounts();
        void CalculateInterest();
    }
}
