using System.Collections.Generic;
using oikonomos.common.DTOs;
using oikonomos.common.Models;

namespace oikonomos.web.Models.Groups
{
    public class GroupViewModel
    {
        public IEnumerable<EventDto>                 GroupEvents                   { get; set; }
        public bool                                  ShowRoles                     { get; set; }
        public bool                                  ShowList                      { get; set; }
        public int                                   GroupId                       { get; set; }
        public string                                GroupName                     { get; set; }
        public int                                   SelectedGroupClassificationId { get; set; }
        public IList<GroupClassificationViewModel>   GroupClassifications          { get; set; }
        public int                                   SelectedSuburbId              { get; set; }
        public IEnumerable<SuburbViewModel>          Suburbs                       { get; set; }
        public string                                DisplaySuburbLookup           { get; set; }
        public string                                DisplayGroupClassification    { get; set; }
        public IEnumerable<StandardCommentViewModel> StandardComments              { get; set; }
        public int                                   RoleId                        { get; set; }
        public IEnumerable<RoleViewModel>            SecurityRoles                 { get; set; }
        public string                                DisplayOverseeingElder        { get; set; }
    }
}