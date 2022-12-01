using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models
{
    public class AssetTermValue
    {
        public int Id { get; set; }
        public double Count { get; set; }
        public bool IsOptional { get; set; } = false;
        [ForeignKey("AssetType")]
        public int AssetTypeId { get; set; }
        public virtual AssetType AssetType { get; set; }
        [ForeignKey("AssetTerm")]
        public int AssetTermId { get; set; }
        public virtual AssetTerm AssetTerm { get; set; }

        public bool IsDeleted { get; set; }

    }
}
