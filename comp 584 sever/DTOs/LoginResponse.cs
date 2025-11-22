namespace comp_584_sever.DTOs
{
    public class LoginResponse
    {
        public bool Success { get; set; }
        public required string message { get; set; }
        public required string token { get; set; }

    }
}
