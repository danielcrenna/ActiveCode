// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using System.Security.Cryptography;

namespace ActiveCaching
{
	/// <summary>
	///     See: https://tools.ietf.org/html/rfc7232#section-2
	/// </summary>
	public interface IETagGenerator
	{
		string GenerateFromBuffer(ReadOnlySpan<byte> data);
	}

	public class WeakETagGenerator : IETagGenerator
	{
		public string GenerateFromBuffer(ReadOnlySpan<byte> buffer)
		{
			using var md5 = MD5.Create();
			var hash = new byte[6];
			Debug.Assert(md5.TryComputeHash(buffer, hash, out _));
			var hex = BitConverter.ToString(hash);
			return $"W/\"{hex.Replace("-", "")}\"";
		}
	}
}