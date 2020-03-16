// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ActiveCaching
{
	/// <summary>
	///     See: https://tools.ietf.org/html/rfc7232#section-2
	/// </summary>
	public interface IETagGenerator
	{
		string GenerateFromBuffer(ReadOnlySpan<byte> data);
	}
}