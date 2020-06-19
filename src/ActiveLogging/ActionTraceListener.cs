// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;

namespace ActiveLogging
{
	public sealed class ActionTraceListener : TraceListener
	{
		private readonly Action<string> _write;
		private readonly Action<string> _writeLine;

		public ActionTraceListener(Action<string> write, Action<string> writeLine = null)
		{
			_write = write;
			_writeLine = writeLine ?? write;
		}

		public override void WriteLine(string value)
		{
			_writeLine?.Invoke(value);
		}

		public override void Write(string value)
		{
			_write?.Invoke(value);
		}
	}
}