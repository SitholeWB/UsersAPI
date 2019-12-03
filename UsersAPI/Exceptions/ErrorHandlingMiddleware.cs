using Contracts;
using Microsoft.AspNetCore.Http;
using Models.Entities;
using Models.Enums;
using Models.Exceptions;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Net;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;

namespace UsersAPI.Exceptions
{
	public class ErrorHandlingMiddleware
	{
		private readonly RequestDelegate next;
		private IErrorLogService _errorLogService;

		public ErrorHandlingMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context, IErrorLogService errorLogService /* other dependencies */)
		{
			_errorLogService = errorLogService;
			var hadError = false;
			try
			{
				await next(context);
			}
			catch (Exception ex)
			{
				hadError = true;
				await HandleExceptionAsync(context, ex);
			}
			finally
			{
				if (hadError)
				{
					await next(context);
				}
			}
		}

		private async Task HandleExceptionAsync(HttpContext context, Exception exception)
		{
			try
			{
				var errorType = (exception is UserException)? ErrorTypes.UserException : ErrorTypes.Unexpected;
				await _errorLogService.AddErrorLogAsync(new ErrorLog
				{
					DateAdded = DateTime.UtcNow,
					Exception = JsonSerializer.Serialize(exception),
					LocationInCode = $"{exception.Source}",
					Message = $"Error: {exception.Message}",
					Id = Guid.NewGuid(),
					LastModifiedDate = DateTime.UtcNow,
					Type = errorType.ToString(),
					RequestDetails = JsonSerializer.Serialize(new
					{
						url = GetUri(context.Request).AbsoluteUri
					})
				});
			}
			finally
			{
				await HandleExceptionResponseAsync(context, exception);
			}
		}

		private static Task HandleExceptionResponseAsync(HttpContext context, Exception exception)
		{
			var code = HttpStatusCode.InternalServerError; // 500 if unexpected

			var result = JsonSerializer.Serialize(new { Error = exception.Message });
			if (exception is UserException)
			{
				var ex = exception as UserException;
				if (ex.ErrorCode >= 10 && ex.ErrorCode <= 50)
				{
					code = HttpStatusCode.BadRequest;
				}
				else if (ex.ErrorCode >= 51 && ex.ErrorCode <= 100)
				{
					code = HttpStatusCode.NotFound;
				}

				result = JsonSerializer.Serialize(new
				{
					Error = exception.Message,
					UserException = new
					{
						Code = ex.Code,
						Description = GetEnumDescription((ErrorCodes)ex.ErrorCode)
					}
				});
			}


			context.Response.ContentType = "application/json";
			context.Response.StatusCode = (int)code;
			return context.Response.WriteAsync(result);
		}

		public static string GetEnumDescription(Enum value)
		{
			var fileInfo = value.GetType().GetField(value.ToString());

			var attributes = fileInfo.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

			if (attributes?.Any() ?? false)
			{
				return attributes.First().Description;
			}

			return value.ToString();
		}

		public static Uri GetUri(HttpRequest request)
		{
			var uriBuilder = new UriBuilder
			{
				Scheme = request.Scheme,
				Host = request.Host.Host,
				Port = request.Host.Port.GetValueOrDefault(80),
				Path = request.Path.ToString(),
				Query = request.QueryString.ToString()
			};
			return uriBuilder.Uri;
		}
	}
}
