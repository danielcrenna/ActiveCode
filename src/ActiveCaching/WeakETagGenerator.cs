// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace ActiveCaching
{
	public class WeakETagGenerator : IETagGenerator
	{
		public string GenerateFromBuffer(ReadOnlySpan<byte> buffer)
		{
			using var md5 = MD5.Create();
			var hash = new byte[6];
			var hashed = md5.TryComputeHash(buffer, hash, out _);
			Debug.Assert(hashed);
			var hex = BitConverter.ToString(hash);
			return $"W/\"{hex.Replace("-", "")}\"";
		}
	}
}