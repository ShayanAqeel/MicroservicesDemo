namespace AuthenticationMicroservice.Models
{
    public class AddUserRequest
    {
        public string userName { get; set; }
        public string password { get; set; }
        public string role{ get; set; }
    }
}
