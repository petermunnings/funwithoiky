using System;
using System.Collections.Generic;
using Lib.Web.Mvc.JQuery.JqGrid;
using NUnit.Framework;
using oikonomos.common.DTOs;
using oikonomos.services.interfaces;

namespace oikonomos.services.tests
{
    [TestFixture]
    public class GridFormatterTests
    {
        [Test]
        public void CanFormatCommentsCorrectly()
        {
            IGridFormatter gridFormatter = new GridFormatter();

            var request = new JqGridRequest
                              {
                                  sidx = "Date",
                                  rows = 2,
                                  page = 1,
                                  sord = "asc"
                              };

            var comments = new List<CommentDto>
                {new CommentDto {Comment = "Comment1", CommentDate = new DateTime(2012, 1, 1)},
                 new CommentDto {Comment = "Comment2", CommentDate = new DateTime(2012, 2, 1)},
                 new CommentDto {Comment = "Comment3", CommentDate = new DateTime(2012, 3, 1)},
                 new CommentDto {Comment = "Comment4", CommentDate = new DateTime(2012, 4, 1)}
                };
            
            var result = gridFormatter.FormatCommentsForGrid(comments, request);

            Assert.That(result.rows.Length, Is.EqualTo(2));
            Assert.That(result.records, Is.EqualTo(4));
        }

        [Test]
        public void CanSortCommentsCorrectly()
        {
            IGridFormatter gridFormatter = new GridFormatter();

            var request = new JqGridRequest
            {
                sidx = "Comment",
                rows = 2,
                page = 1,
                sord = "desc"
            };

            var comments = new List<CommentDto>
                {new CommentDto {Comment = "Comment1", CommentDate = new DateTime(2012, 1, 1)},
                 new CommentDto {Comment = "Comment2", CommentDate = new DateTime(2012, 2, 1)},
                 new CommentDto {Comment = "Comment3", CommentDate = new DateTime(2012, 3, 1)},
                 new CommentDto {Comment = "Comment4", CommentDate = new DateTime(2012, 4, 1)}
                };

            var result = gridFormatter.FormatCommentsForGrid(comments, request);

            Assert.That(result.rows[0].cell[1], Is.EqualTo("01 Apr 2012"));
        }
         
    }
}