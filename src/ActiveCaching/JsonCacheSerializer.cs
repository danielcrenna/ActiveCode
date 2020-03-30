// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Text;
using System.Text.Json;
using Microsoft.Extensions.Primitives;
using TypeKitchen.Serialization;

namespace ActiveCaching
{
	public sealed class JsonCacheSerializer : ICacheSerializer, ICacheDeserializer
	{
		public void ObjectToBuffer<T>(T value, ref Span<byte> buffer, ref int startAt)
		{
			buffer.WriteString(ref startAt, new StringValues(JsonSerializer.Serialize(value)));
		}

		public T BufferToObject<T>(ReadOnlySpan<byte> bytes)
		{
			unsafe
			{
				fixed (byte* b = &bytes.GetPinnableReference())
				{
					return JsonSerializer.Deserialize<T>(Encoding.UTF8.GetString(b, bytes.Length));
				}
			}
		}
	}
}