namespace MediumApi.Application.Response
{
    public class PostResponse
    {
        public int Id { get; set; }
        public string Link { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Description { get; set; }
        public string Content { get; set; }
        public string Thumbnail { get; set; }
        public string PubDate { get; set; }
        public string[] Categories { get; set; }
    }
}
