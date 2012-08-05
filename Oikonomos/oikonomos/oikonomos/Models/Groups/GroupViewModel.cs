using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using oikonomos.common.Models;

namespace oikonomos.web.Models.Groups
{
    public class GroupViewModel
    {
        public bool                               ShowRoles                     { get; set; }
        public bool                               ShowList                      { get; set; }
        public int                                GroupId                       { get; set; }
        public string                             GroupName                     { get; set; }
        public int                                SelectedGroupClassificationId { get; set; }
        public List<GroupClassificationViewModel> GroupClassifications          { get; set; }
        public int                                SelectedSuburbId              { get; set; }
        public List<SuburbViewModel>              Suburbs                       { get; set; }
        public string                             DisplaySuburbLookup           { get; set; }
        public string                             DisplayGroupClassification    { get; set; }
        public int                                SelectedStandardCommentId     { get; set; }
        public List<StandardCommentViewModel>     StandardComments              { get; set; }
        public int                                RoleId                        { get; set; }
        public List<RoleViewModel>                SecurityRoles                 { get; set; }
    }
}