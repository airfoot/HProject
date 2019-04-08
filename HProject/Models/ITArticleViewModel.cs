using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace HProject.Models
{
    public class ITArticleViewModel
    {

        public string Id { get; set; }
        [Required]
        [Display(Name = "名称")]
        public string Name { get; set; }
        [Required]
        [Display(Name = "描述")]
        public string Description { get; set; }

        [Required]
        [Display(Name = "库存")]
        public int Stock { get; set; }
    }
}