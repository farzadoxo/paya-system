namespace Application.DTO
{
    public class StatusDetermineDTO
    {
        public StatusDetermineDTO(string status,string note)
        {
            Status = status;
            Note = note;
        }
        public string Status { get; set; }
        public string Note { get; set; }
    }
}