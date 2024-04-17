using ecommerce.Admin.Catalog.InventoryTickets.IventoryTicketItems;
using ecommerce.Admin.Catalog.Products.Attributes;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace ecommerce.Admin.Catalog.InventoryTickets
{
    public interface IInventoryTicketAppService :ICrudAppService<
        InventoryTicketDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateInventoryTicketDto,
        CreateUpdateInventoryTicketDto>
    {
        Task<PagedResultDto<InventoryTicketInListDto>> GetListFilterAsync(BaseListFilterDto input);
        Task<List<InventoryTicketInListDto>> GetListAllAsync();
        Task DeleteMultipleAsync(IEnumerable<Guid> ids);

        Task<PagedResultDto<InventoryTicketItemInListDto>> GetListItemsFilterAsync(InventoryTicketItemFilter input);
        Task<List<InventoryTicketItemInListDto>> GetListItemsAllAsync(Guid inventoryId);
        Task DeleteItemsTicketAsync(Guid inventoryTicketId, Guid id);


        Task<InventoryTicketItemInListDto> addItemAsync(CreateUpdateIventoryTicketItemDto input);

        Task<InventoryTicketItemInListDto> updateItemAsync(Guid id, CreateUpdateIventoryTicketItemDto input);

    }
}
