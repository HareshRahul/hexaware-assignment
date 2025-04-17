using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq.Expressions;
using System.Transactions;
using System.Data;
using HMBank.Exceptions;
using System.Text;
namespace HMBank
{
    internal class BankRepositoryImpl : IBankRepository
    {
        SqlConnection con = null;
        SqlCommand command = null;
        public void CreateAccount(Customer customer, string accType, decimal balance)
        {
            int rowsAffected = 0;
            int rowsAffected1 = 0;
            int customerId=0;
            long accountId=0;
            string query= $"insert into customers(customer_id, first_name, last_name,email, phone_number, address)values (@id, @first, @last, @email, @phone, @address)";
            string query1= $"insert into accounts(account_id, customer_id, account_type, balance)values (@accId, @custId, @type, @balance)";
            string query2 = $"select top 1 customer_id FROM customers order by customer_id desc";
            string query3 = $"select top 1 account_id FROM accounts order by account_id desc";

            try
            {
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query2, con);

                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {

                        customerId = (int)reader["customer_id"];
                    }
                    command = new SqlCommand(query3, con);
                    SqlDataReader reader1= command.ExecuteReader();

                    if (reader1.Read())
                    {

                        accountId = (int)reader1["account_id"];
                    }
                }
                customerId = ++customerId;
                Account account1 = new Account
                {
                    accountNumber =++accountId,
                    customer=customer,
                    accountType = accType,
                    accountBalance = balance
                };

                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    command.Parameters.Add(new SqlParameter("@id", customerId));
                    command.Parameters.Add(new SqlParameter("@first", customer.firstName));
                    command.Parameters.Add(new SqlParameter("@last", customer.lastName));
                    command.Parameters.Add(new SqlParameter("@email", customer.emailAdress));
                    command.Parameters.Add(new SqlParameter("@phone", customer.phoneNo));
                    command.Parameters.Add(new SqlParameter("@address", customer.address));
                    rowsAffected = command.ExecuteNonQuery();
                }
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query1, con);
                    command.Parameters.Add(new SqlParameter("@accId",account1.accountNumber));
                    command.Parameters.Add(new SqlParameter("@custId", customerId));
                    command.Parameters.Add(new SqlParameter("@type", account1.accountType));
                    command.Parameters.Add(new SqlParameter("@balance", account1.accountBalance));
                    rowsAffected1 = command.ExecuteNonQuery();
                }
                
                if(rowsAffected<=0 )
                {
                    throw new ProductException("Customer not added");
                }
                else if (rowsAffected1 <= 0)
                {
                    throw new ProductException("Account not added");
                }


            }
            catch(ProductException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            Console.WriteLine("Account added successfully");

            }


        
            
       

        public decimal GetAccountBalance(long accountNumber)
        {
            
            decimal balance = 0;
            SqlConnection con = null;
            SqlCommand command = null;
            string query = $"select* from accounts where account_id=@id";
            try
            {
                Account account = GetAccountDetails(accountNumber);
                if (account == null)
                {
                    throw new ProductException($"Account {accountNumber} not found ");
                }
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@id", accountNumber);
                    SqlDataReader reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        
                        balance = (decimal)reader["balance"];
                    }

                }
                
            }
            catch(ProductException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
           
            return balance;
        }

        public decimal Deposit(long accountNumber, decimal amount)
        {
            int rowsAffected = 0;
            decimal newamt = 0;



            try
            {
                Account account = GetAccountDetails(accountNumber);
                if (account == null)
                {
                    throw new ProductException($"Account {accountNumber} not found ");
                }
                if(amount<=0)
                {
                    throw new InsufficientFundException($"Invalid deposit amount");
                }
                newamt = (account.accountBalance) + amount;
                

                    string query = "update accounts set balance=@newamt where  account_id=@aid";

                    using (con = DBUtility.GetConnection())
                    {
                        command = con.CreateCommand();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@newamt", newamt);
                        command.Parameters.AddWithValue("aid", accountNumber);
                        rowsAffected = command.ExecuteNonQuery();
                    }
            }
            catch(InsufficientFundException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (ProductException ex)
            {
                Console.WriteLine( ex.Message);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            
                return newamt;
            
        }

        public decimal Withdraw(long accountNumber, decimal amount)
        {
            int rowsAffected = 0;


            decimal newamt = 0;
                
                try
                {
                    Account account = GetAccountDetails(accountNumber);
                if (account == null)
                {
                    throw new ProductException($"Account {accountNumber} not found ");
                }
                if (account.accountType == "Savings" && account.accountBalance - amount < 1000)
                    {
                        throw new Exception("Minimum balance violation");
                    }
                    else if (account.accountType == "Current" && account.accountBalance - amount < -10000)
                    {
                        throw new Exception("Overdraft limit exceeded");
                    }
                    else if (account.accountType == "Zero Balance" && account.accountBalance - amount < 0)
                    {
                        throw new Exception("insufficient balance");
                    }
                    
                newamt = (account.accountBalance) - amount;
                string query = "update accounts set balance=@newamt where  account_id=@aid";
                using (con = DBUtility.GetConnection())
                    {
                        command = con.CreateCommand();
                        command.CommandText = query;
                        command.Parameters.AddWithValue("@newamt", newamt);
                        command.Parameters.AddWithValue("aid", accountNumber);
                        rowsAffected = command.ExecuteNonQuery();
                }
                }
            catch (ProductException ex)
            {
                Console.WriteLine(ex.Message);
            }
            catch (SqlException e)
                {
                    Console.WriteLine(e.ToString());
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.ToString());
                }

            return newamt;

        }

        public void Transfer(long from, long to, decimal amount)
        {
            if (from < 0 || to < 0)
            {
                Withdraw(from, amount);
                Deposit(to, amount);
            }
        }

        public int CalculateInterest()
        {
            int rowsAffected = 0;
            
            //decimal newBalance = 0;
            SqlConnection con = null;
            SqlCommand command = null;
            string query = $"update accounts SET balance =@newBalance where account_id=@accountNumber";
            string query1 = $"select account_id,balance from accounts where account_type='Savings'";
            try
            {
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query1, con);
                    SqlDataReader reader = command.ExecuteReader();
                    List<(long accountNumber, decimal newBalance)> updates = new List<(long, decimal)>();
                    while (reader.Read())
                    {
                        long accountNumber = (int)reader["account_id"];
                        decimal balance = (decimal)reader["balance"];
                        decimal interest = (balance * 4m)/100m;
                        decimal newBalance = balance + interest;
                        updates.Add((accountNumber, newBalance));
                    }
                    foreach(var(accountNumber,newBalance) in updates)
                    {
                        using (con = DBUtility.GetConnection())
                        {
                            command = new SqlCommand(query, con);
                            command.Parameters.AddWithValue("@newBalance", newBalance);
                            command.Parameters.AddWithValue("@accountNumber", accountNumber);
                            rowsAffected=command.ExecuteNonQuery();
                           
                        }
                    }
                    

                }
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return rowsAffected;
                
        }

        public Account GetAccountDetails(long accountNumber)
        {
            Account account = null;
            SqlConnection con = null;
            SqlCommand command= null;
            string query = $"select* from accounts where account_id=@id";
            try
            {
                
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@id", accountNumber);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        account = new Account
                        {
                            accountNumber = (int)reader["account_id"],
                            accountType = (string)reader["account_type"],
                            accountBalance = (decimal)reader["balance"]
                        };
                        
                    }
                    if (account == null)
                    {
                        throw new ProductException($"Account {accountNumber} not found ");
                    }

                }
               
            }
            catch(ProductException e)
            {
                Console.WriteLine(e.Message);
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return account;
            
        }

       public  List<Transactions> GetTransactions(DateTime from, DateTime to)
        {
           List<Transactions> transactions = new List<Transactions>();
            Transactions transaction = null;
            
            SqlConnection con= null;
            SqlCommand command= null;
            string query=$"SELECT * FROM transactions WHERE transaction_date BETWEEN @from AND @to";
            try
            {
                using(con= DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    command.Parameters.AddWithValue("@from", from);
                    command.Parameters.AddWithValue("@to", to);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        transaction = new Transactions
                        {
                            transactionType = reader["transaction_type"].ToString(),
                            dateTime = (DateTime)reader["transaction_date"],
                            transactionAmount = (decimal)reader["amount"],

                           
                            account = new Account
                            {
                                accountNumber = Convert.ToInt64(reader["account_id"]),
                                //accountType = (string)reader["account_type"],
                                //accountBalance = (decimal)reader["balance"]
                            }
                        };
                        transactions.Add(transaction);
                    }
                }
                
            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }

            return transactions;

        }

        public List<Account> ListAccounts()
        {
            List<Account> accounts = new List<Account>();
            Account account = null;
            SqlConnection con = null;
            SqlCommand command = null;
            string query = $"select * from accounts";
            try
            {
                using (con = DBUtility.GetConnection())
                {
                    command = new SqlCommand(query, con);
                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        account = new Account
                        {
                            accountNumber = (int)reader["account_id"],
                            accountType = (string)reader["account_type"],
                            accountBalance = (decimal)reader["balance"]
                        };

                        accounts.Add(account);
                    }


                }

            }
            catch (SqlException e)
            {
                Console.WriteLine(e.ToString());
            }
            catch (Exception e)
            {
                Console.WriteLine(e.ToString());
            }
            return accounts;
        }





    }
}
