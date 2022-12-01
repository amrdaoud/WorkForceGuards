using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace WorkForceManagementV0.Models.DTO
{
    public class AssetDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Barcode { get; set; }
        public string TypeName { get; set; }
        public int TypeId { get; set; }
        public string LocationName { get; set; }
        public int LocationId { get; set; }

        public string Specs { get; set; }
        public bool IsDisabled { get; set; } = false;


    }

    public class FullAssetDto
    {
       public List<AssetDto> Result { get; set; }

       public int ResultSize { get; set; }

    }
}
