namespace ProductMicroservice.Models
{
    public class AddProductRequest
    {
        public string name { get; set; }
        public string description { get; set; }
        public int price { get; set; }
    }
}
