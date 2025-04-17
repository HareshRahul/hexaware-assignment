using System;
using HMBank.Exceptions;

namespace HMBank
{
    public class Account
    {
        public long accountNumber { get; set; }
        public string accountType { get; set; }
        public Customer customer { get; set; }
        public decimal accountBalance { get; set; }

        public Account()
        {
            // Default constructor - no need for try-catch here
        }

        public Account(long accountNumber, Customer customer, string accountType, decimal balance)
        {
            try
            {
                this.accountNumber = accountNumber;
                this.accountType = accountType ?? throw new ArgumentNullException(nameof(accountType), "Account type cannot be null.");
                this.customer = customer ?? throw new ArgumentNullException(nameof(customer), "Customer cannot be null.");
                this.accountBalance = balance;
            }
            catch (ArgumentNullException ex)
            {
                Console.WriteLine($"Initialization error: {ex.Message}");
                throw; // Re-throw if you want to handle it higher up
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error during Account initialization: {ex.Message}");
                throw;
            }
        }

        public override string ToString()
        {
            try
            {
                return $"AccountNumber: {accountNumber}, Account Type: {accountType}, Account Balance: {accountBalance}\n{customer}";
            }
            catch (Exception ex)
            {
                return $"Error displaying account info: {ex.Message}";
            }
        }
    }
}
