using System.Configuration;
using System.Data.EntityClient;
using oikonomos.data;

namespace oikonomos.repositories.tests
{
    public abstract class TestBase
    {
        protected readonly oikonomosEntities _context;
        protected string _sqlConnectionString;

        protected TestBase()
        {
            var entityConString = ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString;
            _context = new oikonomosEntities(entityConString);
            var entityConnection = (EntityConnection)_context.Connection;
            _sqlConnectionString = entityConnection.StoreConnection.ConnectionString;
        }
    }
}