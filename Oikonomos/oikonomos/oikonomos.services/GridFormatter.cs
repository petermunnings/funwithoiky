using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.common.DTOs;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class GridFormatter : IGridFormatter
    {
        public JqGridData FormatCommentsForGrid(IEnumerable<CommentDto> comments, JqGridRequest request)
        {
            var totalRecords = comments.Count();

            switch (request.sidx)
            {
                case "Date":
                    {
                        comments = request.sord.ToLower() == "asc" ? comments.OrderBy(e => e.CommentDate).Skip((request.page - 1) * request.rows).Take(request.rows) : comments.OrderByDescending(e => e.CommentDate).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                case "Comment":
                    {
                        comments = request.sord.ToLower() == "asc" ? comments.OrderBy(e => e.Comment).Skip((request.page - 1) * request.rows).Take(request.rows) : comments.OrderByDescending(e => e.Comment).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                case "CreatedBy":
                    {
                        comments = request.sord.ToLower() == "asc" ? comments.OrderBy(e => e.CreatedByPerson).Skip((request.page - 1) * request.rows).Take(request.rows) : comments.OrderByDescending(e => e.CreatedByPerson).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
            }

            var commentsForGrid = new JqGridData()
            {
                total = (int)Math.Ceiling((float)totalRecords / request.rows),
                page = request.page,
                records = totalRecords,
                rows = (from e in comments
                        select new JqGridRow()
                        {
                            id = e.CommentId.ToString(),
                            cell = new[] {
                                e.CommentId.ToString(),                    
                                e.CommentDate.ToString("dd MMM yyyy"),
                                e.Comment,
                                e.CreatedByPerson
                                }
                        }).ToArray()
            };
            return commentsForGrid;
        }
    }
}