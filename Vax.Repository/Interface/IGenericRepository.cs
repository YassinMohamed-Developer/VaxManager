using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Vax.Repository.Interface
{
	public interface IGenericRepository<T> where T : class
	{
		T GetById(int Id);

		Task<T> GetByIdAsync(int Id);

		IEnumerable<T> GetAll();

		Task<IEnumerable<T>> GetAllAsync();

		T Find(Expression<Func<T, bool>> criteria,string[] includes = null);

		Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null);

		IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null);

		Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null);

		T Add(T entity);

		Task<T> AddAsync(T entity);

		IEnumerable<T> AddRange(IEnumerable<T> entities);

		Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities);

		T Update(T entity);

		void Delete(T entity);
		void DeleteRange(IEnumerable<T> entities);

		Task<IEnumerable<T>> DapperGetAllAsync();

		IEnumerable<T> DapperGetAll();
	}
}
