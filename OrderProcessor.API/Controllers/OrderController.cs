using Microsoft.AspNetCore.Mvc;
using OrderProcessor.API.Mapper;
using OrderProcessor.API.Models;
using OrderProcessor.Domain.Builder;
using OrderProcessor.Domain.Service;
using System;

namespace OrderProcessor.API.Controllers
{
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _service;
        private readonly IOrderBuilder _builder;

        public OrderController(IOrderService service, IOrderBuilder builder)
        {
            _service = service;
            _builder = builder;
        }

        [HttpPost]
        public IActionResult Post([FromBody] OrderRequest request)
        {
            try
            {
                var order = OrderMapper.OrderMapperRequest(request, _builder);
                _service.Publish(order);

                return Accepted();
            }
            catch(Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
