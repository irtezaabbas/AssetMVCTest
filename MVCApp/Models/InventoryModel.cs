using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MVCApp.Models
{
    public class InventoryModel
    {
        public int Id { get; set; }
        
        [Range(10000,999999, ErrorMessage = "PLEASE ENTER A VALID ASSET TAG")]
        [Display(Name = "Asset Tag")]
        [Required(ErrorMessage ="You need to enter a asset tag.")]
        public string AssetTag { get; set; }

        [Display(Name = "Asset State")]
        [Required(ErrorMessage = "You need to enter a asset state.")]
        public string AssetState { get; set; }

        [Display(Name = "Used By")]
        [Required(ErrorMessage = "You need to enter the student id.")]
        public string UsedBy { get; set; }

        [Display(Name = "Usage Type")]
        public string UsageType { get; set; }

        [Display(Name = "Assigned on")]
        public DateTime AssignedOn { get; set; }

    }
}