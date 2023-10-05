using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Play.Catalog.Service.DTOs;
using Play.Catalog.Service.ExtentionMethods;
using Play.Catalog.Service.Repositories;

namespace Play.Catalog.Service.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class ItemsController : ControllerBase
    {

        private readonly ItemsRepository _itemsRepository;
        // private static readonly List<ItemDto> items = new()
        // {
        //     new ItemDto(Guid.NewGuid(), "Elixir", "Increses Endurance(10 secs)", 12, DateTimeOffset.Now),
        //     new ItemDto(Guid.NewGuid(), "Magic Potion", "Invisibility(5 secs)", 24, DateTimeOffset.Now),
        //     new ItemDto(Guid.NewGuid(), "Speed Runner", "Achieve Max Speed(15 secs)", 18, DateTimeOffset.Now)
        // };

        public ItemsController(ItemsRepository items)
        {
            _itemsRepository = items;
        }

        [HttpGet]
        public async Task<IActionResult> GetAsync()
        {
            var items = (await _itemsRepository.GetAllAsync())
                            .Select(item => item.AsDto());
            return Ok(items);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult> GetByIdAsync(Guid id)
        {
            var item = await _itemsRepository.GetAsync(id);
            if(item == null) { return NotFound(); }

            return Ok(item.AsDto());
        }

        [HttpPost]
        public async Task<ActionResult> PostAsync(CreateItemDto paramItem)
        {
            var item = new ItemDto(Guid.NewGuid(), paramItem.Name, paramItem.Description, paramItem.Price, DateTimeOffset.Now);
            await _itemsRepository.CreateAsync(item.AsEntity());

            return CreatedAtAction(nameof(GetByIdAsync), item, new { id = item.Id });
        }

        [HttpPut]
        public async Task<ActionResult> PutAsync(Guid id, ItemDto paramItem)
        {
            await _itemsRepository.UpdateAsync(id, paramItem.AsEntity());
            return NoContent();
        }

        [HttpDelete]
        public async Task<ActionResult> DeleteAsync(Guid id) 
        {
            await _itemsRepository.RemoveAsync(id);
            return NoContent();       
        }
    }
}