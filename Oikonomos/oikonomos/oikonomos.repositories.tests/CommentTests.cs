using System;
using System.Linq;
using NUnit.Framework;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;

namespace oikonomos.repositories.tests
{
    [TestFixture]
    public class CommentTests : TestBase
    {
        [Test]
        public void ShouldGetTheRightNumberOfComments()
        {
            const int personId   = 1;
            const int roleId     = 4;
            var commentsToDelete = Context.Comments.Where(c => c.AboutPersonId == personId).ToList();
            foreach(var commentToDelete in commentsToDelete)
                Context.DeleteObject(commentToDelete);

            ICommentRepository commentsRepo = new CommentRepository(Context);
            var currentPerson = new Person {PersonId = personId, RoleId = roleId};

            const string testComment = "Test Comment";

            var newComment = new CommentDto
            {
                Comment       = testComment,
                AboutPersonId = personId,
                CommentDate   = DateTime.Now
            };

            var commentId = commentsRepo.SaveItem(currentPerson, newComment);

            var sut = commentsRepo.GetListOfComments(currentPerson, personId);

            Assert.That(sut.Count(), Is.EqualTo(1));
        }

        [Test]
        public void ShouldGetTheRightPersonWhoMadeTheComment()
        {
            
            const int personId = 1;
            const int roleId = 1;
            var commentsToDelete = Context.Comments.Where(c => c.AboutPersonId == personId).ToList();
            foreach (var commentToDelete in commentsToDelete)
                Context.DeleteObject(commentToDelete);

            ICommentRepository commentsRepo = new CommentRepository(Context);
            var currentPerson = new Person { PersonId = personId, RoleId = roleId };

            const string testComment = "Test Comment";

            var newComment = new CommentDto
            {
                Comment       = testComment,
                AboutPersonId = personId,
                CommentDate   = DateTime.Now
            };

            var commentId = commentsRepo.SaveItem(currentPerson, newComment);
            var sut       = commentsRepo.GetListOfComments(currentPerson, personId).ToList();

            Assert.That(sut[0].CreatedByPerson, Is.EqualTo("Peter Munnings"));
        }
         
    }
}