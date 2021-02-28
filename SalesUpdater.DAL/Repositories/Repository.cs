using AutoMapper;
using SalesUpdater.Entity;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Interfaces.DAL.Repositories;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using X.PagedList;

namespace SalesUpdater.DAL.Repositories
{
    public abstract class Repository<TDTO, TEntity> : IRepository<TDTO>
        where TDTO : CoreModel where TEntity : class
    {
        protected readonly SalesContext _context;

        protected readonly IDbSet<TEntity> DbSet;

        protected readonly IMapper _mapper;

        protected Repository(SalesContext context, IMapper mapper)
        {
            _context = context;
            DbSet = _context.Set<TEntity>();
            _mapper = mapper;
        }

        protected virtual TEntity DTOtoEntity(TDTO dto)
        {
            return _mapper.Map<TEntity>(dto);
        }

        public void Add(params TDTO[] models)
        {
            foreach (var model in models)
            {
                var entity = DTOtoEntity(model);
                DbSet.Add(entity);
                _context.Entry(entity).State = EntityState.Added;
            }
        }

        public TDTO Add(TDTO model)
        {
            var entity = DTOtoEntity(model);

            var result = DbSet.Add(entity);
            _context.Entry(entity).State = EntityState.Added;

            return _mapper.Map<TDTO>(result);
        }

        public void Remove(params TDTO[] models)
        {
            foreach(var model in models)
            {
                var entity = DTOtoEntity(model);
                if (_context.Entry(entity).State == EntityState.Detached)
                {
                    DbSet.Attach(entity);
                }
                DbSet.Remove(entity);
                _context.Entry(entity).State = EntityState.Deleted;
            }
        }

        public void Update(params TDTO[] entities)
        {
            foreach (var model in entities)
            {
                var x = DbSet.Find(model.ID);

                _mapper.Map(model, x);

                //  var entity = DTOtoEntity(model);
                //  DbSet.Attach(entity);
                //  _context.Entry(entity).State = EntityState.Modified;
            }
        }

        public TDTO Update(TDTO model)
        {
           // var entity = DTOtoEntity(model);

            var x = DbSet.Find(model.ID);

            var y = _mapper.Map(model, x);

            // var result = DbSet.Attach(entity);
            // _context.Entry(entity).State = EntityState.Modified;

            return _mapper.Map<TDTO>(model);
        }

        public TDTO Get(int ID)
        {
            return _mapper.Map<TDTO>(DbSet.Find(ID));
        }

        public IEnumerable<TDTO> GetAll()
        {
            return _mapper.Map<IEnumerable<TDTO>>(DbSet.AsNoTracking());
        }

        public IEnumerable<TDTO> Find(Expression<Func<TDTO, bool>> predicate)
        {
            var newPredicate = predicate.GetPredicate<TDTO, TEntity>();

            return _mapper.Map<IEnumerable<TDTO>>(DbSet.AsNoTracking().Where(newPredicate));
        }

        public void Save()
        {
            _context.SaveChanges();
        }

        public async Task DeleteAsync(int id)
        {
            var entity = _mapper.Map<TEntity>(await GetAsync(id).ConfigureAwait(false));

            if (_context.Entry(entity).State == EntityState.Detached)
            {
                try
                {
                    DbSet.Add(entity);

                    var z = _context.Entry(entity).State;
                }
                catch (Exception x)
                {
                    throw;
                }
            }
            try
            {
                DbSet.Remove(entity);
                var kek = _context.Entry(entity).State;
             //   var local = DbSet.Local;
             //   if (local != null && local.Count != 0)
             //   {
             //       local.Clear();
             //       var newlocal = DbSet.Local;
             //       _context.Entry(entity).State = EntityState.Deleted;
             //   }
             //   else
             //   {
             //   _context.Entry(entity).State = EntityState.Deleted;
             //   }
                 _context.Entry(entity).State = EntityState.Deleted;
            }
            catch (Exception e)
            {
                throw;
            }
        }

        public async Task<TDTO> GetAsync(int id)
        {
            Expression<Func<TDTO, bool>> predicate = x => x.ID == id;

            var newPredicate = predicate.GetPredicate<TDTO, TEntity>();

            var result = await DbSet.AsNoTracking().FirstOrDefaultAsync(newPredicate).ConfigureAwait(false);

            return _mapper.Map<TDTO>(result);
        }

        public async Task<IPagedList<TDTO>> GetPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<TDTO, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending)
        {
            IPagedList<TEntity> result;
            if (predicate != null)
            {
                var newPredicate = predicate.GetPredicate<TDTO, TEntity>();

                result = await DbSet
                    .AsNoTracking()
                    .Where(newPredicate)
                    .OrderBy("ID", sortDirection)
                    .ToPagedListAsync(pageNumber, pageSize)
                    .ConfigureAwait(false);
            }
            else
            {
                result = await DbSet
                    .AsNoTracking()
                    .OrderBy("ID", sortDirection)
                    .ToPagedListAsync(pageNumber, pageSize)
                    .ConfigureAwait(false);
            }

            return _mapper.Map<IPagedList<TDTO>>(result);
        }

        public async Task<IEnumerable<TDTO>> FindAsync(Expression<Func<TDTO, bool>> predicate)
        {
            var newPredicate = predicate.GetPredicate<TDTO, TEntity>();

            return _mapper.Map<IEnumerable<TDTO>>(await DbSet.AsNoTracking().Where(newPredicate).ToListAsync()
                .ConfigureAwait(false));
        }

        public async Task<int?> SaveAsync()
        {
            return await _context.SaveChangesAsync().ConfigureAwait(false);
        }
    }
}
