// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveLogging.Internal
{
	internal sealed class BufferedAsyncLogSink : IAsyncDisposable
	{
		private readonly ILogAppender _appender;
		private readonly IAsyncLogFlusher _flusher;
		private readonly Queue<LogEntry> _queue;

		public BufferedAsyncLogSink(Queue<LogEntry> queue, IAsyncLogFlusher flusher, ILogAppender appender)
		{
			_queue = queue;
			_flusher = flusher;
			_appender = appender;
		}

		public async ValueTask DisposeAsync()
		{
			if (_queue.Count == 0)
				return;
			await _flusher.BeginAsync(_appender.GetCancellationToken());
			while (_queue.TryDequeue(out var entry))
				await _flusher.Add(entry, _appender.GetCancellationToken());
			await _flusher.EndAsync(_appender.GetCancellationToken());
		}
	}
}