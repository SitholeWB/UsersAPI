using Models.Commands.Responses;
using Models.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace Services.Helpers
{
	public static class UserModelsHelper
	{
		public static UserResponse ConvertUserToUserResponse(User userEntity)
		{
			if (userEntity == null)
			{
				return null;
			}
			return new UserResponse
			{
				About = userEntity.About,
				AccountAuth = userEntity.AccountAuth,
				Country = userEntity.Country,
				DateAdded = userEntity.DateAdded,
				Email = userEntity.Email,
				Gender = userEntity.Gender,
				Id = userEntity.Id,
				LastModifiedDate = userEntity.LastModifiedDate,
				Name = userEntity.Name,
				Role = userEntity.Role,
				Status = userEntity.Status,
				Surname = userEntity.Surname,
				Username = userEntity.Username,
			};
		}
	}
}