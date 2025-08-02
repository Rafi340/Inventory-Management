using Inventory.Domain.Entities.ViewModel;
using Inventory.Domain.ListModel.Product;
using MediatR;
namespace Inventory.Application.Features.Products.Queries
{
    public class GetProductSpQuery : ProductListModel , IRequest<(IList<ProductViewModel> data, int total, int totalDisplay)> , IProductListModel
    {
    }
}
