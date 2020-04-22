// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Threading;
using System.Threading.Tasks;
using ActiveStorage;
using Microsoft.Extensions.Options;

namespace ActiveLogging.Internal
{
	internal sealed class ActiveStorageAsyncLogFlusher : IAsyncLogFlusher
	{
		private readonly IStorageProvider _storageProvider;
		private readonly IOptionsMonitor<ActiveStorageLoggerOptions> _options;
		private Queue _queue;

		public ActiveStorageAsyncLogFlusher(IStorageProvider storageProvider, IOptionsMonitor<ActiveStorageLoggerOptions> options)
		{
			_storageProvider = storageProvider;
			_options = options;
		}

		public Task BeginAsync(CancellationToken cancellationToken = default)
		{
			_queue = new Queue();
			return Task.CompletedTask;
		}

		public Task Add<T>(T entry, CancellationToken cancellationToken = default)
		{
			_queue.Enqueue(entry);
			return Task.CompletedTask;
		}

		public async Task EndAsync(CancellationToken cancellationToken = default)
		{
			if (_queue?.Count == 0)
				return;

			var appender = _storageProvider.GetAppender(_options.CurrentValue.Slot);
			while (_queue?.Count > 0)
				await appender.CreateAsync(_queue.Dequeue(), cancellationToken);
		}
	}
}