namespace MediumApi.Domain.Models
{
    public class Category
    {
        public int Id { get; set; }
        public int PostId { get; set; }
        public string Content { get; set; }
    }
}
