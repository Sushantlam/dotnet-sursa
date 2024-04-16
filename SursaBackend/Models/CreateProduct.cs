namespace SursaBackend.Models
{
    public class CreateProduct
    {
        public Guid Id { get; set; }

        public string Course { get; set; }


        public int Time { get; set; }

        public string Tutor { get; set; }
    }
}
