using System;
using System.Collections.Generic;

namespace SWD.SAPelearning.Repository.Models
{
    public partial class Role
    {
        public Role()
        {
            Users = new HashSet<Usertb>();
        }

        public string Roleid { get; set; } = null!;
        public string Rolename { get; set; } = null!;

        public virtual ICollection<Usertb> Users { get; set; }
    }
}
