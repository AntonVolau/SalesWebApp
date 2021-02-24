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
                        config.CreateMap<ClientFilterCoreModel, ClientFilterViewModel>().ReverseMap();
                    }
                    catch (Exception z)
                    {
                        throw;
                    }
                    config.CreateMap<ProductFilterCoreModel, ProductFilterViewModel>().ReverseMap();

                    config.CreateMap<ManagerFilterViewModel, ManagerFilterCoreModel>().ReverseMap();

                    config.CreateMap<SaleFilterViewModel, SaleFilterCoreModel>().ReverseMap();
                    try
                    {
                        config
                            .CreateMap<IPagedList<ClientViewModel>, IPagedList<ClientDTO>>()
                            .ConvertUsing<PagedListConverter<ClientViewModel, ClientDTO>>();
                    }
                    catch (Exception i)
                    {
                        throw;
                    }
                    config
                        .CreateMap<IPagedList<ClientDTO>, IPagedList<ClientViewModel>>()
                        .ConvertUsing<PagedListConverter<ClientDTO, ClientViewModel>>();

                    config
                        .CreateMap<IPagedList<ProductViewModel>, IPagedList<ProductDTO>>()
                        .ConvertUsing<PagedListConverter<ProductViewModel, ProductDTO>>();
                    config
                        .CreateMap<IPagedList<ProductDTO>, IPagedList<ProductViewModel>>()
                        .ConvertUsing<PagedListConverter<ProductDTO, ProductViewModel>>();

                    config
                        .CreateMap<IPagedList<ManagerViewModel>, IPagedList<ManagerDTO>>()
                        .ConvertUsing<PagedListConverter<ManagerViewModel, ManagerDTO>>();
                    config
                        .CreateMap<IPagedList<ManagerDTO>, IPagedList<ManagerViewModel>>()
                        .ConvertUsing<PagedListConverter<ManagerDTO, ManagerViewModel>>();

                    config
                        .CreateMap<IPagedList<SaleViewModel>, IPagedList<SaleDTO>>()
                        .ConvertUsing<PagedListConverter<SaleViewModel, SaleDTO>>();
                    config
                        .CreateMap<IPagedList<SaleDTO>, IPagedList<SaleViewModel>>()
                        .ConvertUsing<PagedListConverter<SaleDTO, SaleViewModel>>();
                });
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }

    public class PagedListConverter<TSource, TDestination>
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