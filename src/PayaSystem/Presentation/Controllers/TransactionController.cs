using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Mvc;
using Domain.Entities;
using RouteAttribute = Microsoft.AspNetCore.Mvc.RouteAttribute;
using Application.DTO;
using System.Net;
using Application.Interfaces.Services;
using Domain.Commons;

namespace Presentation.Controllers
{
    [Route("/api")]
    [ApiController]

    public class TransactionController : ControllerBase
    {
        public readonly ITransactionService _service;
        public TransactionController(ITransactionService service)
        {
            _service = service;
        }


        [HttpGet("sheba")]
        public JsonResult GetAll()
        {
            var transactions = _service.GetAll();
            return new JsonResult(new {requests = transactions});
        }

        [HttpPost("sheba")]
        public Task<OprationResult> NewTransaction(AddTransactionDTO dto)
        {
            Transaction maped = new Transaction
            {
                Price = dto.Price,
                FromShebaNumber = dto.FromShebaNumber,
                ToShebaNumber = dto.ToShebaNumber,
                Note = dto.Note
            };
            var result = _service.Add(maped);
            return result;
        }

        [HttpPut("sheba/{request_id}")]
        public Task<OprationResult> StatusDetermine(int request_id,StatusDetermineDTO dto)
        {
            if(dto.Status.ToLower() == "confirmed" || dto.Status.ToLower() == "canceled")
            {
                if(dto.Status == "confirmed")
                {
                    var result = _service.Confirmed(request_id);
                    return result;
                }
                else
                {
                    var result = _service.Canceled(request_id);
                    return result;
                }
                
            }
            else
            {
                return ErrorReturn.Invalid_Determining_Status();

            }
        }
    }
}