using Common.Helpers;

using Domain.Data;
using Domain.Models;
using Microsoft.AspNetCore.Http;

using MongoDB.Bson;
using MongoDB.Driver;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public class MongoRepository : BasePrincipal, IMongoRepository
    {
        private readonly IMongoDatabase _context;

        public MongoRepository(IHttpContextAccessor httpContextAccessor, IMongoContext context) : base(httpContextAccessor)
        {
            _context = context.Database;
        }

        public async Task<TEntity> AddAsync<TEntity>(TEntity entity) where TEntity : class
        {
            if (entity is IBaseModel model)
            {
                model.Id = Guid.NewGuid();
                model.CreateBy = _userId;
                model.CreateAt = DateTime.Now;
                model.IsDeleted = false;
                await _context.GetCollection<TEntity>(typeof(TEntity).Name).InsertOneAsync(entity);
                return entity;
            }
            else
            {
                await _context.GetCollection<TEntity>(typeof(TEntity).Name).InsertOneAsync(entity);
                return entity;
            }
        }

        public async Task AddRangeAsync<TEntity>(IEnumerable<TEntity> entities) where TEntity : class
        {
            foreach (var entity in entities)
            {
                if (entity is IBaseModel model)
                {
                    model.Id = Guid.NewGuid();
                    model.CreateBy = _userId;
                    model.CreateAt = DateTime.Now;
                    model.IsDeleted = false;
                }
            }
            await _context.GetCollection<TEntity>(typeof(TEntity).Name).InsertManyAsync(entities);
        }

        public async Task<TEntity> UpdateAsnyc<TEntity>(string id, TEntity entity) where TEntity : class
        {
            if (entity is IBaseModel model)
            {
                model.UpdateBy = _userId;
                model.UpdateAt = DateTime.Now;
                await _context.GetCollection<TEntity>(typeof(TEntity).Name).ReplaceOneAsync(FilterId<TEntity>(id), entity);
                return entity;
            }
            await _context.GetCollection<TEntity>(typeof(TEntity).Name).ReplaceOneAsync(FilterId<TEntity>(id), entity);
            return entity;
        }

        public async Task<IEnumerable<TEntity>> GetAll<TEntity>() where TEntity : class
        {
            return await _context.GetCollection<TEntity>(typeof(TEntity).Name).Find(IsUse<TEntity>()).ToListAsync();
        }

        public IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : class
        {
            return _context.GetCollection<TEntity>(typeof(TEntity).Name).AsQueryable();
        }

        public IQueryable<TEntity> AsQueryable<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IBaseModel)))
            {
                filter = filter.And(x => !((IBaseModel)x).IsDeleted);
            }
            return _context.GetCollection<TEntity>(typeof(TEntity).Name).AsQueryable();
        }


        public async Task<List<TEntity>> FindAllAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IBaseModel)))
            {
                filter = filter.And(x => !((IBaseModel)x).IsDeleted);
            }
            return await _context.GetCollection<TEntity>(typeof(TEntity).Name).Find(filter).ToListAsync();
        }

        public async Task<TEntity> FirsOfDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IBaseModel)))
            {
                filter = filter.And(x => !((IBaseModel)x).IsDeleted);
            }
            return await _context.GetCollection<TEntity>(typeof(TEntity).Name).Find(filter).FirstOrDefaultAsync();
        }

        public async Task<List<TEntity>> FindForPageAsync<TEntity>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IBaseModel)))
            {
                filter = filter.And(x => !((IBaseModel)x).IsDeleted);
            }

            return await _context.GetCollection<TEntity>(typeof(TEntity).Name).Find(filter)
                .Skip((pageIndex - 1) * pageSize).Limit(pageSize)
                .ToListAsync();
        }

        public async Task<List<TEntity>> FindAllAsync<TEntity>(BsonDocument bsons) where TEntity : class
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IBaseModel)))
            {
                var check = bsons.Any(x => x.Name == nameof(BaseModel.IsDeleted));
                if (!check)
                {
                    bsons.Add(nameof(BaseModel.IsDeleted), false);
                }
            }
            return await _context.GetCollection<TEntity>(typeof(TEntity).Name).Aggregate().Match(bsons).ToListAsync();
        }

        public async Task<List<TEntity>> FindForPageAsync<TEntity>(BsonDocument bsons, int pageIndex, int pageSize) where TEntity : class
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IBaseModel)))
            {
                var check = bsons.Any(x => x.Name == nameof(BaseModel.IsDeleted));
                if (!check)
                {
                    bsons.Add(nameof(BaseModel.IsDeleted), false);
                }
            }

            return await _context.GetCollection<TEntity>(typeof(TEntity).Name)
                .Aggregate().Match(bsons)
                .Skip((pageIndex - 1) * pageSize).Limit(pageSize)
                .ToListAsync();
        }


        public async Task<long> CountAsync<TEntity>(BsonDocument bsons) where TEntity : class
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IBaseModel)))
            {
                var check = bsons.Any(x => x.Name == nameof(BaseModel.IsDeleted));
                if (!check)
                {
                    bsons.Add(nameof(BaseModel.IsDeleted), false);
                }

            }
            return await _context.GetCollection<TEntity>(typeof(TEntity).Name).CountDocumentsAsync(bsons);
        }

        public async Task<long> CountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IBaseModel)))
            {
                filter = filter.And(x => !((IBaseModel)x).IsDeleted);
            }

            return await _context.GetCollection<TEntity>(typeof(TEntity).Name).Find(filter).CountDocumentsAsync();
        }

        public async Task<long> CountAsync<TEntity>() where TEntity : class
        {
            return await _context.GetCollection<TEntity>(typeof(TEntity).Name).CountDocumentsAsync(null);
        }

        public async Task<bool> AnyAsync<TEntity>(BsonDocument bsons) where TEntity : class
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IBaseModel)))
            {
                var check = bsons.Any(x => x.Name == nameof(BaseModel.IsDeleted));
                if (!check)
                {
                    bsons.Add(nameof(BaseModel.IsDeleted), false);
                }
            }
            return await _context.GetCollection<TEntity>(typeof(TEntity).Name).Find(bsons).AnyAsync();
        }
        public async Task<bool> AnyLiqnAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class
        {
            if (typeof(TEntity).GetInterfaces().Contains(typeof(IBaseModel)))
            {
                filter = filter.And(x => !((IBaseModel)x).IsDeleted);
            }
            return await _context.GetCollection<TEntity>(typeof(TEntity).Name).Find(filter).AnyAsync();
        }

        public async Task<TEntity> GetById<TEntity>(string id) where TEntity : class
        {
            return await _context.GetCollection<TEntity>(typeof(TEntity).Name).Find(FilterId<TEntity>(id)).FirstOrDefaultAsync();
        }

        public async Task Remove<TEntity>(string id) where TEntity : class
        {
            await _context.GetCollection<TEntity>(typeof(TEntity).Name).DeleteOneAsync(FilterId<TEntity>(id));
        }

        public async Task DeleteAsync<TEntity>(string id, TEntity entity) where TEntity : class
        {
            if (entity is IBaseModel model)
            {
                model.IsDeleted = false;
                await UpdateAsnyc<TEntity>(model.Id.ToString(), entity);
            }
            await _context.GetCollection<TEntity>(typeof(TEntity).Name).DeleteOneAsync(id);
        }

        private static FilterDefinition<TEntity> FilterId<TEntity>(string key) where TEntity : class
        {
            return Builders<TEntity>.Filter.Eq("Id", key);
        }

        private static FilterDefinition<TEntity> IsUse<TEntity>() where TEntity : class
        {
            return Builders<TEntity>.Filter.Eq("IsDeleted", false);
        }
    }
}
