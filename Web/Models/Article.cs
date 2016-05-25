using Ease.Database.EntityFramework;
using System;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    /// <summary>
    /// Article
    /// </summary>
    public class Article : EntityBase<Guid>
    {
        /// <summary>
        /// Title
        /// </summary>
        [Display(Name = "عنوان")]
        [StringLength(maximumLength: 100)]
        public string Title { get; set; }

        /// <summary>
        /// Article abstract
        /// </summary>
        [Display(Name = "چکیده")]
        [StringLength(maximumLength: 1024)]
        public string Abstract { get; set; }

        /// <summary>
        /// Content
        /// </summary>
        [Display(Name = "متن کامل")]
        public string Content { get; set; }

        /// <summary>
        /// Tags
        /// </summary>
        [Display(Name = "تگ ها")]
        public string Tags { get; set; }

        /// <summary>
        /// Visits
        /// </summary>
        [Display(Name = "تعداد بازدید")]
        public int Visits { get; set; }

        /// <summary>
        /// Has image
        /// </summary>
        public bool HasImage { get; set; }

        /// <summary>
        /// Last update
        /// </summary>
        [Display(Name = "آخرین به روز رسانی")]
        public DateTime LastUpdate { get; set; }

        /// <summary>
        /// Category
        /// </summary>
        public virtual ArticleCategory Category { get; set; }

        /// <summary>
        /// Author
        /// </summary>
        public virtual ApplicationUser Author { get; set; }
    }
}