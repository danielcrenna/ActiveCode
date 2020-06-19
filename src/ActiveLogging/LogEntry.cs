// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;

namespace ActiveLogging
{
	public sealed class LogEntry
	{
		public LogLevel LogLevel { get; set; }
		public int EventId { get; set; }
		public string EventName { get; set; }
		public string State { get; set; }
		public string Exception { get; set; }
		public string Message { get; set; }
	}
}