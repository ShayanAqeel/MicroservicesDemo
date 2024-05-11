namespace ProductMicroservice.Models
{
    public class EditProductRequest
    {
        public string name { get; set; }
        public string description { get; set; }
        public int price { get; set; }
    }
}
