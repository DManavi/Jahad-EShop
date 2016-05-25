using Ease.Database.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    /// <summary>
    /// Gallery
    /// </summary>
    public sealed class Gallery : EntityBase<Guid>
    {
        /// <summary>
        /// Title
        /// </summary>
        [Display(Name = "عنوان")]
        [StringLength(maximumLength: 100)]
        public string Title { get; set; }

        /// <summary>
        /// Status
        /// </summary>
        [Display(Name = "وضعیت")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Items
        /// </summary>
        public ICollection<GalleryItem> Items { get; set; }
    }
}