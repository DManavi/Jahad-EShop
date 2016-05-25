using Ease.Database.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    /// <summary>
    /// Gallery item
    /// </summary>
    public class GalleryItem : EntityBase<Guid>
    {
        /// <summary>
        /// Title
        /// </summary>
        [Display(Name = "عنوان")]
        [StringLength(maximumLength: 100)]
        public string Title { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [Display(Name = "محتوا")]
        public string Content { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public GalleryItemType Type { get; set; }

        /// <summary>
        /// Visits
        /// </summary>
        [Display(Name = "بازدید")]
        public int Visits { get; set; }

        /// <summary>
        /// Last update
        /// </summary>
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Gallery
        /// </summary>
        public virtual Gallery Gallery { get; set; }

        /// <summary>
        /// Author
        /// </summary>
        public virtual ApplicationUser Author { get; set; }
    }

    /// <summary>
    /// Galery item type
    /// </summary>
    public enum GalleryItemType
    {
        /// <summary>
        /// Image
        /// </summary>
        Image = 0
    }
}