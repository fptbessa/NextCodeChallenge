using System;
using System.Collections.Generic;

#nullable disable

namespace NextCodeChallenge.Models.Entities
{
    public partial class Topic
    {
        public int TopicId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public int CreatorUserId { get; set; }
        public DateTime CreationDate { get; set; }

        public virtual User CreatorUser { get; set; }
    }
}
