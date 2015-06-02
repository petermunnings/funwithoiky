using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.data;

namespace oikonomos.services.interfaces
{
    public interface IChildReportsService
    {
        JqGridData FetchListOfChildren(Person currentPerson, JqGridRequest request, string[] selectedRoles);
    }
}
