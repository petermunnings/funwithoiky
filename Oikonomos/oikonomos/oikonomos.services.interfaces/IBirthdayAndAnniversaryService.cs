using System;
using System.IO;

namespace oikonomos.services.interfaces
{
    public interface IBirthdayAndAnniversaryService
    {
        Stream GetBirthdayListForAMonth(string selectedRoles, int selectedMonth, int churchId);
        Stream GetBirthdayListForAMonth(int selectedMonth, int churchId);
        Stream GetAnniversaryListForAMonth(string selectedRoles, int selectedMonth, int churchId);
        Stream GetAnniversaryListForAMonth(int selectedMonth, int churchId);
        Stream GetBirthdayListForADateRange(DateTime startDate, DateTime endDate, int churchId);
        Stream GetAnniversaryListForForADateRange(DateTime startDate, DateTime endDate, int churchId);
    }
}
