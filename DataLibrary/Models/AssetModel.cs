using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Models
{
    public class AssetModel
    {
        public int Id { get; set; }
        public string AssetTag { get; set; }
        public string AssetState { get; set; }
        public string UsedBy { get; set; }
        public string UsageType { get; set; }
        public DateTime AssignedOn { get; set; }
        public DateTime LastModified { get; set;

    }
}
