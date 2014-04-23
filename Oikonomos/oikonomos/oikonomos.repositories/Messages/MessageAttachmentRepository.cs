using oikonomos.data;
using oikonomos.repositories.interfaces.Messages;

namespace oikonomos.repositories.Messages
{
    public class MessageAttachmentRepository : RepositoryBase, IMessageAttachmentRepository
    {
        public void SaveMessageAttachment(int messageId, string name, string type, int length, string attachmentContentType, byte[] attachmentContent)
        {
            var messageAttachment = new MessageAttachment
                {
                    MessageId = messageId,
                    FileName = name,
                    FileContent = attachmentContent,
                    FileLength = length,
                    FileType = type
                };

            Context.AddToMessageAttachments(messageAttachment);
            Context.SaveChanges();
        }
    }
}