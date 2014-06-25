
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
        Gender = 14,
        WindowsLive = 15,
        MaritalStatus = 16,
        ShowOverseeingElder = 17,
        ImageLink = 18
    }

    public enum Permissions
    {
        Login                         = 0,
        AllocateSecurityRole          = 1,
        EditOwnDetails                = 2,
        EditOwnGroups                 = 3,
        EditAllGroups                 = 4,
        EditGroupPersonalDetails      = 5,
        ViewGroupContactDetails       = 6,
        EditChurchPersonalDetails     = 7,
        ViewChurchContactDetails      = 8,
        AddGroups                     = 9,
        DeleteGroups                  = 10,
        RemovePersonFromGroup         = 11,
        ViewLists                     = 12,
        ViewAdminReports              = 13,
        AddComment                    = 14,
        AddEvent                      = 15,
        DeleteEvent                   = 16,
        EditSettings                  = 17,
        ViewComments                  = 19,
        ViewEvents                    = 20,
        AddSite                       = 21,
        EditSite                      = 22,
        DeleteSite                    = 23,
        AddSuburb                     = 24,
        DeleteSuburb                  = 25,
        AddGroupClassification        = 26,
        DeleteGroupClassification     = 27,
        EditBulkSmsDetails            = 28,
        EditChurchContactDetails      = 29,
        SendEmailAndPassword          = 30,
        SetGroupLeaderOrAdministrator = 31,
        ViewPeopleNotInAnyGroup       = 32,
        ViewGroupAttendance           = 33,
        EmailGroupMembers             = 34,
        SmsGroupMembers               = 35,
        EmailGroupLeaders             = 36,
        SmsGroupLeaders               = 37,
        EmailChurch                   = 38,
        SmsChurch                     = 39,
        SystemAdministrator           = 40,
        NotifyGroupLeaderOfVisit      = 41,
        SendWelcomeLetter             = 42,
        AddNewPerson                  = 43,
        EditGroups                    = 44,
        EditGroupLeader               = 45,
        EditGroupAdministrator        = 46,
        DeletePerson                  = 47,
        IncludeInGroupAttendanceStats = 48,
        EditPermissions               = 49,
        IncludeInChurchList           = 50,
        IncludeInNotInGroupList       = 51,
        AllocateToGroup               = 52,
        SendVisitorWelcomeLetter      = 53,
        EditEmailTemplates            = 54,
        ViewHelp                      = 55,
        ViewPersonGroups              = 56,
        ViewDiscipleship101           = 57,
        ViewMessages                  = 58,
        IncludeInGroupList            = 59,
        LinkPersonToNewFamily         = 60,
        ShowEvents                    = 61
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

    public enum ChurchStatuses
    {
        Active = 1,
        Pending = 2,
        Trial = 3,
        Cancelled = 4
    }

    public enum EmailTemplates
    {
        WelcomeVisitors = 1,
        WelcomeMembers = 2,
        NotifyGroupLeader = 3
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

        public static string OverseeingElder
        {
            get { return "Overseeing Elder"; }
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

    public class ImageSize
    {
        public static string FullSize {get { return "FullSize"; }}
        public static string Large { get { return "Large"; } }
        public static string Small { get { return "Small"; } }
    }

}
