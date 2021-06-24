using Mango.Web.Models;
using Mango.Web.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace Mango.Web.Services
{
    public class ShoppingCartService : BaseService, IShoppingCartService
    {
        private readonly IHttpClientFactory _clientFactory;
        public ShoppingCartService(IHttpClientFactory clientFactory) : base(clientFactory)
        {
            _clientFactory = clientFactory;
        }


        public async Task<T> AddToCartAsync<T>(CartDTO cartDTO, string token = null)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDTO,
                Url = SD.ShoppingCartAPIBase + "/cart/AddCart",
                AccessToken = token
            });
        }

        public async Task<T> ApplyCoupon<T>(CartDTO cartDTO, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = cartDTO,
                Url = SD.ShoppingCartAPIBase + "/cart/ApplyCoupon",
                AccessToken = token
            });
        }

        public async Task<T> Checkout<T>(CartHeaderDTO cartHeaderDTO, string token = null)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartHeaderDTO,
                Url = SD.ShoppingCartAPIBase + "/cart/checkout",
                AccessToken = token
            });
        }

        public async Task<T> GetCartByUserIdAsync<T>(string userId, string token = null)
        {
            {
                return await SendAsync<T>(new ApiRequest()
                {
                    ApiType = SD.ApiType.GET,
                    Url = SD.ShoppingCartAPIBase + "/cart/GetCart/" + userId,
                    AccessToken = token
                });
            }

        }

        public async Task<T> RemoveCoupon<T>(string userId, string token = null)
        {
            return await SendAsync<T>(new ApiRequest
            {
                ApiType = SD.ApiType.POST,
                Data = userId,
                Url = SD.ShoppingCartAPIBase + "/cart/RemoveCoupon",
                AccessToken = token
            });
        }

        public async Task<T> RemoveFromCartAsync<T>(int cartId, string token = null)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartId,
                Url = SD.ShoppingCartAPIBase + "/cart/RemoveCart",
                AccessToken = token
            });
        }

        public async Task<T> UpdateCartAsync<T>(CartDTO cartDTO, string token = null)
        {
            return await SendAsync<T>(new ApiRequest()
            {
                ApiType = SD.ApiType.POST,
                Data = cartDTO,
                Url = SD.ShoppingCartAPIBase + "/cart/UpdateCart",
                AccessToken = token
            });
        }
    }
}
