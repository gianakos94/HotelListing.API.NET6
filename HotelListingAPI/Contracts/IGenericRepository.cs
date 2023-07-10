namespace HotelListingAPI.Contracts
{
    public interface IGenericRepository<T> where T : class
    {
        Task<T> GetAsync(int? id);

        // Take a list of T and bring all the records
        Task<List<T>> GetAllAsync();
        
        Task<T> AddAsync(T entity);

        Task UpdateAsync(T entity);
        Task DeleteAsync(int id);

        Task<bool> Exists(int id);

    }
   
}
 