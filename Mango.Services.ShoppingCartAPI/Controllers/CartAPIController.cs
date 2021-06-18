using Mango.MessageBus;
using Mango.Services.ShoppingCartAPI.Models.DTOs;
using Mango.Services.ShoppingCartAPI.Models.Messages;
using Mango.Services.ShoppingCartAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.ShoppingCartAPI.Controllers
{
    [ApiController]
    [Route("api/cart")]
    public class CartAPIController : Controller
    {
        private readonly ICartRepository _cartRepository;
        private readonly ICouponRepository _couponRepository;
        protected ResponseDTO _response;
        private readonly IMessageBus _messageBus;
        

        public CartAPIController(ICartRepository cartRepository, IMessageBus messageBus, ICouponRepository couponRepository)
        {
            _cartRepository = cartRepository;
            _response = new ResponseDTO();
            _messageBus = messageBus;
            _couponRepository = couponRepository;
        }

        [HttpGet("GetCart/{userId}")]
        public async Task<object> GetCart(string userId)
        {
            try
            {
                CartDTO cartDTO = await _cartRepository.GetCartByUserId(userId);
                _response.Result = cartDTO;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessge = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpPost("AddCart")]
        public async Task<object> AddCart(CartDTO cartDTO)
        {
            try
            {
                CartDTO newCartDTO = await _cartRepository.CreateUpdateCart(cartDTO);
                _response.Result = newCartDTO;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessge = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpPost("UpdateCart")]
        public async Task<object> UpdateCart(CartDTO cartDTO)
        {
            try
            {
                CartDTO newCartDTO = await _cartRepository.CreateUpdateCart(cartDTO);
                _response.Result = newCartDTO;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessge = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpPost("RemoveCart")]
        public async Task<object> RemoveCart([FromBody]int cartId)
        {
            try
            {
                bool isSuccess = await _cartRepository.RemoveFromCart(cartId);
                _response.Result = isSuccess;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessge = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpPost("ApplyCoupon")]
        public async Task<object> ApplyCoupon([FromBody] CartDTO cartDTO)
        {
            try
            {
                bool isSuccess = await _cartRepository.ApplyCoupon(cartDTO.CartHeader.UserId, cartDTO.CartHeader.CouponCode);
                _response.Result = isSuccess;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessge = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpPost("RemoveCoupon")]
        public async Task<object> RemoveCoupon([FromBody] string userId)
        {
            try
            {
                bool isSuccess = await _cartRepository.RemoveCoupon(userId);
                _response.Result = isSuccess;
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessge = new List<string>() { e.ToString() };
            }
            return _response;
        }

        [HttpPost("Checkout")]
        public async Task<object> Checkout(CheckoutHeaderDTO checkoutHeaderDTO)
        {
            try
            {
                CartDTO cartDTO = await _cartRepository.GetCartByUserId(checkoutHeaderDTO.UserId);
                if(cartDTO == null)
                {
                    return BadRequest();
                }

                if (!string.IsNullOrEmpty(checkoutHeaderDTO.CouponCode))
                {
                    CouponDTO coupon = await _couponRepository.GetCoupon(checkoutHeaderDTO.CouponCode);
                    if(checkoutHeaderDTO.DiscountTotal!= coupon.DiscountAmount)
                    {
                        _response.IsSuccess = false;
                        _response.ErrorMessge = new List<string>() { "Coupon Price không tồn tại hoặc lỗi" };
                        _response.DisplayMessage = "Coupon price bị lỗi";
                        return _response;
                    }
                }

                checkoutHeaderDTO.CartDetails = cartDTO.CartDetails;
                //
                await _messageBus.PublicMessage(checkoutHeaderDTO, "checkout/messagetopic");
            }
            catch (Exception e)
            {
                _response.IsSuccess = false;
                _response.ErrorMessge = new List<string>() { e.ToString() };
            }
            return _response;
        }
    }
}
