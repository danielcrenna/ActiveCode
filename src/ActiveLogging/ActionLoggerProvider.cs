// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Logging;

namespace ActiveLogging
{
	public sealed class ActionLoggerProvider : ILoggerProvider
	{
		private readonly Action<string> _writeLine;

		public ActionLoggerProvider(Action<string> writeLine) => _writeLine = writeLine;

		public ILogger CreateLogger(string categoryName)
		{
			return new ActionLogger(categoryName, _writeLine);
		}

		public void Dispose()
		{
		}
	}
}