using System.Net;
using Domain.Entities;

namespace Domain.Commons
{
    public class OprationResult
    {
        public string Message { get; set; }
        public HttpStatusCode Code { get; set; }
        public Transaction Request { get; set; }
        private OprationResult(Transaction request,string message,HttpStatusCode code)
        {
            Message = message;
            Code = code;
            Request = request;
        }

        public static OprationResult Success(string message , Transaction request,HttpStatusCode code) => 
            new(message:message,request:request,code:code);
        
        public static OprationResult Failure(string message,HttpStatusCode code) => 
            new(message:message,request:null,code:code);
    }
}
