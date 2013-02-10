using System.Configuration;
using oikonomos.data;

namespace oikonomos.repositories
{
    public class CurrentContext
    {
        private static oikonomosEntities instance;

        private CurrentContext(){}

        public static oikonomosEntities Instance
        {
            get
            {
                return instance ?? (instance = new oikonomosEntities(ConfigurationManager.ConnectionStrings["oikonomosEntities"].ConnectionString));
            }
        }
    }
}
