// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace ActiveLogging.Internal
{
	internal sealed class AsyncLogSink : IAsyncDisposable
	{
		private readonly ILogReceiver _receiver;
		private readonly IAsyncLogFlusher _flusher;
		private readonly Queue<LogEntry> _queue;

		public AsyncLogSink(Queue<LogEntry> queue, IAsyncLogFlusher flusher, ILogReceiver receiver)
		{
			_queue = queue;
			_flusher = flusher;
			_receiver = receiver;
		}

		public async ValueTask DisposeAsync()
		{
			if (_queue.Count == 0)
				return;
			await _flusher.BeginAsync(_receiver.GetCancellationToken());
			while (_queue.TryDequeue(out var entry))
				await _flusher.Add(entry, _receiver.GetCancellationToken());
			await _flusher.EndAsync(_receiver.GetCancellationToken());
		}
	}
}