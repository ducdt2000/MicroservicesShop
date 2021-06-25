using Mango.Web.Models.DTOs;
using Mango.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
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
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _productService.GetAllProductAsync<ResponseDTO>(token);
            if(response != null && response.IsSuccess)
            {
                listProduct = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }

            return View(listProduct);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductCreate(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                var res = await _productService.CreateProductAsync<ResponseDTO>(model, token);
                if (res != null && res.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ProductEdit(int id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var res = await _productService.GetProductByIdAsync<ResponseDTO>(id, token);
            if (res != null && res.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(res.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductEdit(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                var res = await _productService.UpdateProductAsync<ResponseDTO>(model, token);
                if (res != null && res.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }

        [Authorize(Roles="Admin")]
        public async Task<IActionResult> ProductDelete(int id)
        {
            var token = await HttpContext.GetTokenAsync("access_token");
            var res = await _productService.GetProductByIdAsync<ResponseDTO>(id, token);
            if (res != null && res.IsSuccess)
            {
                var model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(res.Result));
                return View(model);
            }
            return NotFound();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ProductDelete(ProductDTO model)
        {
            if (ModelState.IsValid)
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                var res = await _productService.DeleteProductAsync<ResponseDTO>(model.Id, token);
                if (res.IsSuccess)
                {
                    return RedirectToAction(nameof(ProductIndex));
                }
            }
            return View(model);
        }
    }
}
