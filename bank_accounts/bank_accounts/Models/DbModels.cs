using System;
using System.Collections.Generic;
namespace bank_accounts.Models
{
    public class User
    {
        public User()
        {
            Transactions = new List<Transaction>();
        }
        public int UserId {get; set;}
        public string FirstName {get; set;}
        public string LastName {get; set;}
        public string Email {get; set;}
        public string Password {get; set;}
        public int Balance {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public List<Transaction> Transactions {get; set;}
    }
    public class Transaction
    {
        public int TransactionId{get; set;}
        public int Amount {get; set;}
        public int UserId {get; set;}
        public DateTime CreatedAt {get; set;}
        public DateTime UpdatedAt {get; set;}
        public User User {get; set;}
    }
}