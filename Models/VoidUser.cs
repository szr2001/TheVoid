using Microsoft.AspNetCore.Identity;

namespace TheVoid.Models
{
    public class VoidUser:IdentityUser
    {
        public DateTime LastWroteToVoid { get; set; }
        public DateTime LastReadFromVoid { get; set; }
        public int RetrivedVoidMessages { get; set; }
        public int AddedVoidMessages { get; set; }
    }
}
