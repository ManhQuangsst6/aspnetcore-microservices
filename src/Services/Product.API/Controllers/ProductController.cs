using Contracts.Common.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Product.API.Entities;
using Product.API.Persistence;

namespace Product.API.Controllers;
[ApiController]
[Route("api/[controller]")]
public class ProductController:ControllerBase
{
    private readonly IRepositoryBaseAsync<CatalogProduct,long,ProductContext> _repository;

    public ProductController(IRepositoryBaseAsync<CatalogProduct, long, ProductContext> repository)
    {
        _repository = repository;
    }

    [HttpGet]
    public async Task<IActionResult> GetProductAsync()
    {
        var result = await _repository.GetAll().ToListAsync();
        return Ok(result);
    }
}