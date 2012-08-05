using System.Collections.Generic;
using oikonomos.common.DTOs;
using oikonomos.data;

namespace oikonomos.repositories.interfaces
{
    public interface ICommentRepository
    {
        IEnumerable<CommentDto> GetListOfComments(Person currentPerson, int personId);
    }
}