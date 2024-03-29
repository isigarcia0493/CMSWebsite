﻿using CMSWebsite.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace CMSWebsite.Areas.Admin.ViewModels
{
    public class AlbumViewModel
    {
        [Key]
        public int AlbumId { get; set; }

        [Required(ErrorMessage = "Name is required")]
        [DisplayName("Album Name")]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required(ErrorMessage = "Short description is required")]
        [DisplayName("Short Decription")]
        [MaxLength(50)]
        public string ShortDescription { get; set; }

        [Required(ErrorMessage = "Long description is required")]
        [DisplayName("Long Description")]
        [MaxLength(500)]
        public string LongDescription { get; set; }

        public string PublicId { get; set; }

        [DisplayName("Upload Photo")]
        public IFormFile AlbumImageUrl { get; set; }

        public ICollection<Image> Images { get; set; }

        //Relationship
        public int CategoryId { get; set; }

        [ForeignKey("CategoryId")]
        public Category Category { get; set; }
    }
}
