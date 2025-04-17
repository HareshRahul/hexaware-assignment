using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HMBank
{
    internal interface ICustomerProvider
    {
        decimal GetAccountBalance(long accountNumber);
        decimal Deposit(long accountNumber, decimal ammount);
        decimal Withdraw(long accountNumber, decimal amount);
        void Transfer(long from, long to, decimal amount);
        string GetAccountDetails(long accountNumber);
        List<Transactions> GetTransactions(DateTime from, DateTime to);
    }
}
