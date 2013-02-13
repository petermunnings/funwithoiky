using System;
using System.Collections.Generic;
using System.Linq;
using Lib.Web.Mvc.JQuery.JqGrid;
using oikonomos.common.Models;
using oikonomos.data;
using oikonomos.repositories.interfaces;
using oikonomos.services.interfaces;

namespace oikonomos.services
{
    public class PersonGroupService : IPersonGroupService
    {
        private readonly IPersonGroupRepository _personGroupRepository;

        public PersonGroupService(IPersonGroupRepository personGroupRepository)
        {
            _personGroupRepository = personGroupRepository;
        }

        public JqGridData FetchGroupsPersonIsInJQGrid(Person currentPerson, int personId, JqGridRequest request)
        {
            var groups = _personGroupRepository.GetPersonGroupViewModels(personId, currentPerson);
            var groupsPersonIsIn = groups.Where(g => g.IsInGroup);

            return CreateGridFromData(request, groupsPersonIsIn);
        }

        public JqGridData FetchGroupsPersonIsNotInJQGrid(Person currentPerson, int personId, JqGridRequest request)
        {
            var groups = _personGroupRepository.GetPersonGroupViewModels(personId, currentPerson);
            var groupsPersonIsNotIn = groups.Where(g => g.IsInGroup==false);

            return CreateGridFromData(request, groupsPersonIsNotIn);
        }

        private static JqGridData CreateGridFromData(JqGridRequest request, IEnumerable<PersonGroupViewModel> groupsPersonIsIn)
        {
            var totalRecords = groupsPersonIsIn.Count();

            switch (request.sidx)
            {
                case "GroupName":
                    {
                        if (request.sord.ToLower() == "asc")
                        {
                            groupsPersonIsIn = groupsPersonIsIn.OrderBy(g => g.GroupName)
                                                               .Skip((request.page - 1)*request.rows)
                                                               .Take(request.rows)
                                                               .ToList();
                        }
                        else
                        {
                            groupsPersonIsIn = groupsPersonIsIn
                                .OrderByDescending(g => g.GroupName)
                                .Skip((request.page - 1)*request.rows)
                                .Take(request.rows)
                                .ToList();
                        }
                        break;
                    }
            }

            var groupsGridData = new JqGridData()
                {
                    total = (int) Math.Ceiling((float) totalRecords/(float) request.rows),
                    page = request.page,
                    records = totalRecords,
                    rows = (from g in groupsPersonIsIn
                            select new JqGridRow()
                                {
                                    id = g.GroupId.ToString(),
                                    cell = new string[]
                                        {
                                            g.GroupId.ToString(),
                                            g.GroupName,
                                            g.IsPrimaryGroup.ToString()
                                        }
                                }).ToArray()
                };
            return groupsGridData;
        }

    }
}