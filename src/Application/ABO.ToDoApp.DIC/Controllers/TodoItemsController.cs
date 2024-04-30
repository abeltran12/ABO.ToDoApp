using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ABO.ToDoApp.DIC.Controllers
{
    [Authorize(Policy = "OwnerOfTodoListPolicy")]
    [Route("api/todolists/{todolistId}/todoitem")]
    [ApiController]
    public class TodoItemsController : ControllerBase
    {
        // GET: api/<TodoItemsController>
        [HttpGet]
        public IEnumerable<string> Get(int todolistId)
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<TodoItemsController>/5
        [HttpGet("{id}")]
        public string Get(int todolistId, int id)
        {
            return "value";
        }

        // POST api/<TodoItemsController>
        [HttpPost]
        public void Post(int todolistId,[FromBody] string value)
        {
        }

        // PUT api/<TodoItemsController>/5
        [HttpPut("{id}")]
        public void Put(int todolistId, int id, [FromBody] string value)
        {
        }

        // DELETE api/<TodoItemsController>/5
        [HttpDelete("{id}")]
        public void Delete(int todolistId, int id)
        {
        }
    }
}