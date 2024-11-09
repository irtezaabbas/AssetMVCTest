using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataLibrary.DataAccess;
using DataLibrary.Models;

namespace DataLibrary.Logic
{
    public static class AssetManager
    {
        public static int CreateAsset(string AssetTag, string AssetState, string UsedBy, string UsageType, DateTime AssignedOn)
        {
            AssetModel data = new AssetModel
            {
                AssetTag = AssetTag,
                AssetState = AssetState,
                UsedBy = UsedBy,
                UsageType = UsageType,
                AssignedOn = AssignedOn,
                LastModified = DateTime.Now
            };

            string sql = @"insert into dbo.Assets (AssetTag, AssetState, UsedBy, UsageType, AssignedOn, LastModified)
                           values(@AssetTag, @AssetState, @UsedBy, @UsageType, @AssignedOn, @LastModified);";

            return SQLDataAccess.SaveData(sql, data);
        }

        public static List<AssetModel> LoadAssets()
        {
            string sql = @"select Id, AssetTag, AssetState, UsedBy, UsageType, AssignedOn from dbo.Assets;";

            return SQLDataAccess.LoadData<AssetModel>(sql);
        }

        public static async Task<List<AssetModel>> LoadAssetsFromFreshservice(string apiKey)
        {
            var freshserviceAssets = await FreshserviceClient.GetAssetsFromFreshserviceAsync(apiKey);
            return freshserviceAssets;
        }

        public static List<AssetModel> LoadChangedAssets(DateTime lastSyncTime)
        {
            string sql = @"select Id, AssetTag, AssetState, UsedBy, UsageType, AssignedOn 
                           from dbo.Assets
                           where LastModified > @LastSyncTime";

            var parameters = new { LastSyncTime = lastSyncTime };
            return SQLDataAccess.LoadData<AssetModel>(sql, parameters);
        }

        public static async Task UpdateFreshserviceAsync(string apiKey)
        {
            var changedAssets = LoadChangedAssets(DateTime.UtcNow.AddMinutes(-15));

            foreach (var asset in changedAssets)
            {
                await FreshserviceClient.UpdateAssetInFreshserviceAsync(apiKey, asset);
            }
        }

        public static async Task AssignAsset(string assetTag, string assignedTo, DateTime assignedOn, string apiKey)
        {
            AssetModel asset = new AssetModel
            {
                AssetTag = assetTag,
                UsedBy = assignedTo,
                AssignedOn = assignedOn,
                AssetState = "Assigned",
                LastModified = DateTime.Now
            };

            CreateAsset(asset.AssetTag, asset.AssetState, asset.UsedBy, asset.UsageType, asset.AssignedOn);
            await UpdateFreshserviceAsync(apiKey, asset);
        }

        public static List<AssetModel> ViewAssignedAssets()
        {
            string sql = @"select Id, AssetTag, AssetState, UsedBy, UsageType, AssignedOn from dbo.Assets where AssetState = 'Assigned';";
            return SQLDataAccess.LoadData<AssetModel>(sql);
        }
    }
}

