using BackendExamHub.Model;
using BackendExamHub.Services;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace BackendExamHub.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MyofficeAcpdController : ControllerBase
    {
        private readonly MyofficeAcpdService _service;

        public MyofficeAcpdController(MyofficeAcpdService service)
        {
            _service = service;
        }

        //依sid查詢
        [HttpGet("{id}")]
        public async Task<ActionResult<MyofficeAcpd>> Get(int id)
        {
            var item = await _service.GetByIdAsync(id);
            if (item == null)
            {
                return NotFound();
            }
            return Ok(item);
        }

        [HttpPost]
        public async Task<ActionResult> Post([FromBody] MyofficeAcpd item)
        {
            await _service.CreateAsync(item);
            return CreatedAtAction(nameof(Get), new { id = item.AcpdSid }, item);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Put(int id, [FromBody] MyofficeAcpd item)
        {
            if (id != item.AcpdSid)
            {
                return BadRequest();
            }
            await _service.UpdateAsync(item);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<ActionResult> Delete(int id)
        {
            await _service.DeleteAsync(id);
            return NoContent();
        }
    }
}
