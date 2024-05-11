namespace AuthenticationMicroservice.Models
{
    public class EditUserRequest
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string role { get; set; }
    }
}
