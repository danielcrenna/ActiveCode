// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using ActiveErrors;
using ActiveStorage;

namespace ActiveLogging
{
	public class LogService
	{
		private readonly IStorageProvider _provider;
		
		public LogService(IStorageProvider provider)
		{
			_provider = provider;
		}

		public async Task<Operation<ulong>> GetLogEntryCountAsync(string slot, CancellationToken cancellationToken = default)
		{
			var counter = _provider.GetCounter(slot);
			return await counter.CountAsync<LogEntry>(cancellationToken);
		}

		public async Task<Operation<IEnumerable<LogEntry>>> GetAllLogsAsync(string slot, CancellationToken cancellationToken = default)
		{
			var fetcher = _provider.GetFetcher(slot);
			return await fetcher.FetchAsync<LogEntry>(cancellationToken);
		}
	}
}