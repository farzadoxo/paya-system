using Application.Interfaces.Repository;
using Application.Interfaces.Services;
using DomainEntities = Domain.Entities;
using DataEntites = Infrastructure.Data.Entities;
using Infrastructure.Data.Contexts;
using Domain.Entities;
using System.Net;
using Domain.Commons;

namespace Infrastructure.Services
{
    public class TransactionService : ITransactionService
    {
        private readonly ITransactionRepository _repo;
        public TransactionService(ITransactionRepository repository)
        {
            _repo = repository;
        }

        public List<Transaction> GetAll()
        {
            var transactions = _repo.GetAll();
            return transactions;
        }


        public async Task<OprationResult> Add(Transaction transaction)
        {
            if(transaction.FromShebaNumber.Length == 24 && transaction.ToShebaNumber.Length == 24)
            {
                if(transaction.FromShebaNumber.ToUpper().Contains("IR") && transaction.ToShebaNumber.ToUpper().Contains("IR"))
                {
                    if(transaction.ToShebaNumber != transaction.FromShebaNumber)
                    {
                        var fromUser = _repo.FindAccount(transaction.FromShebaNumber);
                        var toUser = _repo.FindAccount(transaction.ToShebaNumber);

                        if(fromUser != null && toUser != null)
                        {
                            if(transaction.Price < fromUser.Balance)
                            {
                                // add record to database
                                var result = _repo.Add(transaction);

                                Transaction domainTransaction = new Transaction
                                {
                                    Id = result.Id,
                                    Price = result.Price,
                                    FromShebaNumber = result.FromShebaNumber,
                                    ToShebaNumber = result.ToShebaNumber,
                                    Note = result.Note,
                                    Status = result.Status,
                                    CreatedAt = result.CreatedAt
                                };

                                return OprationResult.Success("Request is saved successfully and is in pending status",domainTransaction,HttpStatusCode.Created);
                                
                            }
                            else
                            {
                                return OprationResult.Failure("The amount of the desired amount is less than the balance of the origin Sheba number!",HttpStatusCode.BadRequest);
                            }
                        }
                        else
                        {
                            return OprationResult.Failure("Owner of this sheba numbers not reached!",HttpStatusCode.NotFound);
                        }
                        }
                    else
                    {
                          return OprationResult.Failure("The Sheba number of origin and destination cannot be the same!",HttpStatusCode.BadRequest);
                    }
                }
                else
                {
                    return OprationResult.Failure("Sheba number most be start with 'IR' !",HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return OprationResult.Failure("Length of Sheba number most be 24 character!",HttpStatusCode.BadRequest);
            }
        }

        public async Task<OprationResult> Confirmed(int request_id)
        {
            var transaction = _repo.FindTransaction(request_id);
            if(transaction != null)
            {
                if(transaction.Status == "pending")
                {
                    var fromUser = _repo.FindAccount(transaction.FromShebaNumber);
                    var toUser = _repo.FindAccount(transaction.ToShebaNumber);
                    
                    if(fromUser != null && toUser != null)
                    {
                        var result = _repo.Confirmed(request_id);
                        return OprationResult.Success("Request is Confirmed!",result,HttpStatusCode.OK);
                    }
                    else
                    {
                        return OprationResult.Failure("Owner of this sheba numbers not reached!",HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    return OprationResult.Failure("This request has already been determined!",HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return OprationResult.Failure("No transaction find with this 'request_id' !",HttpStatusCode.NotFound);
            }
        }


        public async Task<OprationResult> Canceled(int request_id)
        {
            var transaction = _repo.FindTransaction(request_id);
            if(transaction != null)
            {
                if(transaction.Status == "pending")
                {
                    var fromUser = _repo.FindAccount(transaction.FromShebaNumber);
                    var toUser = _repo.FindAccount(transaction.ToShebaNumber);
                    
                    if(fromUser != null && toUser != null)
                    {
                        var result = _repo.Canceled(request_id);
                        return OprationResult.Success("Request is Canceled!",result,HttpStatusCode.OK);
                    }
                    else
                    {
                        return OprationResult.Failure("Owner of this sheba numbers not reached!",HttpStatusCode.NotFound);
                    }
                }
                else
                {
                    return OprationResult.Failure("This request has already been determined!",HttpStatusCode.BadRequest);
                }
            }
            else
            {
                return OprationResult.Failure("No transaction find with this 'request_id' !",HttpStatusCode.NotFound);
            }
        }
    }
}