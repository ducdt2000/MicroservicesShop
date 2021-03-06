using Mango.Services.ShoppingCartAPI.Models.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.ShoppingCartAPI.Repository.Interfaces
{
    public interface ICartRepository
    {
        Task<CartDTO> GetCartByUserId(string userId);
        Task<CartDTO> CreateUpdateCart(CartDTO cartDTO);
        Task<bool> RemoveFromCart(int cartDetailsId);
        Task<bool> ApplyCoupon(string userId, string couponCode);
        Task<bool> RemoveCoupon(string userId);
        Task<bool> ClearCart(string userId);
    }
}
