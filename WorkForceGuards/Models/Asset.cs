using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using WorkForceManagementV0.Models.Interfaces;

namespace WorkForceManagementV0.Models
{
    public class Asset
    {
        public int Id { get; set; }
        [Required]
        public string Barcode { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 3, ErrorMessage = "Name between 3 and 100 characters")]
        public string Name { get; set; }
        public string Specs { get; set; }
        public bool IsDisabled { get; set; } = false;
        [ForeignKey("AssetType")]
        public int AssetTypeId { get; set; }
        public virtual AssetType AssetType { get; set; }
        [ForeignKey("Location")]
        public int LocationId { get; set; }
        public virtual Location Location { get; set; }

        public bool IsDeleted { get; set; }
    }
}
