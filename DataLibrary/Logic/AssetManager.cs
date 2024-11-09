using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DataLibrary.DataAccess;
using DataLibrary.Models;
using System.Net.Http;
using System.Net.Http.Headers;
using Newtonsoft.Json;

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

        public static async Task<List<AssetModel>> LoadAssetsFromFreshservice(string apiKey)
        {
            var freshserviceAssets = await FreshserviceClient.GetAssetsFromFreshserviceAsync(apiKey);

            foreach (var asset in freshserviceAssets)
            {
                CreateAsset(asset.AssetTag, asset.AssetState, asset.UsedBy, asset.UsageType, asset.AssignedOn);
            }

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
    }
}

