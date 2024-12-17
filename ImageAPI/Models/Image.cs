namespace ImageAPI.Models
{
    public class Image
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public string? Path { get; set; } // Behold stien, hvis du også ønsker den
        public DateTime Timestamp { get; set; }
        public byte[]? Data { get; set; } // Binær data for billedet
        public override string ToString()
        {
            return $"Id: {Id}, Name: {Name}, Time: {Timestamp:dd-MM-yyyy HH:mm:ss}";
        }

        //bombehund
    }
}
