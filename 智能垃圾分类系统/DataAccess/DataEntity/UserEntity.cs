using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace 智能垃圾分类系统.DataAccess.DataEntity
{
    public class UserEntity
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string RealName { get; set; }
        public string Password { get; set; }
        public string Avatar { get; set; }
        public int Gender { get; set; }
        public int Points { get; set; }
        public string UserPhone { get; set; }
        public string Email { get; set; }
    }
}
