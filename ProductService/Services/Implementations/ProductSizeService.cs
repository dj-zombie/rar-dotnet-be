using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;
using ProductService.Data;
using ProductService.Dtos.Models;
using ProductService.Dtos.Requests;
using ProductService.Services.Interfaces;
using ProductService.Models;


public class ProductSizeService : IProductSizeService
{
    private readonly AppDbContext _context;
    private readonly IMapper _mapper;

    public ProductSizeService(AppDbContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    public async Task<IEnumerable<ProductSizeDto>> GetAllAsync()
    {
        return await _context.ProductSizes
            .AsNoTracking()
            .ProjectTo<ProductSizeDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
    }

    public async Task<ProductSizeDto> GetByIdAsync(int id)
    {
        var productSize = await _context.ProductSizes
            .AsNoTracking()
            .FirstOrDefaultAsync(ps => ps.Id == id);

        if (productSize == null)
        {
            throw new KeyNotFoundException($"ProductSize with ID {id} not found.");
        }

        return _mapper.Map<ProductSizeDto>(productSize);
    }

    public async Task<ProductSizeDto> CreateAsync(CreateProductSizeRequest request)
    {
        // var productSize = _mapper.Map<ProductSize>(request);
        var productSize = new ProductSize
        {
            SizeName = request.SizeName
        };

        _context.ProductSizes.Add(productSize);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductSizeDto>(productSize);
    }

    public async Task<ProductSizeDto> UpdateAsync(int id, UpdateProductSizeRequest request)
    {
        var productSize = await _context.ProductSizes.FindAsync(id);
        if (productSize == null)
        {
            throw new KeyNotFoundException($"ProductSize with ID {id} not found.");
        }

        // Map the incoming request data onto the existing entity
        _mapper.Map(request, productSize);
        await _context.SaveChangesAsync();

        return _mapper.Map<ProductSizeDto>(productSize);
    }

    public async Task DeleteAsync(int id)
    {
        var productSize = await _context.ProductSizes.FindAsync(id);
        if (productSize == null)
        {
            // You can choose to throw or just return silently
            throw new KeyNotFoundException($"ProductSize with ID {id} not found.");
        }

        _context.ProductSizes.Remove(productSize);
        await _context.SaveChangesAsync();
    }
}