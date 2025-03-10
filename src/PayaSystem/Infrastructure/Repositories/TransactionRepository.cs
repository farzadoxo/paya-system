using Application.Interfaces.Repository;
using DomainEntites = Domain.Entities;
using Infrastructure.Data.Contexts;
using Application.DTO;
using DataEntites = Infrastructure.Data.Entities;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;


namespace Infrastructure.Repositories
{

    public class TransactionRepository : ITransactionRepository
    {
        // Inject Dependencies
        private readonly PayaDbContext _db;
        public TransactionRepository(PayaDbContext db)
        {
            _db = db;
        }


        public DomainEntites.Account FindAccount(string shebNumber)
        {
            var item = _db.Accounts.Find(shebNumber);
            if(item != null)
            {
                DomainEntites.Account account = new DomainEntites.Account
            {
                FullName=item.FullName,
                Balance = item.Balance,
                ShebaNumber = item.ShebaNumber
            };
            return account;
            }

            return null;
            
        }


        public DomainEntites.Transaction FindTransaction(int id)
        {
            var item = _db.Transactions.Find(id);
            if(item != null)
            {
                DomainEntites.Transaction transaction = new DomainEntites.Transaction
                {
                    Id = item.Id,
                    Price = item.Price,
                    FromShebaNumber = item.ToShebaNumber,
                    ToShebaNumber = item.ToShebaNumber,
                    Note = item.Note,
                    Status = item.Status,
                    CreatedAt = item.CreatedAt
                };

                return transaction;
            }

            return null;
        }

        public List<DomainEntites.Transaction> GetAll()
        {
            var result = _db.Transactions.Select(x => new DomainEntites.Transaction
            {Id = x.Id,
            Price = x.Price,
            FromShebaNumber = x.FromShebaNumber,

            ToShebaNumber = x.ToShebaNumber,
            Note = x.Note,
            CreatedAt = x.CreatedAt,
            Status = x.Status
            }).OrderByDescending(x => x.CreatedAt).ToList();

            return result;
        }

        public DomainEntites.Transaction Add(Transaction transaction)
        {
            // get accounts from database
            var fromUser = _db.Accounts.Find(transaction.FromShebaNumber);
            var toUser = _db.Accounts.Find(transaction.ToShebaNumber);

            // make a DataEntity to add to database
            DataEntites.Transaction dataTransaction = new DataEntites.Transaction
            {
                Price = transaction.Price,
                FromShebaNumber = transaction.FromShebaNumber,
                ToShebaNumber = transaction.ToShebaNumber,
                Note = transaction.Note,
                Status = "pending",
                CreatedAt = DateTime.Now
            };

            // money
            fromUser.Balance -= transaction.Price;
            // Add a new transaction to database
            _db.Transactions.Add(dataTransaction);
            // save everything in database
            _db.SaveChanges();

            // Get added record
            var result = _db.Transactions.Select(x => new DomainEntites.Transaction{
                Id = x.Id,
                Price = x.Price,
                FromShebaNumber = x.FromShebaNumber,
                ToShebaNumber = x.ToShebaNumber,
                Status = x.Status,
                Note = x.Note,
                CreatedAt = x.CreatedAt
            }).OrderByDescending(x => x.Id).FirstOrDefault();

            return result;
 
        }


        public DomainEntites.Transaction Confirmed(int request_id)
        {

            // Get transaction from database (by id)
            var transaction = _db.Transactions.Find(request_id);
            var fromUser = _db.Accounts.Find(transaction.FromShebaNumber);
            var toUser = _db.Accounts.Find(transaction.ToShebaNumber);

            // update records
            transaction.Status = "confirmed";
            toUser.Balance += transaction.Price;
            _db.SaveChanges();

            Transaction domainTransaction = new Transaction
            {
                Id = transaction.Id,
                Price = transaction.Price,
                FromShebaNumber = transaction.FromShebaNumber,
                ToShebaNumber = transaction.ToShebaNumber,
                Note = transaction.Note,
                Status = "confirmed",
                CreatedAt = transaction.CreatedAt
            };

            return domainTransaction;     
        }
        
        
        public DomainEntites.Transaction Canceled(int request_id)
        {
            // Get transaction from database (by id)
            var transaction = _db.Transactions.Find(request_id);
            var fromUser = _db.Accounts.Find(transaction.FromShebaNumber);
            var toUser = _db.Accounts.Find(transaction.ToShebaNumber);

            // update records
            transaction.Status = "canceled";
            fromUser.Balance += transaction.Price;
            _db.SaveChanges();

            Transaction domainTransaction = new Transaction
            {
                Id = transaction.Id,
                Price = transaction.Price,
                FromShebaNumber = transaction.FromShebaNumber,
                ToShebaNumber = transaction.ToShebaNumber,
                Note = transaction.Note,
                Status = "canceled",
                CreatedAt = transaction.CreatedAt
            };

            return domainTransaction; 
        }
    }
}