namespace YTScrapper.Domain.Models
{
    public class YouTubeSearchItem
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Author { get; set; }
        public string Duration { get; set; }
    }
}
