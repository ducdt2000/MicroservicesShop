using Mango.Web.Models;
using Mango.Web.Models.DTOs;
using Mango.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IProductService _productService;
        private readonly IShoppingCartService _cartService;

        public HomeController(ILogger<HomeController> logger, IProductService productService, IShoppingCartService cartService)
        {
            _logger = logger;
            _productService = productService;
            _cartService = cartService;
        }

        [Authorize]
        public async Task<IActionResult> Details(int id)
        {
            ProductDTO model = new();
            var response = await _productService.GetProductByIdAsync<ResponseDTO>(id, "");
            if (response != null && response.IsSuccess)
            {
                model = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
            }
            return View(model);
        }

        [HttpPost]
        [ActionName("Details")]
        [Authorize]
        public async Task<IActionResult> DetailsPost(ProductDTO productDTO)
        {
            CartDTO cartDTO = new()
            {
                CartHeader = new CartHeaderDTO
                {
                    UserId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value
                }
            };

            CartDetailsDTO cartDetailsDTO = new CartDetailsDTO()
            {
                Count = productDTO.Count,
                ProductId = productDTO.Id
            };

            var response = await _productService.GetProductByIdAsync<ResponseDTO>(productDTO.Id, "");
            if(response!=null && response.IsSuccess)
            {
                cartDetailsDTO.Product = JsonConvert.DeserializeObject<ProductDTO>(Convert.ToString(response.Result));
            }

            List<CartDetailsDTO> cartDetailsDTOs = new();
            cartDetailsDTOs.Add(cartDetailsDTO);
            cartDTO.CartDetails = cartDetailsDTOs;

            var token = await HttpContext.GetTokenAsync("access_token");
            var addToCartResponse = await _cartService.AddToCartAsync<ResponseDTO>(cartDTO, token);
            
            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Index));
            }


            return View(productDTO);
        }

        public async Task<IActionResult> Index()
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            List<ProductDTO> list = new();
            var response = await _productService.GetAllProductAsync<ResponseDTO>("");
            if (response != null && response.IsSuccess)
            {
                list = JsonConvert.DeserializeObject<List<ProductDTO>>(Convert.ToString(response.Result));
            }
            return View(list);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [Authorize]
        public async Task<IActionResult> LoginAsync()
        {
            //var token = await HttpContext.GetTokenAsync("access_token");


            return RedirectToAction(nameof(Index));
        }


        public IActionResult Logout()
        {
            return SignOut("Cookies", "oidc");
        }
    }
}
