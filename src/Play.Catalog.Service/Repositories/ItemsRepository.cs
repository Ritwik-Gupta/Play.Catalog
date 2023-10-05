using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using Play.Catalog.Service.Entities;
using Play.Catalog.Service.Models;

namespace Play.Catalog.Service.Repositories 
{
    public class ItemsRepository 
    {
        private readonly IMongoCollection<Item> _items;
        private readonly FilterDefinitionBuilder<Item> filterBuilder;
        public ItemsRepository(ICatalogDatabaseSettings settings)
        {   
            var client = new MongoClient(settings.ConnectionString);
            var database = client.GetDatabase(settings.DatabaseName);

            _items = database.GetCollection<Item>(settings.ItemsCollectionName);
        }

        public async Task<List<Item>> GetAllAsync()
        {
            return await _items.Find(filterBuilder.Empty).ToListAsync();
        }

        public async Task<Item> GetAsync(Guid id) 
        {
            var item = _items.Find(filterBuilder.Eq(x => x.Id, id)).FirstOrDefaultAsync() ?? throw new KeyNotFoundException();
            return await item;
        }

        public async Task CreateAsync(Item item)
        {
            if(item == null) { throw new ArgumentNullException(nameof(item)); }

            await _items.InsertOneAsync(item);
        }

        public async Task UpdateAsync(Guid id, Item item) 
        {
            if(item == null) { throw new ArgumentNullException(); }

            var _ = _items.Find(filterBuilder.Eq(x => x.Id, id)).FirstOrDefaultAsync() ?? throw new KeyNotFoundException();

            await _items.FindOneAndUpdateAsync(
                filterBuilder.Eq(x => x.Id, id), 
                Builders<Item>.Update
                .Set(p => p.Name, item.Name)
                .Set(p => p.Description, item.Description)
                .Set(p => p.Price, item.Price));
        }

        public async Task RemoveAsync(Guid id)
        {
            var item = await _items.Find(filterBuilder.Eq(x => x.Id, id)).FirstOrDefaultAsync() ?? throw new KeyNotFoundException();

            await _items.DeleteOneAsync(filterBuilder.Eq(x => x.Id, id));
        }

    }
}