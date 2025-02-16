
using System.Net;
using System.Text.Json;
using Vax.Service.Helper;

namespace  VaxManager.Middlewares
{
	public class CustomExceptionHandlerMiddleware
	{
		private readonly RequestDelegate _next;

		public CustomExceptionHandlerMiddleware(RequestDelegate next)
		{
			_next = next;
		}

		public async Task InvokeAsync(HttpContext context)
		{
			try
			{
				await _next(context);
			}
			catch (Exception error)
			{
				var response = context.Response;
				response.ContentType = "application/json";
				var responseModel = new BaseResult<string>() { IsSuccess = false, Errors = [error?.Message] };
				switch (error)
				{
					case CustomException e:
						// custom application error
						response.StatusCode = e.StatusCode;
						break;
					//case KeyNotFoundException e:
					//    // not found error
					//    response.StatusCode = (int)HttpStatusCode.NotFound;
					//    break;
					default:
						// unhandled error
						response.StatusCode = (int)HttpStatusCode.InternalServerError;
						break;
				}
				var result = JsonSerializer.Serialize(responseModel);

				await response.WriteAsync(result);
			}
		}
	}
}
