using HotelListingAPI.Models;
using Microsoft.Build.Execution;
using static HotelListingAPI.Models.QueryParameters;

namespace HotelListingAPI.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(int? id);

        Task<TResult?> GetAsync<TResult>(int? id);

        // Take a list of T and bring all the records
        Task<List<T>> GetAllAsync();

        Task<List<TResult>> GetAllAsync<TResult>();

        Task<PagedResult<TResult>> GetAllAsync<TResult>(QueryParameters queryParameters); 
        
        Task<T> AddAsync(T entity);

        Task<TResult> AddAsync <TSource,TResult>(TSource source);

        Task UpdateAsync(T entity);

        Task UpdateAsync<TSource>(int id, TSource source);
        Task DeleteAsync(int id);

        Task<bool> Exists(int id);

    }
   
}
 