using Dummy1.Localization;
using Volo.Abp.Authorization.Permissions;
using Volo.Abp.Localization;

namespace Dummy1.Permissions;

public class Dummy1PermissionDefinitionProvider : PermissionDefinitionProvider
{
    public override void Define(IPermissionDefinitionContext context)
    {
        var myGroup = context.AddGroup(Dummy1Permissions.GroupName);
        //Define your own permissions here. Example:
        //myGroup.AddPermission(Dummy1Permissions.MyPermission1, L("Permission:MyPermission1"));
    }

    private static LocalizableString L(string name)
    {
        return LocalizableString.Create<Dummy1Resource>(name);
    }
}
