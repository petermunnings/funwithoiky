namespace oikonomos.repositories.interfaces.Messages
{
    public interface IMessageAttachmentRepository
    {
        void SaveMessageAttachment(int messageId, string name, string type, int length, string attachmentContentType, byte[] attachmentContent);
    }
}
