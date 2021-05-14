using Maincotech.ExamAssistant.Dtos;
using Maincotech.ExamAssistant.Services.Models;
using Microsoft.Azure.Cosmos;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Maincotech.ExamAssistant.Services
{
    public class SettingService : ISettingService, IDisposable
    {
        private const string DatabaseId = "examassitantdb";
        private const string ContainerId = "usersettings";
        private const string ConnectionStringName = "ExamAssitantCosmosDb";

        private readonly CosmosClient _cosmosClient;

        public SettingService(IConfiguration configuration)
        {
            _cosmosClient = new CosmosClient(configuration.GetConnectionString(ConnectionStringName));
        }

        public async Task<FirebaseSettingDto> CreateOrFirebaseSetting(FirebaseSettingDto dto)
        {
            var container = _cosmosClient.GetContainer(DatabaseId, ContainerId);
            var entity = dto.To<FirebaseSetting>();
            if (string.IsNullOrEmpty(dto.Id))
            {
                entity.Id = Guid.NewGuid().ToString();
                var response = await container.CreateItemAsync<FirebaseSetting>(entity, new PartitionKey(dto.UserId));
                entity = response.Resource;
            }
            else
            {
                var response = await container.ReplaceItemAsync<FirebaseSetting>(entity, entity.Id, new PartitionKey(entity.UserId));
                entity = response.Resource;
            }

            return entity.To<FirebaseSettingDto>();
        }

        public void Dispose()
        {
            _cosmosClient?.Dispose();
        }

        public async Task<FirebaseSettingDto> GetFirebaseSetting(string userID)
        {
            var sqlQueryText = $"SELECT * FROM c WHERE c.userid = '{userID}'";
            var container = _cosmosClient.GetContainer(DatabaseId, ContainerId);

            QueryDefinition queryDefinition = new QueryDefinition(sqlQueryText);

            ;
            List<FirebaseSetting> entities = new();

            var iterator = container.GetItemQueryIterator<FirebaseSetting>(queryDefinition);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                entities.AddRange(response.Resource);
            }
            //await foreach (FirebaseSetting entity in container.GetItemQueryIterator<FirebaseSetting>(queryDefinition))
            //{
            //    entities.Add(entity);
            //}

            if (entities.Count == 0)
            {
                return null;
            }
            return entities.First().To<FirebaseSettingDto>();
        }

        public async Task<FirebaseSettingDto> GetFirebaseSettingByProjectId(string projectId)
        {
            var sqlQueryText = $"SELECT * FROM c WHERE c.projectid = '{projectId}'";
            var container = _cosmosClient.GetContainer(DatabaseId, ContainerId);

            QueryDefinition queryDefinition = new(sqlQueryText);
            List<FirebaseSetting> entities = new();
            var iterator = container.GetItemQueryIterator<FirebaseSetting>(queryDefinition);

            while (iterator.HasMoreResults)
            {
                var response = await iterator.ReadNextAsync();
                entities.AddRange(response.Resource);
            }
            //await foreach (FirebaseSetting entity in container.GetItemQueryIterator<FirebaseSetting>(queryDefinition))
            //{
            //    entities.Add(entity);
            //}

            if (entities.Count == 0)
            {
                return null;
            }
            return entities.First().To<FirebaseSettingDto>();
        }
    }
}