// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Text.Json;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using TypeKitchen;

namespace ActiveLogging
{
	public sealed class ActiveStorageLogger : ILogger
	{
		private readonly string _categoryName;
		private readonly ActiveStorageLoggerProvider _provider;
		private readonly ILogReceiver _receiver;
		private readonly IOptionsMonitor<LoggerFilterOptions> _options;

		public ActiveStorageLogger(string categoryName, ActiveStorageLoggerProvider provider, ILogReceiver receiver, IOptionsMonitor<LoggerFilterOptions> options)
		{
			_categoryName = categoryName;
			_receiver = receiver;
			_provider = provider;
			_options = options;
		}

		public bool IsEnabled(LogLevel logLevel) => logLevel != LogLevel.None;
		public IDisposable BeginScope<TState>(TState state) => _provider.BeginScope(state);

		public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
		{
			if (!IsEnabled(logLevel))
				return;

			var cancellationToken = _receiver.GetCancellationToken();

			var entry = new LogEntry
			{
				LogLevel = logLevel,
				EventId = eventId.Id,
				EventName = eventId.Name,
				State =  SerializeState(state),
				Exception = SerializeException(exception),
				Message = formatter(state, exception)
			};

			_receiver.Append(entry, cancellationToken);
		}

		private static string SerializeException(Exception exception)
		{
			return JsonSerializer.Serialize(exception);
		}

		private static string SerializeState<TState>(TState state)
		{
			var hash = new Dictionary<string, object>();

			if (state is IReadOnlyList<KeyValuePair<string, object>> list)
			{
				foreach (var (k, v) in list)
					hash.Add(k, v);
			}
			else
			{
				var accessor = ReadAccessor.Create(state, AccessorMemberTypes.Properties, AccessorMemberScope.Public, out var members);
				foreach (var member in members)
					if (accessor.TryGetValue(state, member.Name, out var value))
						hash.Add(member.Name, value);
			}

			var stateJson = JsonSerializer.Serialize(hash);
			return stateJson;
		}
	}
}