namespace Executorlibs.AspNetCore.Identity
{
    public class ClaimsLoadingOptions
    {
        public bool LoadUserId { get; set; }

        public bool LoadUserName { get; set; }

        public bool LoadUserEmail { get; set; }

        public bool LoadUserSecurityStamp { get; set; }

        public bool LoadUserClaims { get; set; }
    }
}
