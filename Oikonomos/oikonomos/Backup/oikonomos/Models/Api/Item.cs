namespace oikonomos.web.Models.Api
{
    public class Item
    {
        public Group group { get; set; }
        public string title { get; set; }
        public string subtitle { get; set; }
        public string description { get; set; }
        public string content { get; set; }
        public string backgroundImage { get; set; }
    }
}