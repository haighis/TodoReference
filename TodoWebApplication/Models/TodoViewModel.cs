using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace WebApplicationSystem1.Models
{
    public class TodoViewModel
    {
        [Required(
            AllowEmptyStrings = false, 
            ErrorMessage = "Please enter a task name"
            )]
        [MaxLength(256)]
        [Display(Name = "Task Name")]
        public string TaskName { get; set; }
    }
}