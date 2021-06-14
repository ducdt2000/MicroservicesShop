﻿using Mango.Services.ShoppingCartAPI.Models.DTOs;
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
    public class CartController : Controller
    {
        private readonly ICartRepository _cartRepository;
        protected ResponseDTO _response;

        public CartController(ICartRepository cartRepository)
        {
            _cartRepository = cartRepository;
            this._response = new ResponseDTO();
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
        public async Task<object> RemoveCart(int cartId)
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
    }
}
