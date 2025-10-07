using Dapper;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using Vax.Data.Context;
using Vax.Repository.Interface;

namespace Vax.Repository.Implmentation
{
	public class GenericRepository<T> : IGenericRepository<T> where T : class
	{
		private readonly Vaxdbcontext _context;
		private readonly IDbConnection _connection;
		public GenericRepository(Vaxdbcontext context)
        {
			_context = context;
			_connection = _context.Database.GetDbConnection();
		}
        public T Add(T entity)
		{
			_context.Set<T>().Add(entity);
			return entity;
		}

		public async Task<T> AddAsync(T entity)
		{
			await _context.Set<T>().AddAsync(entity);
			return entity;
		}

		public IEnumerable<T> AddRange(IEnumerable<T> entities)
		{
			_context.Set<T>().AddRange(entities);
			return entities;
		}

		public async Task<IEnumerable<T>> AddRangeAsync(IEnumerable<T> entities)
		{
			await _context.Set<T>().AddRangeAsync(entities);
			return entities;
		}

		public void Delete(T entity)
		{
			_context.Set<T>().Remove(entity);
		}

		public void DeleteRange(IEnumerable<T> entities)
		{
			_context.Set<T>().RemoveRange(entities);
		}

		public T Find(Expression<Func<T, bool>> criteria, string[] includes = null)
		{
			IQueryable<T> query = _context.Set<T>();

			if(includes != null)
			{
				foreach (var item in includes)
				{
					query = query.Include(item);
				}
			}

			return query.SingleOrDefault(criteria);
		}

		public IEnumerable<T> FindAll(Expression<Func<T, bool>> criteria, string[] includes = null)
		{
			IQueryable<T> query = _context.Set<T>();

			if(includes != null)
			{
				foreach (var item in includes)
				{
					query = query.Include(item);
				}
			}
			return query.Where(criteria).ToList();
		}

		public async Task<IEnumerable<T>> FindAllAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
		{
			IQueryable<T> query = _context.Set<T>();

			if(includes != null)
			{
				foreach (var item in includes)
				{
					query = query.Include(item);
				}
			}

			return await query.Where(criteria).ToListAsync();
		}

		public async Task<T> FindAsync(Expression<Func<T, bool>> criteria, string[] includes = null)
		{
			IQueryable<T> query = _context.Set<T>();

			if(includes != null)
			{
				foreach (var item in includes)
				{
					query = query.Include(item);
				}
			}

			return await query.SingleOrDefaultAsync(criteria);
		}

		public IEnumerable<T> DapperGetAll()
		{
			return _connection.Query<T>($"select * from {typeof(T).Name}s");
		}

		public IEnumerable<T> GetAll()
		{
			return _context.Set<T>().ToList();
		}

		public async Task<IEnumerable<T>> DapperGetAllAsync()
		{
			#region For Test the Faster is Dapper or EFCore
			//Stopwatch stopwatch = new Stopwatch();
			//stopwatch.Start();
			//if (_connection.State != ConnectionState.Open)
			//{
			//	_connection.Open();
			//}
			//var result = await _connection.QueryAsync<T>($"select * from {typeof(T).Name}s");
			//stopwatch.Stop();
			//Console.WriteLine($"Dapper time: {stopwatch.ElapsedMilliseconds} ms");
			//return result; 
			#endregion

			if (_connection.State != ConnectionState.Open)
			{
				_connection.Open();
			}
			return await _connection.QueryAsync<T>($"select * from {typeof(T).Name}s");
		}

		public async Task<IEnumerable<T>> GetAllAsync()
		{
			#region For Test the Faster is Dapper or EFCore
			//Stopwatch stopwatch = new Stopwatch();
			//stopwatch.Start();
			//var result = await _context.Set<T>().ToListAsync();
			//stopwatch.Stop();
			//Console.WriteLine($"EFCore time: {stopwatch.ElapsedMilliseconds} ms");
			//return result; 
			#endregion

			return await _context.Set<T>().ToListAsync();
		}

		public T GetById(int Id)
		{
			 return _context.Set<T>().Find(Id);
			
		}

		public async Task<T> GetByIdAsync(int Id)
		{
			return await _context.Set<T>().FindAsync(Id);
		}

		public T Update(T entity)
		{
			_context.Set<T>().Update(entity);
			return entity;
		}
	}
}
