namespace Executorlibs.AspNetCore.Identity
{
    public class RoleClaimsLoadingOptions : ClaimsLoadingOptions
    {
        public bool LoadUserRoles { get; set; }

        public bool LoadRoleClaims { get; set; }
    }
}
