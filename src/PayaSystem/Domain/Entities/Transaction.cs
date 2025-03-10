namespace Domain.Entities
{
    public class Transaction
    {
        public int Id { get; set; }
        public decimal Price { get; set; }
        public string FromShebaNumber { get; set; }
        public string ToShebaNumber { get; set; }
        public string Note { get; set; }
        public DateTime CreatedAt { get; set; }
        public string Status { get; set; }
    }
}