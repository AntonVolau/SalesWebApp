using AutoMapper;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using SalesUpdater.Web.Data.Models;
using SalesUpdater.Web.Data.Models.Filters;
using System;
using System.Collections.Generic;
using X.PagedList;

namespace SalesUpdater.Web
{
    internal static class AutoMapper
    {
        internal static MapperConfiguration CreateConfiguration()
        {
            try
            {
                return new MapperConfiguration(config =>
                {
                    try
                    {
                        config.CreateMap<ClientViewModel, ClientDTO>().ReverseMap();
                    }
                    catch (Exception u)
                    {
                        throw;
                    }

                    config.CreateMap<ProductViewModel, ProductDTO>().ReverseMap();

                    config.CreateMap<ManagerViewModel, ManagerDTO>().ReverseMap();

                    config.CreateMap<SaleViewModel, SaleDTO>().ReverseMap();
                    try
                    {
                        config.CreateMap<ClientCoreFilterModel, ClientViewFilterModel>().ReverseMap();
                    }
                    catch (Exception z)
                    {
                        throw;
                    }
                    config.CreateMap<ProductCoreFilterModel, ProductViewFilterModel>().ReverseMap();

                    config.CreateMap<ManagerViewFilterModel, ManagerCoreFilterModel>().ReverseMap();

                    config.CreateMap<SaleViewFilterModel, SaleCoreFilterModel>().ReverseMap();
                    try
                    {
                        config
                            .CreateMap<IPagedList<ClientViewModel>, IPagedList<ClientDTO>>()
                            .ConvertUsing<CustomConverter<ClientViewModel, ClientDTO>>();
                    }
                    catch (Exception i)
                    {
                        throw;
                    }
                    config
                        .CreateMap<IPagedList<ClientDTO>, IPagedList<ClientViewModel>>()
                        .ConvertUsing<CustomConverter<ClientDTO, ClientViewModel>>();

                    config
                        .CreateMap<IPagedList<ProductViewModel>, IPagedList<ProductDTO>>()
                        .ConvertUsing<CustomConverter<ProductViewModel, ProductDTO>>();
                    config
                        .CreateMap<IPagedList<ProductDTO>, IPagedList<ProductViewModel>>()
                        .ConvertUsing<CustomConverter<ProductDTO, ProductViewModel>>();

                    config
                        .CreateMap<IPagedList<ManagerViewModel>, IPagedList<ManagerDTO>>()
                        .ConvertUsing<CustomConverter<ManagerViewModel, ManagerDTO>>();
                    config
                        .CreateMap<IPagedList<ManagerDTO>, IPagedList<ManagerViewModel>>()
                        .ConvertUsing<CustomConverter<ManagerDTO, ManagerViewModel>>();

                    config
                        .CreateMap<IPagedList<SaleViewModel>, IPagedList<SaleDTO>>()
                        .ConvertUsing<CustomConverter<SaleViewModel, SaleDTO>>();
                    config
                        .CreateMap<IPagedList<SaleDTO>, IPagedList<SaleViewModel>>()
                        .ConvertUsing<CustomConverter<SaleDTO, SaleViewModel>>();
                });
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }

    public class CustomConverter<TSource, TDestination>
        : ITypeConverter<IPagedList<TSource>, IPagedList<TDestination>>
        where TSource : class
        where TDestination : class
    {
        public IPagedList<TDestination> Convert(IPagedList<TSource> source, IPagedList<TDestination> destination,
            ResolutionContext context)
        {
            var sourceList = context.Mapper.Map<IEnumerable<TSource>, IEnumerable<TDestination>>(source);
            IPagedList<TDestination> pagedResult = new StaticPagedList<TDestination>(sourceList, source.GetMetaData());
            return pagedResult;
        }
    }
}