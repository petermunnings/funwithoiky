
namespace oikonomos.common
{
    public enum GroupTypes
    {
        HomeGroup = 1,
        LifeGroup = 2
    }

    public enum OptionalFields
    {
        WorkPhone = 2,
        CellPhone = 3,
        Skype = 4,
        Twitter = 5,
        Facebook = 6,
        Occupation = 7,
        SuburbLookup = 8,
        HomeGroupClassification = 9,
        HeardAbout = 10,
        HomeGroupEvents = 11,
        GroupAdministratorsCanAddMembers = 12,
        ShowWholeChurch = 13,
        Gender = 14
    }

    //public enum SecurityRoles
    //{
    //    ChurchAdministrator = 1,
    //    GroupAdministrator = 2,
    //    Member = 3,
    //    SystemAdministrator = 4,
    //    Contact = 5,
    //    PastMember = 6,
    //    Visitor = 7,
    //    Elder = 8
    //}

    public enum Permissions
    {
        Login,
        ShowOnContactList,
        EditOwnDetails,
        EditOwnGroups,
        EditAllGroups,
        EditGroupPersonalDetails,
        ViewGroupContactDetails,
        EditChurchPersonalDetails,
        ViewChurchContactDetails,
        AddGroup,
        DeleteGroup,
        RemovePersonFromGroup,
        ViewLists,
        ViewAdminReports,
        AddComment,
        AddEvent,
        DeleteEvent,
        EditSettings,
        SendSms,
        ViewGeneralComments,
        ViewPersonalComments,
        AddSite,
        EditSite,
        DeleteSite,
        AddSuburb,
        DeleteSuburb,
        AddGroupClassification,
        DeleteGroupClassification,
        EditBulkSmsDetails,
        EditChurchContactDetails,
        SendEmailAndPassword,
        SetGroupLeaderOrAdministrator,
        ViewPeopleNotInAnyGroup,
        ViewGroupAttendance,
        EmailGroupMembers,
        SmsGroupMembers,
        EmailGroupLeaders,
        SmsGroupLeaders,
        EmailChurch,
        SmsChurch,
        SystemAdministrator
    }

    public enum Relationships
    {
        Unknown = 0,
        Husband = 1,
        Wife = 2,
        Son = 3,
        Daughter = 4,
        Father = 5,
        Mother = 6,
        Brother = 7,
        Sister = 8,
        Grandmother = 9,
        Grandfather = 10,
        Grandson = 11,
        Granddaughter = 12
    }

    public enum Tables
    {
        Person = 1,
        Family = 2,
        Group = 3,
        Site = 4,
        Church = 5
    }

    public enum EventVisibilities
    {
        Elders = 1,
        GroupAdministrators = 2,
        Group = 3,
        Site = 4,
        Church = 5,
        Public = 6
    }

    public enum SmsProviders
    {
        BulkSmsSouthAfrica = 1
    }

    public class EventNames
    {
        public static string AttendedGroup
        {
            get { return "Attended Group"; }
        }

        public static string DidNotAttendGroup
        {
            get { return "Did not attend Group"; }
        }

        public static string LeftTheGroup
        {
            get { return "Left the group"; }
        }

        public static string Comment
        {
            get { return "Comment"; }
        }

    }

    public class CacheNames
    {
        public static string Permissions
        {
            get { return "Permissions"; }
        }

        public static string SecurityRoles
        {
            get { return "SecurityRoles"; }
        }
    }

    public class ExceptionMessage
    {
        public static string SessionTimedOut
        {
            get { return "You've been inactive for too long, you're going to have to log in again"; }
        }

        public static string InvalidCredentials
        {
            get { return "You don't have permission to perform that action"; }
        }
    }

    public class SessionVariable
    {
        public static string EmailAddresses { get { return "EmailAddresses"; } }
        public static string CellPhoneNos { get { return "CellPhoneNos"; } }
        public static string LoggedOnPerson { get { return "LoggedOnPerson"; } }
        public static string Church { get { return "Church"; } }
    }

}
