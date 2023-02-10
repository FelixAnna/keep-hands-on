using System.ComponentModel;

namespace HSS.SignalRDemo.Models
{
    public class UserModel
    {
        [DisplayName("User Name")]
        public string UserName { get; set; } = null!;

        [DisplayName("Password")]
        public string Password { get; set; } = null!;
    }
}