using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace HMBank
{
    internal interface IBankRepository
    {
        
            void CreateAccount(Customer customer, string accType, decimal balance);
            List<Account> ListAccounts();
        decimal GetAccountBalance(long accountNumber);
        decimal Deposit(long accountNumber, decimal amount);
        decimal Withdraw(long accountNumber, decimal amount);
            void Transfer(long from, long to, decimal amount);
            int CalculateInterest();
            Account GetAccountDetails(long accountNumber);
            List<Transactions> GetTransactions(DateTime from, DateTime to);
        }
    }


