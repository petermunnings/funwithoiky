namespace oikonomos.common.Models
{
    public class OptionalFieldViewModel
    {
        public int ChurchOptionalFieldId { get; set; }
        public int OptionalFieldId { get; set; }
        public string Name { get; set; }
        public string Regex { get; set; }
        public bool Display { get; set; }
        public string Checked
        {
            get
            {
                if (Display)
                    return "yes";
                else
                    return "no";
            }
        }
    }
}
