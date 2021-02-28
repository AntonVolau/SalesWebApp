using AutoMapper;
using SalesUpdater.Entity;
using SalesUpdater.Interfaces.Core.DataTransferObject;
using System;
using System.Collections.Generic;
using X.PagedList;

namespace SalesUpdater.DAL
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
                        config.CreateMap<Clients, ClientDTO>().ReverseMap();
                    }
                    catch(Exception e)
                    {
                        throw;
                    }

                    config.CreateMap<Products, ProductDTO>().ReverseMap();

                    config.CreateMap<Managers, ManagerDTO>().ReverseMap();

                        config.CreateMap<Sales, SaleDTO>();
                        config.CreateMap<SaleDTO, Sales>()
                            .ForMember(x => x.Clients, opt => opt.Ignore())
                            .ForMember(x => x.Managers, opt => opt.Ignore())
                            .ForMember(x => x.Products, opt => opt.Ignore());

                    config
                           .CreateMap<IPagedList<Clients>, IPagedList<ClientDTO>>()
                           .ConvertUsing<CustomConverter<Clients, ClientDTO>>();
                    config
                        .CreateMap<IPagedList<ClientDTO>, IPagedList<Clients>>()
                        .ConvertUsing<CustomConverter<ClientDTO, Clients>>();

                    config
                        .CreateMap<IPagedList<Products>, IPagedList<ProductDTO>>()
                        .ConvertUsing<CustomConverter<Products, ProductDTO>>();
                    config
                        .CreateMap<IPagedList<ProductDTO>, IPagedList<Products>>()
                        .ConvertUsing<CustomConverter<ProductDTO, Products>>();

                    config
                        .CreateMap<IPagedList<Managers>, IPagedList<ManagerDTO>>()
                        .ConvertUsing<CustomConverter<Managers, ManagerDTO>>();
                    config
                        .CreateMap<IPagedList<ManagerDTO>, IPagedList<Managers>>()
                        .ConvertUsing<CustomConverter<ManagerDTO, Managers>>();

                    config
                        .CreateMap<IPagedList<Sales>, IPagedList<SaleDTO>>()
                        .ConvertUsing<CustomConverter<Sales, SaleDTO>>();
                    config
                        .CreateMap<IPagedList<SaleDTO>, IPagedList<Sales>>()
                        .ConvertUsing<CustomConverter<SaleDTO, Sales>>();


                });
            }
            catch (Exception e)
            {
                throw;
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
}
