using System;
using System.Text.RegularExpressions;

namespace HMBank
{
    public class Customer
    {
        public int counter { get; set; }
        public int customerID { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string emailAdress { get; set; }
        public string phoneNo { get; set; }
        public string address { get; set; }

        public Customer()
        {
            // Default constructor
        }

        public Customer(int customerID, string FirstName, string LastName, string EmailAddress, string PhoneNo, string Address)
        {
            try
            {
                // Email validation
                if (!Regex.IsMatch(EmailAddress, @"^[^@\s]+@[^@\s]+\.[^@\s]+$"))
                {
                    throw new ArgumentException("Invalid email format.");
                }

                // Phone number validation
                if (!Regex.IsMatch(PhoneNo, @"^\d{10}$"))
                {
                    throw new ArgumentException("Invalid phone number format. Must be exactly 10 digits.");
                }

                this.customerID = customerID;
                this.firstName = FirstName;
                this.lastName = LastName;
                this.emailAdress = EmailAddress;
                this.phoneNo = PhoneNo;
                this.address = Address;
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine($"Validation Error: {ex.Message}");
                throw; // Optional: throw to propagate the error if needed
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unexpected Error in Customer constructor: {ex.Message}");
                throw;
            }
        }

        public override string ToString()
        {
            try
            {
                return $"Customer ID: {customerID}, First Name: {firstName}, Last Name: {lastName}, " +
                       $"Email Address: {emailAdress}, Phone Number: {phoneNo}, Address: {address}";
            }
            catch (Exception ex)
            {
                return $"Error displaying customer info: {ex.Message}";
            }
        }
    }
}
