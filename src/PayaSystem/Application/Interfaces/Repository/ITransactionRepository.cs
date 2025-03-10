using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using Application.DTO;
using System.ComponentModel.DataAnnotations;

namespace Application.Interfaces.Repository
{
    public interface ITransactionRepository
    {
        Account FindAccount(string shebaNumber);
        Transaction FindTransaction(int id);
        List<Transaction> GetAll();
        Transaction Add(Transaction transactionInfo);
        Transaction Confirmed(int transactionId);
        Transaction Canceled(int transactionId);
    }
}
