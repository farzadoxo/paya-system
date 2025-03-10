using System.Net;

namespace Domain.Commons
{
    public class ErrorReturn
    {
        public async static Task<OprationResult> Invalid_Determining_Status()
        {
            return OprationResult.Failure("Only 2 modes are acceptable to determine the status: Confirmed/canceled.",HttpStatusCode.BadRequest);
        }
    }
}
