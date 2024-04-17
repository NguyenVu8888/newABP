using ecommerce.Admin.Catalog.InventoryTickets;
using ecommerce.Admin.Catalog.InventoryTickets.IventoryTicketItems;
using ecommerce.InventoryTickets;
using ecommerce.Products;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace ecommerce.Admin.Catalogs.IventoryTickets
{
    public class InventoryTicketAppService : CrudAppService<
        InvenoryTicket,
        InventoryTicketDto,
        Guid,
        PagedResultRequestDto,
        CreateUpdateInventoryTicketDto,
        CreateUpdateInventoryTicketDto>, IInventoryTicketAppService
    {
        private readonly IRepository<InventoryTicketItem,Guid> _inventoryTicketItemRepository;
        private readonly IRepository<Product,Guid> _productRepository;
       
        public InventoryTicketAppService(IRepository<InvenoryTicket, Guid> repository,
            IRepository<InventoryTicketItem, Guid> inventoryTicketItemRepository,
            IRepository<Product, Guid> productRepository) : base(repository)
        {
            _inventoryTicketItemRepository = inventoryTicketItemRepository;
            _productRepository = productRepository;
        }

        public async Task DeleteMultipleAsync(IEnumerable<Guid> ids)
        {
            await Repository.DeleteManyAsync(ids);
            await UnitOfWorkManager.Current.SaveChangesAsync();

        }

        public async Task DeleteItemsTicketAsync(Guid inventoryTicketId, Guid id)
        {
            var inventoryTicket = await Repository.GetAsync(x => x.Id == inventoryTicketId);
            if (inventoryTicket is null)
            {
                throw new BusinessException(ecommerceDomainErrorCodes.InventoryTicketIsNotExists);
            }

            var item = await _inventoryTicketItemRepository.GetAsync(x => x.Id == id);
            if (item is null)
            {
                throw new BusinessException(ecommerceDomainErrorCodes.InventoryTicketItemsIsNotExists);
            }

            await _inventoryTicketItemRepository.DeleteAsync(id);
            await UnitOfWorkManager.Current.SaveChangesAsync();
        }

        public async Task<List<InventoryTicketInListDto>> GetListAllAsync()
        {
            var query = await Repository.GetQueryableAsync();
            /*  query = query.Where(x => x.IsActive == true);*/

            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<InvenoryTicket>, List<InventoryTicketInListDto>>(data);
        }

        public async Task<PagedResultDto<InventoryTicketInListDto>> GetListFilterAsync(BaseListFilterDto input)
        {
            var query = await Repository.GetQueryableAsync();
            query = query.WhereIf(!string.IsNullOrWhiteSpace(input.keyword), p => p.Code.Contains(input.keyword));
          

            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter.ToListAsync(query.OrderByDescending(x => x.CreationTime).Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<InventoryTicketInListDto>(totalCount, ObjectMapper.Map<List<InvenoryTicket>, List<InventoryTicketInListDto>>(data));
        }

        public async Task<List<InventoryTicketItemInListDto>> GetListItemsAllAsync(Guid inventoryId)
        {
            var query = await _inventoryTicketItemRepository.GetQueryableAsync();
            query = query.Where(x=>x.InventoryTicketId.Equals(inventoryId));
      
            var data = await AsyncExecuter.ToListAsync(query);

            return ObjectMapper.Map<List<InventoryTicketItem>, List<InventoryTicketItemInListDto>>(data);
        }

        public async Task<PagedResultDto<InventoryTicketItemInListDto>> GetListItemsFilterAsync(InventoryTicketItemFilter input)
        {
            var query = await _inventoryTicketItemRepository.GetQueryableAsync();
            query = query.WhereIf(!input.InventoryTicketId.HasValue, p => p.InventoryTicketId == input.InventoryTicketId);


            var totalCount = await AsyncExecuter.LongCountAsync(query);
            var data = await AsyncExecuter.ToListAsync(query.OrderByDescending(x => x.ExpiredDate).Skip(input.SkipCount).Take(input.MaxResultCount));

            return new PagedResultDto<InventoryTicketItemInListDto>(totalCount, ObjectMapper.Map<List<InventoryTicketItem>, List<InventoryTicketItemInListDto>>(data));
        }

        public async Task<InventoryTicketItemInListDto> addItemAsync(CreateUpdateIventoryTicketItemDto input)
        {
            var product = await _productRepository.GetAsync(x => x.Id == input.ProductId);
            if (product is null)
            {
                throw new BusinessException(ecommerceDomainErrorCodes.ProductIsNotExists);
            }

            var inventoryTicket = await Repository.GetAsync(x => x.Id == input.InventoryTicketId);
            if (inventoryTicket is null)
            {
                throw new BusinessException(ecommerceDomainErrorCodes.InventoryTicketIsNotExists);
            }

            var newInventoryTicketItemId = Guid.NewGuid();

            var newItem = new InventoryTicketItem(newInventoryTicketItemId, input.InventoryTicketId, input.ProductId, input.SKU, input.Quantity, input.BatchNumber, input.ExpiredDate);
            await _inventoryTicketItemRepository.InsertAsync(newItem);
            try
            {
                await UnitOfWorkManager.Current.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return new InventoryTicketItemInListDto { 
                InventoryTicketId = input.InventoryTicketId,
                ProductId = input.ProductId,
                SKU = input.SKU,
                Quantity = input.Quantity,
                BatchNumber = input.BatchNumber,
                ExpiredDate = input.ExpiredDate,

            };


        }

        public async Task<InventoryTicketItemInListDto> updateItemAsync(Guid id, CreateUpdateIventoryTicketItemDto input)
        {
            var product = await _productRepository.GetAsync(x => x.Id == input.ProductId);
            if (product is null)
            {
                throw new BusinessException(ecommerceDomainErrorCodes.ProductIsNotExists);
            }

            var inventoryTicket = await Repository.GetAsync(x => x.Id == input.InventoryTicketId);
            if (inventoryTicket is null)
            {
                throw new BusinessException(ecommerceDomainErrorCodes.InventoryTicketIsNotExists);
            }

            var TicketItem = await _inventoryTicketItemRepository.GetAsync(x =>x.Id == id);
            if (TicketItem is null)
            {
                throw new BusinessException("Items Ticket Not Exsit");

            }



            TicketItem.InventoryTicketId = input.InventoryTicketId;
            TicketItem.ProductId = input.ProductId;
            TicketItem.SKU = input.SKU;
            TicketItem.Quantity = input.Quantity;
            TicketItem.BatchNumber = input.BatchNumber;
            TicketItem.ExpiredDate = input.ExpiredDate;


            await _inventoryTicketItemRepository.UpdateAsync(TicketItem);
            try
            {
                await UnitOfWorkManager.Current.SaveChangesAsync();

            }
            catch (Exception ex)
            {

                throw ex;
            }

            return new InventoryTicketItemInListDto
            {
                InventoryTicketId = input.InventoryTicketId,
                ProductId = input.ProductId,
                SKU = input.SKU,
                Quantity = input.Quantity,
                BatchNumber = input.BatchNumber,
                ExpiredDate = input.ExpiredDate,

            };
        }
    }
}
