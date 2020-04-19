// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Logging;

namespace ActiveLogging
{
	public sealed class LogEntry
	{
		public LogLevel LogLevel { get; set; }
		public EventId EventId { get; set; }
		public object State { get; set; }
		public Exception Exception { get; set; }
		public string Message { get; set; }
	}
}