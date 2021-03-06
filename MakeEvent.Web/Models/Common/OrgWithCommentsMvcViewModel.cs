﻿using MakeEvent.Web.Models.Admin;
using System.Collections.Generic;

namespace MakeEvent.Web.Models.Common
{
    public class OrgWithCommentsMvcViewModel
    {
        public string OwnerId { get; set; }
        public int?   ImageId { get; set; }

        public string Name        { get; set; }
        public string Email       { get; set; }
        public string Password    { get; set; }
        public string Description { get; set; }
        public string PhoneNumber { get; set; }
        public string Website     { get; set; }
        public string City        { get; set; }
        public string Street      { get; set; }
        public string Office      { get; set; }
        public string BillNumber  { get; set; }

        public byte[] ImageData     { get; set; }
        public string ImageMimeType { get; set; }

        public List<CommentMvcViewModel> Comments { get; set; }

        public CommentMvcViewModel Comment { get; set; }
    }
}