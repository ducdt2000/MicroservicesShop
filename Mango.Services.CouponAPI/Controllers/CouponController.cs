using Mango.Services.CouponAPI.Models.DTOs;
using Mango.Services.CouponAPI.Repository.Interfaces;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mango.Services.CouponAPI.Controllers
{
    [ApiController]
    [Route("api/coupon")]
    public class CouponController : Controller
    {
        private readonly ICouponRepository _couponRepository;
        protected ResponseDTO _response;

        public CouponController(ICouponRepository couponRepository)
        {
            _couponRepository = couponRepository;
            _response = new ResponseDTO();
        }


        [HttpGet("{code}")]
        public async Task<object> GetDiscountForCode(string code)
        {
            try
            {
                var couponDTO = await _couponRepository.GetCouponByCode(code);
                _response.Result = couponDTO;
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
