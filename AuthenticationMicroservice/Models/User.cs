namespace AuthenticationMicroservice.Models
{
    public class User
    {
        public Guid Id { get; set; }
        public string userName { get; set; }
        public string password { get; set; }
        public string role { get; set; }
    }
}
