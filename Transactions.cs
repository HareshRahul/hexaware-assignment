using System;

namespace HMBank
{
    public class Transactions
    {
        public Account account { get; set; }
        public string description { get; set; }
        public DateTime dateTime { get; set; }
        public string transactionType { get; set; }
        public decimal transactionAmount { get; set; }

        public Transactions()
        {
            // Default constructor
        }

        public Transactions(Account account, string description, DateTime dateTime, string transactionType, decimal transactionAmount)
        {
            try
            {
                this.account = account ?? throw new ArgumentNullException(nameof(account), "Account cannot be null.");
                this.description = string.IsNullOrWhiteSpace(description) ? "No description" : description;
                this.dateTime = dateTime;

                if (string.IsNullOrWhiteSpace(transactionType))
                    throw new ArgumentException("Transaction type cannot be empty.");

                if (transactionAmount <= 0)
                    throw new ArgumentException("Transaction amount must be greater than zero.");

                this.transactionType = transactionType;
                this.transactionAmount = transactionAmount;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Validation Error: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected error in Transactions constructor: {ex.Message}");
                throw;
            }
        }

        public override string ToString()
        {
            try
            {
                return $"Account: {account}, Description: {description}, Date and Time: {dateTime}, " +
                       $"Transaction Type: {transactionType}, Transaction Amount: {transactionAmount}";
            }
            catch (Exception ex)
            {
                return $"Error displaying transaction info: {ex.Message}";
            }
        }
    }
}
