﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MakeEvent.Web.Models.ViewModels.Admin
{
    public class Comment
    {
        public int Id { get; set; }
        public int NewsId { get; set; }
        public string AuthorName { get; set; }
        public string AuthorEmail { get; set; }
        public string Text { get; set; }
        public bool IsVisible { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}