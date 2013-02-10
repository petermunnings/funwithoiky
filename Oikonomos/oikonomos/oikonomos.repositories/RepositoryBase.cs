using oikonomos.data;

namespace oikonomos.repositories
{
    public abstract class RepositoryBase
    {
        protected readonly oikonomosEntities Context;

        protected RepositoryBase()
        {
            Context = CurrentContext.Instance;
        }
    }
}