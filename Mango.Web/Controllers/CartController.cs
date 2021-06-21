using Mango.Web.Models;
using Mango.Web.Models.DTOs;
using Mango.Web.Services.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private readonly IShoppingCartService _cartService;
        private readonly ICouponService _couponService;
        public CartController(IProductService productService, IShoppingCartService cartService, ICouponService couponService)
        {
            _productService = productService;
            _cartService = cartService;
            _couponService = couponService;
        }

        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartDTOBaseOnLoggerdInUser());
        }

        [HttpPost]
        [ActionName("ApplyCoupon")]
        public async Task<IActionResult> ApplyCoupon(CartDTO cartDTO)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.ApplyCoupon<ResponseDTO>(cartDTO, token);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        [HttpPost]
        [ActionName("RemoveCoupon")]
        public async Task<IActionResult> RemoveCoupon(CartDTO cartDTO)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveCoupon<ResponseDTO>(cartDTO.CartHeader.UserId, token);

            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }
            return View();
        }

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.RemoveFromCartAsync<ResponseDTO>(cartDetailsId, token);


            if (response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(CartIndex));
            }

            return View();
        }

        [HttpGet]
        public async Task<IActionResult> Checkout()
        {
            return View(await LoadCartDTOBaseOnLoggerdInUser());
        }

        [HttpPost]
        public async Task<IActionResult> Checkout(CartDTO cartDTO)
        {
            try
            {
                var token = await HttpContext.GetTokenAsync("access_token");
                var response = await _cartService.Checkout<ResponseDTO>(cartDTO.CartHeader, token);

                if (!response.IsSuccess)
                {
                    //ViewBag.Error = response.DisplayMessage;
                    TempData["Error"] = response.DisplayMessage;
                    return RedirectToAction(nameof(Checkout));
                }

                return RedirectToAction(nameof(Confirmation));
            }
            catch(Exception e)
            {
                return View(cartDTO);
            }

        }

        public async Task<IActionResult> Confirmation()
        {
            return View();
        }

        private async Task<CartDTO> LoadCartDTOBaseOnLoggerdInUser()
        {
            var userId = User.Claims.Where(u => u.Type == "sub")?.FirstOrDefault()?.Value;
            var token = await HttpContext.GetTokenAsync("access_token");
            var response = await _cartService.GetCartByUserIdAsync<ResponseDTO>(userId, token);

            CartDTO cartDTO = new();
            if (response != null && response.IsSuccess)
            {
                cartDTO = JsonConvert.DeserializeObject<CartDTO>(Convert.ToString(response.Result));
            }

            if(cartDTO.CartHeader!= null)
            {
                if (!string.IsNullOrEmpty(cartDTO.CartHeader.CouponCode))
                {
                    var coupon = await _couponService.GetCoupon<ResponseDTO>(cartDTO.CartHeader.CouponCode, token);
                    if (coupon != null && coupon.IsSuccess)
                    {
                        var couponObj = JsonConvert.DeserializeObject<CouponDTO>(Convert.ToString(coupon.Result));
                        cartDTO.CartHeader.DiscountTotal = couponObj.DiscountAmount;
                    }
                }

                foreach(var detail in cartDTO.CartDetails)
                {
                    cartDTO.CartHeader.OrderTotal += detail.Product.Price * detail.Count;
                }

                cartDTO.CartHeader.OrderTotal -= cartDTO.CartHeader.DiscountTotal;
            }
            return cartDTO;
        }
    }
}
