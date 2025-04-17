using System;
using System.Collections.Generic;
using HMBank.Exceptions;

namespace HMBank
{
    internal class BankApp
    {
        private static IBankRepository bankRepo = new BankRepositoryImpl();

        public static void Run()
        {
            while (true)
            {
                Console.WriteLine("\n===== HMBank System Menu =====");
                Console.WriteLine("1. Create Account");
                Console.WriteLine("2. Deposit");
                Console.WriteLine("3. Withdraw");
                Console.WriteLine("4. Get Balance");
                Console.WriteLine("5. Transfer");
                Console.WriteLine("6. Get Account Details");
                Console.WriteLine("7. List Accounts");
                Console.WriteLine("8. Get Transactions");
                Console.WriteLine("9. Exit");
                Console.Write("Enter your choice: ");

                string choice = Console.ReadLine();

                try
                {
                    switch (choice)
                    {
                        case "1":
                            CreateAccountMenu();
                            break;
                        case "2":
                            Deposit();
                            break;
                        case "3":
                            Withdraw();
                            break;
                        case "4":
                            GetBalance();
                            break;
                        case "5":
                            Transfer();
                            break;
                        case "6":
                            GetAccountDetails();
                            break;
                        case "7":
                            ListAccounts();
                            break;
                        case "8":
                            GetTransactions();
                            break;
                        case "9":
                            Console.WriteLine("Exiting system. Goodbye!");
                            return;
                        default:
                            Console.WriteLine("Invalid option, try again.");
                            break;
                    }
                }
                catch (FormatException fe)
                {
                    Console.WriteLine("Invalid input format. Please enter numeric values where required.");
                }
                catch (InvalidAccountException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                catch (InsufficientFundException ex)
                {
                    Console.WriteLine("Error: " + ex.Message);
                }
                
                catch (Exception ex)
                {
                    Console.WriteLine("An unexpected error occurred: " + ex.Message);
                }
            }
        }

        private static void CreateAccountMenu()
        {
            Console.WriteLine("\nChoose Account Type: 1. Savings 2. Current 3. ZeroBalance");
            string input = Console.ReadLine();

            Console.Write("First Name: "); string first = Console.ReadLine();
            Console.Write("Last Name: "); string last = Console.ReadLine();
            Console.Write("Email: "); string email = Console.ReadLine();
            Console.Write("Phone: "); string phone = Console.ReadLine();
            Console.Write("Address: "); string address = Console.ReadLine();

            Console.Write("Opening Balance: ");
            decimal balance = decimal.Parse(Console.ReadLine());

            var customer = new Customer
            {
                firstName = first,
                lastName = last,
                emailAdress = email,
                phoneNo = phone,
                address = address
            };

            string accType = input switch
            {
                "1" => "Savings",
                "2" => "Current",
                "3" => "ZeroBalance",
                _ => throw new Exception("Invalid account type selected.")
            };

            bankRepo.CreateAccount(customer, accType, balance);
            Console.WriteLine("Customer created successfully.");
        }

        private static void Deposit()
        {
            try
            {
                Console.Write("Account Number: ");
                if (!long.TryParse(Console.ReadLine(), out long accNo))
                {
                    Console.WriteLine("Invalid input. Please enter a numeric account number.");
                    return;
                }

                //  Immediately check if account exists before asking for amount
                var account = bankRepo.GetAccountDetails(accNo); // This should throw InvalidAccountException if not found

                Console.Write("Amount to Deposit: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    Console.WriteLine("Invalid amount. Please enter a valid number.");
                    return;
                }

                decimal newBalance = bankRepo.Deposit(accNo, amount);
                Console.WriteLine($"New Balance: {newBalance}");
            }
            catch (InvalidAccountException ex)
            {
                Console.WriteLine($"Account Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }


        private static void Withdraw()
        {
            try
            {
                Console.Write("Account Number: ");
                if (!long.TryParse(Console.ReadLine(), out long accNo))
                {
                    Console.WriteLine("Invalid input. Please enter a numeric account number.");
                    return;
                }

                // Immediately check account validity
                var account = bankRepo.GetAccountDetails(accNo); // Will throw if account is invalid

                Console.Write("Amount to Withdraw: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal amount))
                {
                    Console.WriteLine("Invalid amount. Please enter a valid number.");
                    return;
                }

                decimal newBalance = bankRepo.Withdraw(accNo, amount);
                Console.WriteLine($"After withdrawal, balance is: {newBalance}");
            }
            catch (InvalidAccountException ex)
            {
                Console.WriteLine($"Account Error: {ex.Message}");
            }
            catch (InsufficientFundException ex)
            {
                Console.WriteLine($"Insufficient Funds: {ex.Message}");
            }
            catch (OverDraftLimitExceededException ex)
            {
                Console.WriteLine($"Overdraft Error: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error: {ex.Message}");
            }
        }


        private static void GetBalance()
        {
            Console.Write("Account Number: ");
            long accNo = long.Parse(Console.ReadLine());

            decimal balance = bankRepo.GetAccountBalance(accNo);
            Console.WriteLine($"Account Balance: {balance}");
        }

        private static void Transfer()
        {
            Console.Write("From Account: ");
            long from = long.Parse(Console.ReadLine());

            Console.Write("To Account: ");
            long to = long.Parse(Console.ReadLine());

            Console.Write("Amount: ");
            decimal amount = decimal.Parse(Console.ReadLine());

            bankRepo.Transfer(from, to, amount);
            Console.WriteLine("Transfer successful.");
        }

        private static void GetAccountDetails()
        {
            Console.Write("Account Number: ");
            long accNo = long.Parse(Console.ReadLine());

            var account = bankRepo.GetAccountDetails(accNo);
            Console.WriteLine(account);
        }

        private static void ListAccounts()
        {
            try
            {
                var list = bankRepo.ListAccounts();

                if (list == null || list.Count == 0)
                {
                    Console.WriteLine("No accounts found.");
                    return;
                }

                Console.WriteLine("\n--- Account List ---");
                foreach (var acc in list)
                {
                    Console.WriteLine(acc);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving account list: {ex.Message}");
            }
        }


        private static void GetTransactions()
        {
            Console.Write("Start Date (yyyy-mm-dd): ");
            DateTime from = DateTime.Parse(Console.ReadLine());

            Console.Write("End Date (yyyy-mm-dd): ");
            DateTime to = DateTime.Parse(Console.ReadLine());

            var transactions = bankRepo.GetTransactions(from, to);
            foreach (var tx in transactions)
            {
                Console.WriteLine(tx);
            }
        }
    }
}
