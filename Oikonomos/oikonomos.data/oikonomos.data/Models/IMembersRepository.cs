using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace oikonomos.data.Models
{
    public interface IMembersRepository
    {
        int GetMembersCount(oikonomosEntities context);
        IQueryable<Person> GetMembers(oikonomosEntities context, string sortExpression, string sortDirection, int pageIndex, int pageSize);
    }
}