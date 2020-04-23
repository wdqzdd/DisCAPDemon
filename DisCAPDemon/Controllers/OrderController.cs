using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DotNetCore.CAP;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DisCAPDemon.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrderController : ControllerBase
    {
        private readonly ICapPublisher _capBus;

        public OrderController(ICapPublisher capPublisher)
        {
            _capBus = capPublisher;
        }

        // GET: api/Order
        [HttpGet("getdt")]
        public IEnumerable<string> Get()
        {

            _capBus.
            _capBus.Publish("xxx.services.show.time", DateTime.Now);

            return new string[] { "value1", "value2" };
        }

        // GET: api/Order/5
        [HttpGet("{id}", Name = "Get")]
        public string Get(int id)
        {
            _capBus.Publish("xxx.services.show.time", DateTime.Now);
            return "value";
        }

        //[CapSubscribe("xxx.services.show.time")]
        //public void CheckReceivedMessage(DateTime datetime)
        //{
        //    Console.WriteLine(datetime);
        //}

        // POST: api/Order
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT: api/Order/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE: api/ApiWithActions/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
