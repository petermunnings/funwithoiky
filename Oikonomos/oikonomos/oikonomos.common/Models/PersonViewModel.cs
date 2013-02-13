using System;
using System.Collections.Generic;

namespace oikonomos.common.Models
{
    public class PersonViewModel
    {
        public string WindowsLiveId { get; set; }
        public int PersonId { get; set; }
        public int FamilyId { get; set; }
        public string Firstname { get; set; }
        public string Surname { get; set; }
        public string FullName
        {
            get
            {
                return Firstname + " " + Surname;
            }
        }
        public string Email { get; set; }
        public DateTime? DateOfBirth_Value { get; set; }
        public string DateOfBirth
        {
            get { return DateOfBirth_Value.HasValue ? DateOfBirth_Value.Value.ToString("dd MMMM yyyy") : string.Empty; }
        }

        public string DateOfBirth_Short
        {
            get { return DateOfBirth_Value.HasValue ? DateOfBirth_Value.Value.ToString("dd MMMM") : string.Empty; }
        }

        public DateTime? Anniversary_Value { get; set; }
        public string Anniversary
        {
            get { return Anniversary_Value.HasValue ? Anniversary_Value.Value.ToString("dd MMMM yyyy") : string.Empty; }
        }

        public string Anniversary_Short
        {
            get { return Anniversary_Value.HasValue ? Anniversary_Value.Value.ToString("dd MMMM") : string.Empty; }
        }
        public string HomePhone { get; set; }
        public string CellPhone { get; set; }
        public string WorkPhone { get; set; }
        public string Skype { get; set; }
        public string Twitter { get; set; }
        public string Occupation { get; set; }
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        public string AddressType { get; set; }
        public decimal Lat { get; set; }
        public decimal Lng { get; set; }
        public string LeaderChecked { get; set; }
        public string AdministratorChecked { get; set; }
        public bool HasUsername { get; set; }
        public bool FindFamily { get; set; }
        public int GroupId { get; set; }  //This is for saving it into the right group
        public bool IsInMultipleGroups { get; set; }
        public string GroupName { get; set; }
        public string Site { get; set; }
        public string HeardAbout { get; set; }
        public string FacebookId { get; set; }
        public string Gender { get; set; }
        public int RoleId { get; set; }
        public string RoleName { get; set; }

        public IEnumerable<RoleViewModel> SecurityRoles { get; set; }

        public IEnumerable<FamilyMemberViewModel> FamilyMembers { get; set; }

        public IEnumerable<PersonGroupViewModel> PersonGroups { get; set; }
    }

    public class FamilyMemberViewModel
    {
        public int PersonId { get; set; }
        public int RelationshipId { get; set; }
        public string FamilyMember { get; set; }
        public string Person { get; set; }
        public string Relationship { get; set; }
        public string FacebookId { get; set; }
    }
}