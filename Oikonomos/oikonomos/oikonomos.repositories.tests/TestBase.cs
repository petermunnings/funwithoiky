using System.Configuration;
using oikonomos.data;

namespace oikonomos.repositories.tests
{
    public abstract class TestBase
    {
        protected readonly oikonomosEntities Context;

        protected TestBase()
        {
            Context = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString);
        }
    }
}