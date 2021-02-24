using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Web.Data.Models.Filters;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using System.Web.Helpers;
using X.PagedList;

namespace SalesUpdater.Web.Data.Contracts.Services
{
    public interface IService<TDTO, in TFilterCoreModel>
        where TDTO : CoreModel
        where TFilterCoreModel : PagedListParameterCoreModel
    {
        Task<IPagedList<TDTO>> GetUsingPagedListAsync(int pageNumber, int pageSize,
            Expression<Func<TDTO, bool>> predicate = null, SortDirection sortDirection = SortDirection.Ascending);

        Task<IPagedList<TDTO>> Filter(TFilterCoreModel filterCoreModel, int pageSize,
            SortDirection sortDirection = SortDirection.Ascending);

        Task<TDTO> GetAsync(int id);

        Task<TDTO> AddAsync(TDTO model);

        Task<TDTO> UpdateAsync(TDTO model);

        Task DeleteAsync(int id);

        Task<IEnumerable<TDTO>> FindAsync(Expression<Func<TDTO, bool>> predicate);
    }
}