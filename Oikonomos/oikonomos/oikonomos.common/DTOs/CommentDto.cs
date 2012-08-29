using System;

namespace oikonomos.common.DTOs
{
    public class CommentDto
    {
        public string   CreatedByPerson { get; set; }
        public int      AboutPersonId   { get; set; }
        public int      CommentId       { get; set; }
        public string   Comment         { get; set; }
        public DateTime CommentDate     { get; set; }
        public string   DisplayDate
        {
            get { return CommentDate.ToString("dd MMM yyyy"); }
        }
    }
}
