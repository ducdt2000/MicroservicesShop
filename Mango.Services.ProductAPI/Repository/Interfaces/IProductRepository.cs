using Mango.Services.ProductAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.ProductAPI.Repository.Interfaces
{
    public interface IProductRepository
    {
        Task<IEnumerable<ProductDTO>> GetProducts();
        Task<ProductDTO> GetProductById(int id);
        Task<ProductDTO> CreateUpdateProduct(ProductDTO productDTO);
        Task<bool> DeleteProduct(int id);
    }
}
