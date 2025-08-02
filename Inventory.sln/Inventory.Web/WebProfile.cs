using AutoMapper;
using Inventory.Application.Features.BalanceTransfers.Commands;
using Inventory.Application.Features.Categories.Commands;
using Inventory.Application.Features.Customers.Commands;
using Inventory.Application.Features.Products.Commands;
using Inventory.Application.Features.Sales.Commands;
using Inventory.Application.Features.Units.Commands;
using Inventory.Domain.Dtos;
using Inventory.Domain.Entities;
using Inventory.Domain.ViewModel;
using Inventory.Web.Areas.Admin.Models;
using Inventory.Web.Areas.Admin.Models.Sales;

namespace Inventory.Web
{
    public class WebProfile : Profile
    {
        public WebProfile()
        {
            CreateMap<Product, AddProductModel>().ReverseMap();
            CreateMap<Product, ProductAddCommand>().ReverseMap();
            CreateMap<Product, ProductUpdateCommand>().ReverseMap();
            CreateMap<UpdateProductModel, Product>().ReverseMap();
            CreateMap<ProductSearchModel, ProductSearchDto>().ReverseMap();


            CreateMap<Unit, UpdateUnitModel>().ReverseMap();
            CreateMap<Unit, UnitAddCommand>().ReverseMap();
            CreateMap<Unit, UnitUpdateCommand>().ReverseMap();

            CreateMap<Category, UpdateCategoryModel>().ReverseMap();
            CreateMap<Category, CategoryAddCommand>().ReverseMap();
            CreateMap<Category, CategoryUpdateCommand>().ReverseMap();


            CreateMap<Customer, CustomerAddCommand>().ReverseMap();
            CreateMap<Customer, CustomerUpdateModel>().ReverseMap();
            CreateMap<Customer, CustomerUpdateCommand>().ReverseMap();
            CreateMap<Customer, CustomerView>().ReverseMap();
            CreateMap<CustomerAddModel, CustomerAddCommand>().ReverseMap();

            CreateMap<Sale , SaleAddCommand>().ReverseMap();
            CreateMap<Sale , SalesUpdateModel>().ReverseMap();
            CreateMap<Sale , SaleUpdateCommand>().ReverseMap();
            CreateMap<SaleItemAddModel, SaleItems>().ReverseMap();
            CreateMap<SaleItemAddDto, SaleItems>().ReverseMap();
            CreateMap<SaleItemsViewModel, SaleItems>().ReverseMap();

            CreateMap<BalanceTransfer, BalanceTransferAddCommand>().ReverseMap();
        }
    }
}
