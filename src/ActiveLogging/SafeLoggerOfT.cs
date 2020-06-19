// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.Extensions.Logging;

namespace ActiveLogging
{
	public class SafeLogger<TCategoryName> : SafeLogger, ISafeLogger<TCategoryName>
	{
		public SafeLogger(ILogger<TCategoryName> inner) : base(inner) { }
	}
}