﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NewsFlow.Models
{
    public class NewsLike
    {
        public int NewsId { get; set; } 
        public string UserId { get; set; } 
        public DateTime LikedAt { get; set; } 
    }
}
