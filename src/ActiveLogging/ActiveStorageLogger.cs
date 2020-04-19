// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

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

			_receiver.Append(new LogEntry
			{
				LogLevel = logLevel,
				EventId = eventId,
				State = state,
				Exception = exception,
				Message = formatter(state, exception)
			}, cancellationToken);
		}
	}
}