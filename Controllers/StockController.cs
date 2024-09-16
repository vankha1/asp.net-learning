using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace api.Controllers;

[Route("api/stock")]
[ApiController]
public class StockController : ControllerBase
{
    private readonly ApplicationDBContext _context;

    public StockController(ApplicationDBContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var stocks = await _context.Stock.ToListAsync();
        var stockDto = stocks.Select(s => s.ToStockDto()); // select: return an immutable list

        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetById([FromRoute] int id)
    {
        var stock = await _context.Stock.FindAsync(id);

        if (stock == null)
        {
            return NotFound();
        }

        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateStockRequestDto stockDto)
    {
        var stockModel = stockDto.ToStockFromCreateDto();
        await _context.Stock.AddAsync(stockModel);
        await _context.SaveChangesAsync();

        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut]
    [Route("{id}")]
    public async Task<IActionResult> Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto)
    {
        var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }

        stockModel.Symbol = stockDto.Symbol;
        stockModel.CompanyName = stockDto.CompanyName;
        stockModel.Purchase = stockDto.Purchase;
        stockModel.LastDiv = stockDto.LastDiv;
        stockModel.Industry = stockDto.Industry;
        stockModel.MarketCap = stockDto.MarketCap;

        await _context.SaveChangesAsync();

        return Ok(stockModel.ToStockDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public async Task<IActionResult> Delete([FromRoute] int id)
    {
        var stockModel = await _context.Stock.FirstOrDefaultAsync(x => x.Id == id);

        if (stockModel == null)
        {
            return NotFound();
        }

        _context.Stock.Remove(stockModel); // Remove is not async function
        await _context.SaveChangesAsync();

        return NoContent();
    }
}
