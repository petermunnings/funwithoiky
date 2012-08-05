using System.Collections.Generic;
using oikonomos.common.DTOs;
using oikonomos.data;

namespace oikonomos.services.interfaces
{
    public interface ICommentService
    {
        IEnumerable<CommentDto> GetListOfComments(Person currentPerson, int personId);
    }
}