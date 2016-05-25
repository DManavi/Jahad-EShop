using Ease.Database.EntityFramework;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Web.Models
{
    /// <summary>
    /// Article category
    /// </summary>
    public class ArticleCategory : EntityBase<Guid>
    {
        /// <summary>
        /// Category title
        /// </summary>
        [Display(Name ="عنوان")]
        [StringLength(maximumLength: 100)]
        public string Title { get; set; }

        /// <summary>
        /// Enabled
        /// </summary>
        [Display(Name = "وضعیت")]
        public bool Enabled { get; set; }

        /// <summary>
        /// Articles
        /// </summary>
        public virtual ICollection<Article> Articles { get; set; }
    }
}