using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace dotnetcore_jwt_auth.Controllers
{
    /// <summary>
    /// Mediator version of the original Customer Controller
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerMediatorController : ControllerBase
    {
        private readonly IMediator _mediator; // internal member for DI
        public CustomerMediatorController(IMediator mediator)
        {
            //DI here. When we installed package MediatR.Extensions.Microsoft.DependencyInjection, the DI was set up as part of services.AddMediatR(typeof(Startup)) in startup.cs.
            _mediator = mediator;
        }

        // GET api/values
        // [Authorize(Roles = "Manager")]
        // [HttpGet]
        // public IEnumerable<string> Get()
        // {
        //     return new string[] { "John Doe", "Jane Doe" };
        // }

        [HttpGet]
        public async Task<IEnumerable<string>> Get()
        {
            return await _mediator.Send(new GetCustomer.Query());
        }
    }
}