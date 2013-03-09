using System.Collections.Generic;
using oikonomos.data;

namespace oikonomos.services.interfaces
{
    public interface ISmsSender
    {
        string SendSmses(string smsText, IEnumerable<string> cellPhoneNos, string username, string password, Person currentPerson);
    }
}