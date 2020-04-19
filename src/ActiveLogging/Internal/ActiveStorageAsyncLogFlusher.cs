// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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

		public ActiveStorageAsyncLogFlusher(IStorageProvider storageProvider, IOptionsMonitor<ActiveStorageLoggerOptions> options)
		{
			_storageProvider = storageProvider;
			_options = options;
		}

		public Task BeginAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
		public async Task Add<T>(T entry, CancellationToken cancellationToken = default) => await _storageProvider.GetAppender(_options.CurrentValue.Slot).CreateAsync(entry, cancellationToken);
		public Task EndAsync(CancellationToken cancellationToken = default) => Task.CompletedTask;
	}
}