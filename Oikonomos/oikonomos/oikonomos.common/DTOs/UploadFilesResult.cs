namespace oikonomos.common.DTOs
{
    public class UploadFilesResult
    {
        public string Name { get; set; }
        public int Length { get; set; }
        public string Type { get; set; }
        public byte[] AttachmentContent { get; set; }
        public string AttachmentContentType { get; set; }
    }
}