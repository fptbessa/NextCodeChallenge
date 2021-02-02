using System;
using System.Collections.Generic;

#nullable disable

namespace NextCodeChallenge.Models.Entities
{
    public partial class User
    {
        public User()
        {
            Topics = new HashSet<Topic>();
        }

        public int UserId { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }

        public virtual ICollection<Topic> Topics { get; set; }
    }
}
