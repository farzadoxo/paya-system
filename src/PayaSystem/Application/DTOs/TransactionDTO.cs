namespace Application.DTO
{
    public class AddTransactionDTO
    {
        public decimal Price { get; set; }
        public string FromShebaNumber { get; set; }
        public string ToShebaNumber { get; set; }
        public string Note { get; set; }
    }
}