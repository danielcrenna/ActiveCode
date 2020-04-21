// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Concurrent;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace ActiveLogging
{
	public sealed class ActiveStorageLoggerProvider : ILoggerProvider, ISupportExternalScope
	{
		private readonly ILogReceiver _receiver;
		private readonly IOptionsMonitor<LoggerFilterOptions> _options;

		private readonly ConcurrentDictionary<string, ActiveStorageLogger> _cache = new ConcurrentDictionary<string, ActiveStorageLogger>();

		public ActiveStorageLoggerProvider(ILogReceiver receiver, IOptionsMonitor<LoggerFilterOptions> options)
		{
			_receiver = receiver;
			_options = options;
		} 
		
		private IExternalScopeProvider _scopeProvider;
		public ILogger CreateLogger(string categoryName) => _cache.GetOrAdd(categoryName, c => new ActiveStorageLogger(categoryName, this, _receiver, _options));
		public IDisposable BeginScope<TState>(TState state) => _scopeProvider.Push(state);
		public void SetScopeProvider(IExternalScopeProvider scopeProvider) => _scopeProvider = scopeProvider;
		public void Dispose() { }

		public void ForEachScope<TState>(Action<object, TState> callback, TState state)
		{
			_scopeProvider.ForEachScope(callback, state);
		}
	}
}