namespace oikonomos.repositories.interfaces
{
    public interface IChurchEmailTemplatesRepository
    {
        string FetchChurchEmailSignature(int churchId);
    }
}
