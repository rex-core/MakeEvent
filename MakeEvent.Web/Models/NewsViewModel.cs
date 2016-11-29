﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MakeEvent.Web.Models
{
    public class NewsViewModel
    {
        public int Id { get; set; }

        public string EncodedImage { get; set; }

        public string DefaultHeader { get; set; }
        public string DefaultDescription { get; set; }
        public string DefaultShortDescription { get; set; }

        public ICollection<NewsLocalizationViewModel> NewsLocalizations { get; set; }
    }
}