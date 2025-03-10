using Domain.Entities;
using Domain.Commons;

namespace Application.Interfaces.Services
{
    public interface ITransactionService
    {
        List<Transaction> GetAll();
        Task<OprationResult> Add(Transaction transaction);
        Task<OprationResult> Confirmed(int request_id);
        Task<OprationResult> Canceled(int request_id); 
    }
}