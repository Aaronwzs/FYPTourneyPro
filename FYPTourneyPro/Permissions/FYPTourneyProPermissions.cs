namespace FYPTourneyPro.Permissions;

public static class FYPTourneyProPermissions
{
    public const string GroupName = "FYPTourneyPro";

    public static class Books
    {
        public const string Default = GroupName + ".Books";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    //Add your own permission names. Example:
    //public const string MyPermission1 = GroupName + ".MyPermission1";

    public static class TodoItems
    {
        public const string Default = GroupName + ".TodoItems";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }


    //Added tour
    public static class Tournaments
    {
        public const string Default = GroupName + ".Tournaments";
        public const string Create = Default + ".Create";
        public const string Edit = Default + ".Edit";
        public const string Delete = Default + ".Delete";
    }

    public static class Categories
    {
        public const string Default = GroupName + ".Categories";
        public const string Create = Default + ".Create";
        
        public const string Delete = Default + ".Delete";
    }

    public static class Draws
    {
        public const string Default = GroupName + ".Draws";
        public const string Generate = Default + ".Generate";
    }
    public static class MatchScores
    {
        public const string Default = GroupName + ".Scores";
        public const string Save = Default + ".Save";
    }

    public static class AppForms
    {
        public const string Default = GroupName + ".AppForms";
        public const string Approve = Default + ".Approve";
        public const string Reject = Default + ".Reject";
    }


}
