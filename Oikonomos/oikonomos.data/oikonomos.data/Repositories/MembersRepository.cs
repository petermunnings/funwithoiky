using System;
using System.Linq;
using System.Web;
using oikonomos.data.Models;
using oikonomos.data;
using System.Configuration;

namespace jqGrid.Repositories.NorthWind
{
    public class MembersRepository : IMembersRepository
    {

        #region IOrdersRepository Members
        public int GetMembersCount(oikonomosEntities context)
        {
            return context.People.Count();
        }

        public IQueryable<Person> GetMembers(oikonomosEntities context, string sortExpression, string sortDirection, int pageIndex, int pageSize)
        {
            return context.People.OrderBy("it." + sortExpression + " " + sortDirection).Skip(pageIndex * pageSize).Take(pageSize);
        }

        #endregion
    }
}