using System.Collections.Generic;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class CommentService : ICommentService
    {
        private readonly ICommentRepository _commentRepository;

        public CommentService(ICommentRepository commentRepository)
        {
            _commentRepository = commentRepository;
        }

        public IEnumerable<CommentDto> GetListOfComments(Person currentPerson, int personId)
        {
            return _commentRepository.GetListOfComments(currentPerson, personId);
        }
    }
}