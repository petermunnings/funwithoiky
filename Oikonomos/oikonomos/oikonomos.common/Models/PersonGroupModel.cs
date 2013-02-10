namespace oikonomos.common.Models
{
    public class PersonGroupModel
    {
        public int PersonId { get; set; }
        public int GroupId { get; set; }
        public string Name { get; set; }
        public string Type { get; set; }
        public string LastAttended { get; set; }
        public string Leader { get; set; }
        public string Administrator { get; set; }
        public bool PrimaryGroup { get; set; }
    }
}
