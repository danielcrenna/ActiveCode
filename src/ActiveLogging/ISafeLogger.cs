// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Logging;

namespace ActiveLogging
{
	/// <inheritdoc />
	public interface ISafeLogger : ILogger
	{
		void Trace(Func<string> message, params object[] args);
		void Trace(Func<string> message, Exception exception, params object[] args);

		void Debug(Func<string> message, params object[] args);
		void Debug(Func<string> message, Exception exception, params object[] args);

		void Info(Func<string> message, params object[] args);
		void Info(Func<string> message, Exception exception, params object[] args);

		void Warn(Func<string> message, params object[] args);
		void Warn(Func<string> message, Exception exception, params object[] args);

		void Error(Func<string> message, params object[] args);
		void Error(Func<string> message, Exception exception, params object[] args);

		void Critical(Func<string> message, params object[] args);
		void Critical(Func<string> message, Exception exception, params object[] args);
	}
}