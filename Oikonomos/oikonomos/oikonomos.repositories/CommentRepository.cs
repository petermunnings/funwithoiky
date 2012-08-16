using System.Collections.Generic;
using System.Linq;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class CommentRepository : ICommentRepository
    {
        private readonly oikonomosEntities _context;

        public CommentRepository(oikonomosEntities context)
        {
            _context = context;
        }

        IEnumerable<CommentDto> ICommentRepository.GetListOfComments(Person currentPerson, int personId)
        {
            var comments = (from c in _context.Comments
                                where c.AboutPersonId == personId
                                select c);
            return (from comment in comments 
                    let canViewRoleIds = (from c in comment.Role.CommentThatCanBeViewed select c.RoleId) 
                    where canViewRoleIds.Contains(currentPerson.RoleId) 
                    select new CommentDto {Comment = comment.Comment1, CommentDate = comment.CommentDate, CommentId = comment.CommentId});
        }
    }
}