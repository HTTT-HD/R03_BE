using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Repositories
{
    public interface IMongoRepository
    {
        Task AddAsync<TEntity>(TEntity obj) where TEntity : class;
        Task AddRangeAsync<TEntity>(IEnumerable<TEntity> objs) where TEntity : class;
        Task UpdateAsnyc<TEntity>(string id, TEntity obj) where TEntity : class;
        Task<TEntity> GetById<TEntity>(string id) where TEntity : class;
        IQueryable<TEntity> AsQueryable<TEntity>() where TEntity : class;
        Task<List<TEntity>> FindAllAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class;
        Task<List<TEntity>> GetAllAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class;
        Task<List<TEntity>> FindAllAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, Expression<Func<TEntity, object>> sort = null)
            where TEntity : class;
        Task<TEntity> FirsOfDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class;
        Task<TEntity> FirsOfDefaultAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null, 
            Expression<Func<TEntity, object>> sort = null) where TEntity : class;
        Task<TEntity> FirsOfDefaultAsync<TEntity>(BsonDocument bsons) where TEntity : class;
        Task<TEntity> FirsOfDefaultAsync<TEntity>(BsonDocument bsons, SortDefinition<TEntity> sort) where TEntity : class;
        Task<List<TEntity>> FindForPageAsync<TEntity>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> filter = null) where TEntity : class;
        Task<List<TEntity>> FindForPageAsync<TEntity>(int pageIndex, int pageSize, Expression<Func<TEntity, bool>> filter = null,
            Expression<Func<TEntity, object>> sort = null) where TEntity : class;

        Task<List<TEntity>> FindAllAsync<TEntity>(BsonDocument bsons) where TEntity : class;
        Task<List<TEntity>> FindAllAsync<TEntity>(BsonDocument bsons, SortDefinition<TEntity> sort) where TEntity : class;
        Task<List<TEntity>> FindForPageAsync<TEntity>(BsonDocument bsons, int pageIndex, int pageSize) where TEntity : class;
        Task<List<TEntity>> FindForPageAsync<TEntity>(BsonDocument bsons, SortDefinition<TEntity> sort, int pageIndex, int pageSize)
            where TEntity : class;
        Task<long> CountAsync<TEntity>(BsonDocument bsons) where TEntity : class;
        Task<long> CountAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class;
        Task<long> CountAsync<TEntity>() where TEntity : class;
        Task<bool> AnyAsync<TEntity>(BsonDocument bsons) where TEntity : class;
        Task<bool> AnyLiqnAsync<TEntity>(Expression<Func<TEntity, bool>> filter = null) where TEntity : class;
        Task Remove<TEntity>(string id) where TEntity : class;
        Task DeleteAsync<TEntity>(string id, TEntity entity) where TEntity : class;
    }
}
