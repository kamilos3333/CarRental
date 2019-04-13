using CarRental.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Web;

namespace CarRental.Repository
{
    public class GenericRepository<TEntity> where TEntity : class
    {
        internal ApplicationDbContext dbContext;
        internal DbSet<TEntity> dbSet;

        public GenericRepository(ApplicationDbContext db)
        {
            this.dbContext = db;
            this.dbSet = db.Set<TEntity>();
        }

        public virtual IEnumerable<TEntity> GetAll(
            Expression<Func<TEntity, bool>> filter = null,
            Func<IQueryable<TEntity>, IOrderedQueryable<TEntity>> orderBy = null,
            string includeProperties = "")
        {
            IQueryable<TEntity> query = dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return orderBy(query).ToList();
            }
            else
            {
                return query.ToList();
            }
        }

        public virtual TEntity GetById(object id)
        {
            return dbSet.Find(id);
        }

        public virtual void Insert(TEntity entity)
        {
            dbSet.Add(entity);
        }

        public virtual void Delete(object id)
        {
            TEntity entityDelete = dbSet.Find(id);
            Delete(entityDelete);
        }

        public virtual void Delete(TEntity entityToDelete)
        {
            if(dbContext.Entry(entityToDelete).State == EntityState.Detached)
            {
                dbSet.Attach(entityToDelete);
            }
            dbSet.Remove(entityToDelete);
        }

        public virtual void Update(TEntity entityToUpdate)
        {
            dbSet.Attach(entityToUpdate);
            dbContext.Entry(entityToUpdate).State = EntityState.Modified;
        }
 
    }
    
}