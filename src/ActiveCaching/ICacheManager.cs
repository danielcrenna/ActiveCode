// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ActiveCaching
{
	public interface ICacheManager
	{
		int KeyCount { get; }
		long? SizeLimitBytes { get; set; }
		long SizeBytes { get; set; }
	}
}