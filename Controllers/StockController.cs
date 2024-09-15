using api.Data;
using api.Dtos.Stock;
using api.Mappers;
using Microsoft.AspNetCore.Mvc;

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
    public IActionResult GetAll(){
        var stocks = _context.Stock.ToList()
        .Select(s => s.ToStockDto()); // select: return an immutable list

        return Ok(stocks);
    }

    [HttpGet("{id}")]
    public IActionResult GetById([FromRoute] int id){
        var stock = _context.Stock.Find(id);

        if (stock == null){
            return NotFound();
        }

        return Ok(stock.ToStockDto());
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateStockRequestDto stockDto){
        var stockModel = stockDto.ToStockFromCreateDto();
        _context.Stock.Add(stockModel);
        _context.SaveChanges();

        return CreatedAtAction(nameof(GetById), new { id = stockModel.Id }, stockModel.ToStockDto());
    }

    [HttpPut]
    [Route("{id}")]
    public IActionResult Update([FromRoute] int id, [FromBody] UpdateStockRequestDto stockDto){
        var stockModel = _context.Stock.FirstOrDefault(x => x.Id == id);

        if (stockModel == null){
            return NotFound();
        }

        stockModel.Symbol = stockDto.Symbol;
        stockModel.CompanyName = stockDto.CompanyName;
        stockModel.Purchase = stockDto.Purchase;
        stockModel.LastDiv = stockDto.LastDiv;
        stockModel.Industry = stockDto.Industry;
        stockModel.MarketCap = stockDto.MarketCap;

        _context.SaveChanges();

        return Ok(stockModel.ToStockDto());
    }

    [HttpDelete]
    [Route("{id}")]
    public IActionResult Delete([FromRoute] int id){
        var stockModel = _context.Stock.FirstOrDefault(x => x.Id == id);

        if (stockModel == null){
            return NotFound();
        }

        _context.Stock.Remove(stockModel);
        _context.SaveChanges();

        return NoContent();
    }
}
