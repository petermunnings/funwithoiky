using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.common.DTOs;
using oikonomos.data;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class ChildReportsService : IChildReportsService
    {
        private readonly IChildrenReportsRepository _childrenReportsRepository;
        private readonly IEmailService _emailService;

        public ChildReportsService(IChildrenReportsRepository childrenReportsRepository,
            IEmailService emailService)
        {
            _childrenReportsRepository = childrenReportsRepository;
            _emailService = emailService;
        }

        public JqGridData FetchListOfChildren(Person currentPerson, JqGridRequest request, string[] selectedRoles)
        {
            IEnumerable<ChildReportDto> listOfChildren = new List<ChildReportDto>();
            try
            {
                listOfChildren = _childrenReportsRepository.GetListOfChildrenForAChurch(currentPerson, ConversionService.ConvertSelectedRolesToListOfInts(selectedRoles));
            }
            catch (Exception ex)
            {
                _emailService.SendExceptionEmail(ex);
            }

            var totalRecords = listOfChildren.Count();

            switch (request.sidx)
            {
                case "Age":
                    {
                        listOfChildren = request.sord.ToLower() == "asc" ? listOfChildren.OrderBy(p => p.Age).Skip((request.page - 1) * request.rows).Take(request.rows) : listOfChildren.OrderByDescending(p => p.Age).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                case "Surname":
                    {
                        listOfChildren = request.sord.ToLower() == "asc" ? listOfChildren.OrderBy(p => p.Surname).Skip((request.page - 1) * request.rows).Take(request.rows) : listOfChildren.OrderByDescending(p => p.Surname).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
                case "Firstname":
                    {
                        listOfChildren = request.sord.ToLower() == "asc" ? listOfChildren.OrderBy(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows) : listOfChildren.OrderByDescending(p => p.Firstname).Skip((request.page - 1) * request.rows).Take(request.rows);
                        break;
                    }
            }

            var childrenGridData = new JqGridData()
            {
                total = (int) Math.Ceiling((float) totalRecords/request.rows),
                page = request.page,
                records = totalRecords,
                rows = (from p in listOfChildren.AsEnumerable()
                    select new JqGridRow()
                    {
                        id = p.PersonId.ToString(),
                        cell = new[]
                        {
                            p.PersonId.ToString(),
                            p.Age.ToString(),
                            p.Firstname,
                            p.Surname,
                            p.CellNo,
                            p.GroupName,
                            p.Father,
                            p.Mother

                        }
                    }).ToArray()
            };

            return childrenGridData;
        }
    }
}