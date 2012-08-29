using System.Collections.Generic;
using System.Linq;
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

        public IEnumerable<CommentDto> GetListOfComments(Person currentPerson, int personId, int maxNoCommentsToReturn)
        {
            return GetListOfComments(currentPerson, personId).OrderByDescending(c=>c.CommentDate).Take(maxNoCommentsToReturn);
        }

        public int SaveComment(Person currentPerson, CommentDto newCommentDto)
        {
            return _commentRepository.SaveItem(currentPerson, newCommentDto);
        }

        public void SaveComments(Person currentPerson, IEnumerable<CommentDto> newComments)
        {
            foreach (var comment in newComments)
                _commentRepository.SaveItem(currentPerson, comment);
        }
    }
}