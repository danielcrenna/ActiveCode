// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ActiveCaching
{
	public interface ICacheSerializer
	{
		void ObjectToBuffer<T>(T value, ref Span<byte> buffer, ref int startAt);
	}
}