namespace oikonomos.common.Models
{
    public class ChurchViewModel
    {
        public int ChurchId { get; set; }
        public string ChurchName { get; set; }
        public string SiteHeader { get; set; }
        public string SiteDescription { get; set; }
        public string BackgroundImage { get; set; }
        public string UITheme { get; set; }
        public string GoogleSearchRegion { get; set; }
        public bool ShowFacebookLogin { get; set; }
    }
}
