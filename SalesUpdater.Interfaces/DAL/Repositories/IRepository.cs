using SalesUpdater.Interfaces.Core.DataTransferObject;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using X.PagedList;
using System.Web.Helpers;

namespace SalesUpdater.Interfaces.DAL.Repositories
{
    public interface IRepository<TDTO> where TDTO : CoreModel
    {
        void Add(params TDTO[] models);

        TDTO Add(TDTO model);

        void Remove(params TDTO[] models);

        void Update(params TDTO[] models);

        TDTO Update(TDTO model);

        TDTO Get(int ID);

        IEnumerable<TDTO> GetAll();

        IEnumerable<TDTO> Find(Expression<Func<TDTO, bool>> predicate);

        void Save();

        Task DeleteAsync(int id);

        Task<TDTO> GetAsync(int id);

        Task<IPagedList<TDTO>> GetPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<TDTO, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending);

        Task<IEnumerable<TDTO>> FindAsync(Expression<Func<TDTO, bool>> predicate);

        Task<int?> SaveAsync();
    }
}
