using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.SemanticKernel;
using Microsoft.SemanticKernel.ChatCompletion;
using Microsoft.SemanticKernel.Connectors.OpenAI;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using Vax.Data.Context;
using Vax.Data.Entity;
using Vax.Service.DTOS.RequestDto;
using Vax.Service.DTOS.ResponseDto;
using Vax.Service.Helper;
using Vax.Service.Interface;

namespace Vax.Service.Implmentation
{
	public class ChatService : IChatService
	{
		private readonly IMemoryCache _cache;
		private readonly Kernel _kernel;
		private readonly UserManager<AppUser> _userManager;
		private readonly Vaxdbcontext _vaxdbcontext;
		private readonly IEmailService _emailService;
		private readonly VaxPlugin _vaxPlugin;
		private readonly IChatCompletionService _chatService;

		public ChatService(IMemoryCache cache, Kernel kernel,
			UserManager<AppUser> userManager,
			Vaxdbcontext vaxdbcontext,
			IEmailService emailService,VaxPlugin vaxPlugin)
		{
			_cache = cache;
			_kernel = kernel;
			_userManager = userManager;
			_vaxdbcontext = vaxdbcontext;
			_emailService = emailService;
			_vaxPlugin = vaxPlugin;
			_chatService = kernel.GetRequiredService<IChatCompletionService>();

		}
		public async Task<BaseResult<string>> Ask(ChatRequest request, string appuserId)
		{
			var userId = await _userManager.FindByIdAsync(appuserId);


			var plugin = _kernel.Plugins.AddFromObject(_vaxPlugin, "VaxPlugin");

			OpenAIPromptExecutionSettings executionSettings = new OpenAIPromptExecutionSettings
			{
				FunctionChoiceBehavior = FunctionChoiceBehavior.Auto(),
			};

			var loadmessages = await _vaxdbcontext.ChatMessages.Where
				(u => u.UserId == userId.Id)
				.OrderBy(o => o.CreatedAt)
				.ToListAsync();

			#region Save the Chat in MemoryCache
			//var history = _cache.GetOrCreate($"chat_{userId}", entry =>
			//{
			//	entry.SlidingExpiration = TimeSpan.FromMinutes(30);

			//	var h = new ChatHistory();
			//	h.AddSystemMessage(
			//		"You are a smart medical assistant for VaxManager. " +
			//		"You help doctors manage patients and vaccination schedules. " +
			//		"Be concise and professional. " +
			//		"Respond in the same language the doctor uses.");
			//	return h;
			//}); 
			#endregion

			var history = new ChatHistory();
			history.AddSystemMessage(
				"You are a medical assistant for VaxManager. " +
				"You ONLY answer questions related to medicine, vaccines, patients, and healthcare. " +
				"If the user asks about anything outside of medicine — such as programming, sports, politics, or any other topic — " +
				"you must politely refuse and say that you can only assist with medical topics. " +
				"Respond in the same language the doctor uses.");

			foreach (var message in loadmessages)
			{
				if (message.Role == "user")
				{
					history.AddUserMessage(message.Message);
				}
				else if (message.Role == "assistant")
				{
					history.AddAssistantMessage(message.Message);
				}
			}
			history.AddUserMessage(request.Message);


			//Save the User Message in DB
			_vaxdbcontext.ChatMessages.Add(new ChatMessage
			{
				UserId = userId.Id,
				Message = request.Message,
				Role = "user",
			});

			//Get AI Response
			var result = await _chatService.GetChatMessageContentAsync(
				history,
				executionSettings: executionSettings,
				kernel: _kernel);

			//Save the AI Response in DB

			_vaxdbcontext.ChatMessages.Add(new ChatMessage
			{
				UserId = userId.Id,
				Message = result.Content ?? string.Empty,
				Role = "assistant",
			});

			await _vaxdbcontext.SaveChangesAsync();

			//history.AddMessage(result.Role, result.Content ?? string.Empty);

			return new BaseResult<string>
			{
				Data = result.Content ?? string.Empty
			};
		}
	}
}
