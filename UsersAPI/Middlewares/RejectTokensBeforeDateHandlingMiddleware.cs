using Contracts;
using Microsoft.AspNetCore.Http;
using Models.Constants;
using Models.Enums;
using Models.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace UsersAPI.Middlewares
{
	public class RejectTokensBeforeDateHandlingMiddleware
	{
		private readonly RequestDelegate next;

		public RejectTokensBeforeDateHandlingMiddleware(RequestDelegate next)
		{
			this.next = next;
		}

		public async Task Invoke(HttpContext context, IUserIdendityService userIdendityService /* other dependencies */)
		{
			var tokenCreatedDate = context.User.Claims.FirstOrDefault(a => a.Type == CustomClaimTypes.CreatedDate);
			if (tokenCreatedDate != null)
			{
				var user = await userIdendityService.GetAuthorizedUserAsync();
				if (user.RejectTokensBeforeDate.HasValue && !string.IsNullOrEmpty(tokenCreatedDate.Value))
				{
					var createdDate = DateTime.Parse(tokenCreatedDate.Value);
					if (createdDate <= user.RejectTokensBeforeDate.Value)
					{
						throw new UserException("Auth token has been rejected, create the new one or remove it if you accessing anonymous endpoint.", ErrorCodes.TokenRejected);
					}
				}
			}

			await next(context);
		}
	}
}