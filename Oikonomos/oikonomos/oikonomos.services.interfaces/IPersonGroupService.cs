using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.data;

namespace oikonomos.services.interfaces
{
    public interface IPersonGroupService
    {
        JqGridData FetchGroupsPersonIsInJQGrid(Person currentPerson, int personId, JqGridRequest request);
        JqGridData FetchGroupsPersonIsNotInJQGrid(Person currentPerson, int personId, JqGridRequest request);
    }
}