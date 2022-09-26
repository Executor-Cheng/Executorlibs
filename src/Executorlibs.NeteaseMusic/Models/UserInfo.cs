namespace Executorlibs.NeteaseMusic.Models
{
    public class UserInfo
    {
        public string UserName { get; set; }

        public long UserId { get; set; }

        public int VipType { get; set; }

        public UserInfo(string userName, long userId, int vipType)
        {
            UserName = userName;
            UserId = userId;
            VipType = vipType;
        }
    }
}
