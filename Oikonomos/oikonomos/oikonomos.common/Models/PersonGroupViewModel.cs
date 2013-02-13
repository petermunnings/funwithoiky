namespace oikonomos.common.Models
{
    public class PersonGroupViewModel
    {
        public int GroupId { get; set; }
        public string GroupName { get; set; }
        public bool IsInGroup { get; set; }
        public bool IsPrimaryGroup { get; set; }
    }
}