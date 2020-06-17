using System.Collections;
using System.Collections.Generic;

namespace TodoApi.Models
{
    public class Like
    {
        public int LikerId { get; set; }
        public int LikedId { get; set; }
        public User Liker { get; set; }
        public User Liked { get; set; }

    }
}