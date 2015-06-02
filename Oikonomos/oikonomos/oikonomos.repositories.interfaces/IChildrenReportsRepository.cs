using System.Collections.Generic;
using oikonomos.common.DTOs;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface IChildrenReportsRepository
    {
        IEnumerable<ChildReportDto> GetListOfChildrenForAChurch(Person personRunningReport, IEnumerable<int> roles);
    }
}
