namespace LSOne.DataLayer.BusinessObjects.Enums
{
    public enum GroupGrantMode
    {
        Deny = 0,
        Grant = 1
    };

    public enum UserGrantMode
    {
        Deny = 0,
        Grant = 1,
        Inherit = 2
    };

    public enum PermissionState
    {
        ExclusiveDeny = 0,
        ExclusiveGrant = 1,
        InheritFromGroupGrant = 2,
        InheritFromGroupNoPermission = 3
    };
}
