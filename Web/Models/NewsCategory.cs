using Ease.Database.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    /// <summary>
    /// News category
    /// </summary>
    public class NewsCategory : EntityBase<Guid>
    {
        /// <summary>
        /// Title
        /// </summary>
        [Display(Name = "عنوان")]
        [StringLength(maximumLength: 100)]
        public string Title { get; set; }

        /// <summary>
        /// Enabled or not
        /// </summary>
        [Display(Name = "فعال")]
        public bool Enabled { get; set; }

        /// <summary>
        /// News
        /// </summary>
        public virtual ICollection<News> News { get; set; }
    }
}