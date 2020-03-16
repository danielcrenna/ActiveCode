// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ActiveCaching.Configuration
{
	public class CacheOptions
	{
		public TimeSpan? ContentionTimeout { get; set; }
		public long? MaxSizeBytes { get; set; }
	}
}