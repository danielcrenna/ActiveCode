// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Internal;

namespace ActiveCaching.Internal
{
	internal sealed class DelegatedSystemClock : ISystemClock
	{
		private readonly Func<DateTimeOffset> _timestamps;

		public DelegatedSystemClock(Func<DateTimeOffset> timestamps) => _timestamps = timestamps;

		public DateTimeOffset UtcNow => _timestamps().ToUniversalTime();
	}
}