using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.Interface;

namespace VaxManager.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ChatController : ControllerBase
	{
		private readonly IChatService _chatService;

		public ChatController(IChatService chatService)
		{
			_chatService = chatService;
		}

		[HttpPost("ask")]
		public async Task<IActionResult> Ask([FromBody] ChatRequest request)
		{

			var UserId = User.FindFirst("UserId").Value;

			var result = await _chatService.Ask(request, UserId);

			return Ok(result);
		}
	}
}
