using DataLibrary.DataAccess;
using DataLibrary.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataLibrary.Logic
{
    public static class AssetManager
    {
        public static int CreateAsset(string AssetTag, string AssetState, string UsedBy, string UsageType,DateTime AssignedOn)
        {
            AssetModel data = new AssetModel
            {
                AssetTag = AssetTag,
                AssetState = AssetState,
                UsedBy = UsedBy,
                UsageType = UsageType,
                AssignedOn = AssignedOn
            };

            string sql = @"insert into dbo.Assets (AssetTag, AssetState, UsedBy, UsageType, AssignedOn)
                          values(@AssetTag, @AssetState, @UsedBy, @UsageType, @AssignedOn);";

            return SQLDataAccess.SaveData(sql, data);
        }

        public static List<AssetModel> LoadAssets()
        {
            string sql = @"select Id, AssetTag, AssetState, UsedBy, UsageType, AssignedOn from dbo.Assets;";

            return SQLDataAccess.LoadData<AssetModel>(sql); 

        }
    }
}
