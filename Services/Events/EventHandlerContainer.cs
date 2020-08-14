using Contracts;
using Models.Entities;
using Models.Events;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Services.Events
{
	public class EventHandlerContainer
	{
		private readonly IServiceProvider _serviceProvider;
		private static readonly Dictionary<Type, List<Type>> _mappings = new Dictionary<Type, List<Type>>();
		private readonly IBackgroundTaskQueue _backgroundTaskQueue;
		private readonly IErrorLogService _errorLogService;

		public EventHandlerContainer(IServiceProvider serviceProvider, IBackgroundTaskQueue backgroundTaskQueue, IErrorLogService errorLogService)
		{
			_serviceProvider = serviceProvider;
			_backgroundTaskQueue = backgroundTaskQueue;
			_errorLogService = errorLogService;
		}

		public static void Subscribe<T, THandler>()
			where T : EventBase
			where THandler : IEventHandler<T>
		{
			var name = typeof(T);

			if (!_mappings.ContainsKey(name))
			{
				_mappings.Add(name, new List<Type> { });
			}

			_mappings[name].Add(typeof(THandler));
		}

		public static void Unsubscribe<T, THandler>()
			where T : EventBase
			where THandler : IEventHandler<T>
		{
			var name = typeof(T);
			_mappings[name].Remove(typeof(THandler));

			if (_mappings[name].Count == 0)
			{
				_mappings.Remove(name);
			}
		}

		/// <summary>
		/// Execute event/s in background thread, does not wait for all subscribers to finish execution.
		/// NOTE: All subscribers will be awaited to finish execution in background.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="eventObject"></param>
		public void Publish<T>(T eventObject) where T : EventBase
		{
			var name = typeof(T);

			if (_mappings.ContainsKey(name))
			{
				foreach (var handler in _mappings[name])
				{
					var service = (IEventHandler<T>)_serviceProvider.GetService(handler);
					_backgroundTaskQueue.QueueBackgroundWorkItem(async workItem =>
					{
						await service.RunAsync(eventObject);
					});
				}
			}
		}

		/// <summary>
		///Execute event/s in immediately, wait for all subscribers/handlers to finish execution.
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="eventObject"></param>
		/// <returns></returns>
		public async Task PublishAsync<T>(T eventObject) where T : EventBase
		{
			var name = typeof(T);

			if (_mappings.ContainsKey(name))
			{
				foreach (var handler in _mappings[name])
				{
					try
					{
						var service = (IEventHandler<T>)_serviceProvider.GetService(handler);
						await service.RunAsync(eventObject);
					}
					catch (Exception ex)
					{
						await _errorLogService.AddErrorLogAsync(new ErrorLog
						{
							Id = Guid.NewGuid(),
							DateAdded = DateTime.UtcNow,
							LastModifiedDate = DateTime.UtcNow,
							LocationInCode = $"{this.GetType().Name}_{ nameof(T)}",
							Type = "EventExecution",
							Exception = JsonConvert.SerializeObject(ex),
							Message = ex.Message,
							RequestDetails = JsonConvert.SerializeObject(eventObject)
						});
					}
				}
			}
		}
	}
}