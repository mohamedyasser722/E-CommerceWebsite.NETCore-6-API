using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Talabat.APIs.Dtos;
using Talabat.APIs.Errors;
using Talabat.Core.Entities;
using Talabat.Core.Repositories.Interfaces;

namespace Talabat.APIs.Controllers
{
    public class BasketsController : BaseApiController
    {

        private readonly IBasketRepository _basketRepository;
        private readonly IMapper _mapper;
        public BasketsController(IBasketRepository basketRepository, IMapper mapper)
        {
            _basketRepository = basketRepository;
            _mapper = mapper;
        }

        // Get Or ReCreate Basket
        [HttpGet("{BasketId}")]
        public async Task<ActionResult<CustomerBasket>> GetBasketById(string BasketId)
        {
            var basket = await _basketRepository.GetBasketAsync(BasketId);

            return Ok(basket?? new CustomerBasket(BasketId));   // the same as: return basket == null ? new CustomerBasket(BasketId) : basket;
        }

        // Update Or Create New Basket
        [HttpPost]
        public async Task<ActionResult<CustomerBasketDto>> UpdateOrCreateBasket(CustomerBasketDto basket)
        {
            var MappedBasket = _mapper.Map<CustomerBasketDto, CustomerBasket>(basket);
            var CreateOrUpdatedBasket = await _basketRepository.UpdateBasketAsync(MappedBasket);

            if (CreateOrUpdatedBasket == null) return BadRequest(new ApiResponse(StatusCodes.Status400BadRequest));

            return Ok(CreateOrUpdatedBasket);
        }

        // Delete Basket
        [HttpDelete("{BasketId}")]
        public async Task<ActionResult<bool>> DeleteBasket(string BasketId)
        {
            return await _basketRepository.DeleteBasketAsync(BasketId);
        }
    }
}
