// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using System.Threading;
using Microsoft.AspNetCore.Http;

namespace ActiveLogging.Internal
{
	internal sealed class HttpContextLogReceiver : ILogReceiver
	{
		private const string QueueContextKey = "HttpContextLogReceiver_Queue";

		private readonly IHttpContextAccessor _accessor;
		private readonly IAsyncLogFlusher _flusher;

		public HttpContextLogReceiver(IHttpContextAccessor accessor, IAsyncLogFlusher flusher)
		{
			_accessor = accessor;
			_flusher = flusher;
		}

		public void Append(LogEntry logEntry, in CancellationToken cancellationToken = default)
		{
			var context = _accessor.HttpContext;
			if (context == null)
				return;

			if (!context.Items.TryGetValue(QueueContextKey, out var cached))
			{
				var newQueue = new Queue<LogEntry>();
				context.Items.Add(QueueContextKey, cached = newQueue);
				context.Response.RegisterForDisposeAsync(new BufferedAsyncLogSink(newQueue, _flusher, this));
			}

			if (cached is Queue<LogEntry> queue)
				queue.Enqueue(logEntry);
		}

		public CancellationToken GetCancellationToken() => _accessor.HttpContext?.RequestAborted ?? default;
	}
}