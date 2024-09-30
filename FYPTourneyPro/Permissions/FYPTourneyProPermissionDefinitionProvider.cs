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
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<FYPTourneyProResource>(name);
    }
}
