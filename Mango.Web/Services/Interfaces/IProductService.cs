﻿using Mango.Web.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Web.Services.Interfaces
{
    public interface IProductService : IBaseService
    {
        Task<T> GetAllProductAsync<T>(string token);
        Task<T> GetProductByIdAsync<T>(int id, string token);
        Task<T> CreateProductAsync<T>(ProductDTO productDTO, string token);
        Task<T> UpdateProductAsync<T>(ProductDTO productDTO, string token);
        Task<T> DeleteProductAsync<T>(int id, string token);
    }
}