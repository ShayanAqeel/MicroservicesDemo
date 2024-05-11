namespace ProductMicroservice.Models
{
    public class AddProductRequest
    {
        //Attributes for api request
        public string name { get; set; }
        public string description { get; set; }
        public int price { get; set; }
    }
}
