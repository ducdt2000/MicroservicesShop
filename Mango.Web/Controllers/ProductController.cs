using Mango.Services.ProductAPI.Models.DTOs;
using Mango.Web.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }



        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDTO> listProduct = new();

            var response = await _productService.GetAllProductAsync<ResponseDTO>();
            if(response != null && response.IsSuccess)
            {
                listProduct = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
        }
    }
}
