using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories
{
    public class CommentRepository : RepositoryBase, ICommentRepository
    {
        public CommentRepository()
        {
            Mapper.CreateMap<Comment, CommentDto>()
                .ForMember(dest=>dest.Comment, opt=>opt.MapFrom(src=>src.Comment1))
                .ForMember(dest => dest.CreatedByPerson, opt=>opt.MapFrom(src=>src.Person1.Fullname));
            Mapper.CreateMap<CommentDto, Comment>()
                .ForMember(dest => dest.Comment1, opt => opt.MapFrom(src => src.Comment));
        }

        IEnumerable<CommentDto> ICommentRepository.GetListOfComments(Person currentPerson, int personId)
        {
            var comments = (from c in Context.Comments
                            from r in c.Role.CanBeReadByRoles
                            where c.AboutPersonId == personId
                            && r.RoleId == currentPerson.RoleId
                            select c);
            return Mapper.Map<IEnumerable<Comment>, IEnumerable<CommentDto>>(comments);
        }

        public int SaveItem(Person currentPerson, CommentDto newCommentDto)
        {
            var newComment            = new Comment();
            Mapper.Map(newCommentDto, newComment);
            newComment.MadeByPersonId = currentPerson.PersonId;
            newComment.MadeByRoleId   = currentPerson.RoleId;
            Context.AddToComments(newComment);
            Context.SaveChanges();
            return newComment.CommentId;
        }
    }
}