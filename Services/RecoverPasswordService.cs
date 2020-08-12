using Contracts;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Models.Commands;
using Models.DTOs;
using Models.Entities;
using Models.Enums;
using Models.Exceptions;
using Services.DataLayer;
using System;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
	public class RecoverPasswordService : IRecoverPasswordService
	{
		private readonly UsersDbContext _dbContext;
		private readonly ICryptoEngineService _cryptoEngineService;
		private readonly IHttpContextAccessor _httpContext;

		public RecoverPasswordService(ICryptoEngineService cryptoEngineService,
			UsersDbContext dbContext, IHttpContextAccessor httpContext)
		{
			_cryptoEngineService = cryptoEngineService;
			_dbContext = dbContext;
			_httpContext = httpContext;
		}

		public async Task<RecoverPassword> GetRecoverPasswordAsync(Guid id)
		{
			return await _dbContext.RecoverPasswords.FirstOrDefaultAsync(a => a.Id == id);
		}

		public async Task TriggerForgottenPasswordAsync(ForgettenPasswordCommand command)
		{
			var user = await _dbContext.Users.AsNoTracking().FirstOrDefaultAsync(a => a.Email == command.Email);
			if (user == null)
			{
				throw new UserException($"No User found for given email: {command.Email}.", ErrorCodes.UserWithGivenEmailNotFound);
			}
			string randomKey = GenerateRandomText(50);
			var recoverPassword = new RecoverPassword
			{
				Email = command.Email,
				DateAdded = DateTime.UtcNow,
				Id = Guid.NewGuid(),
				LastModifiedDate = DateTime.UtcNow
			};
			var hash = _cryptoEngineService.Encrypt(randomKey, recoverPassword.Id.ToString());
			recoverPassword.Hash = RemoveSpecialCharsFromHash(hash);
			await _dbContext.AddAsync<RecoverPassword>(recoverPassword);
			await _dbContext.SaveChangesAsync();

			//TODO: Notify user or other service with  information about reseting password.
		}

		public async Task SetNewUserPasswordAsync(SetNewUserPasswordCommand command, Guid id, string hash)
		{
			var recoverPassword = await _dbContext.RecoverPasswords.FirstOrDefaultAsync(a => a.Id == id);
			if (recoverPassword == null)
			{
				throw new UserException($"Link to reset password not valid.", ErrorCodes.ResetPasswordLinkInValid);
			}
			if (recoverPassword.Hash != hash)
			{
				throw new UserException($"Link to reset password not valid.", ErrorCodes.ResetPasswordLinkInValid);
			}

			if (recoverPassword.DateAdded.AddHours(24) < DateTime.UtcNow)
			{
				throw new UserException($"Link to reset password has expired.", ErrorCodes.ResetPasswordLinkExpired);
			}

			var user = await _dbContext.Users.FirstOrDefaultAsync(a => a.Email == recoverPassword.Email);
			user.Password = _cryptoEngineService.Encrypt(command.Password, user.Id.ToString());
			await _dbContext.SaveChangesAsync();
		}

		private static string GenerateRandomText(int length)
		{
			const string valid = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890";
			StringBuilder res = new StringBuilder();
			Random rnd = new Random();
			while (0 < length--)
			{
				res.Append(valid[rnd.Next(valid.Length)]);
			}
			return res.ToString();
		}

		private static string RemoveSpecialCharsFromHash(string hash)
		{
			var stringBuilder = new StringBuilder();
			foreach (var c in hash)
			{
				if (char.IsLetterOrDigit(c))
				{
					stringBuilder.Append(c);
				}
			}

			return stringBuilder.ToString();
		}
	}
}