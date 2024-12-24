using FYPTourneyPro.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;
using Volo.Abp.MultiTenancy;

namespace FYPTourneyPro.Permissions;

public class FYPTourneyProPermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(FYPTourneyProPermissions.GroupName);

        var booksPermission = myGroup.AddPermission(FYPTourneyProPermissions.Books.Default, L("Permission:Books"));
        booksPermission.AddChild(FYPTourneyProPermissions.Books.Create, L("Permission:Books.Create"));
        booksPermission.AddChild(FYPTourneyProPermissions.Books.Edit, L("Permission:Books.Edit"));
        booksPermission.AddChild(FYPTourneyProPermissions.Books.Delete, L("Permission:Books.Delete"));

        //Define your own permissions here. Example:
        //myGroup.AddPermission(FYPTourneyProPermissions.MyPermission1, L("Permission:MyPermission1"));

        var todoItemPermission = myGroup.AddPermission(FYPTourneyProPermissions.TodoItems.Default, L("Permission:TodoItems"));
        todoItemPermission.AddChild(FYPTourneyProPermissions.TodoItems.Create, L("Permission:TodoItems.Create"));
        todoItemPermission.AddChild(FYPTourneyProPermissions.TodoItems.Edit, L("Permission:TodoItems.Edit"));
        todoItemPermission.AddChild(FYPTourneyProPermissions.TodoItems.Delete, L("Permission:TodoItems.Delete"));



        var categoryPermission = myGroup.AddPermission(FYPTourneyProPermissions.Categories.Default, L("Permission:Categories"));
        categoryPermission.AddChild(FYPTourneyProPermissions.Categories.Create, L("Permission:Categories.Create")); 
        categoryPermission.AddChild(FYPTourneyProPermissions.Categories.Delete, L("Permission:Categories.Delete"));

        var DrawPermission = myGroup.AddPermission(FYPTourneyProPermissions.Draws.Default, L("Permission:Draws"));
        DrawPermission.AddChild(FYPTourneyProPermissions.Draws.Generate, L("Permission:Draws.Generate"));

        var MatchScorePermission = myGroup.AddPermission(FYPTourneyProPermissions.MatchScores.Default, L("Permission:MatchScores"));
        MatchScorePermission.AddChild(FYPTourneyProPermissions.MatchScores.Save, L("Permission:MatchScores.Save"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FYPTourneyProResource>(name);

    }
}
