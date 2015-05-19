using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.data;

namespace oikonomos.services.interfaces
{
    public interface IMessageService
    {
        JqGridData GetMessageStatuses(Person currentPerson, JqGridRequest request);
    }
}