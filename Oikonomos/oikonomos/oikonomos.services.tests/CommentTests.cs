using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Rhino.Mocks;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services.tests
{
    [TestFixture]
    public class CommentTests
    {
        [Test]
        public void CanGetListOfComments()
        {
            var commentRepository = MockRepository.GenerateStub<ICommentRepository>();

            ICommentService commentService = new CommentService(commentRepository);
            const int personId = 1;
            var currentPerson = new Person();
            var listOfComments = new List<CommentDto> {new CommentDto()};
            commentRepository
                .Expect(c => c.GetListOfComments(currentPerson, personId))
                .Return(listOfComments);

            var sut = commentService.GetListOfComments(currentPerson, personId);

            Assert.That(sut.Count(), Is.EqualTo(1));

        }

    }
}