using Microsoft.AspNetCore.Mvc;
using Todo_App.Application.TodoLists.Queries.GetTodos;

namespace Todo_App.WebUI.Controllers;

public class TagsController : ApiControllerBase
{

    [HttpGet]
    public async Task<ActionResult<List<TagDto>>> Get()
    {
        return await Mediator.Send(new GetTagsQuery());
    }
}
