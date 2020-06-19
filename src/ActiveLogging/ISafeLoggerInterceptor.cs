// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Logging;

namespace ActiveLogging
{
	public interface ISafeLoggerInterceptor
	{
		bool CanIntercept { get; }

		bool TryLog<TState>(ILogger inner, ref int safe, LogLevel logLevel, EventId eventId, TState state,
			Exception exception, Func<TState, Exception, string> formatter);
	}
}