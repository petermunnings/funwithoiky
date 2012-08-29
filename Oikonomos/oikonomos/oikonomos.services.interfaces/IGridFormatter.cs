using System.Collections.Generic;
using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.common.DTOs;

namespace oikonomos.services.interfaces
{
    public interface IGridFormatter
    {
        JqGridData FormatCommentsForGrid(IEnumerable<CommentDto> comments, JqGridRequest request);
    }
}