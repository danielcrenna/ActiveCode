// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ActiveConnection.Tests
{
	public class DatabaseFixture : IDisposable
	{
		public void Dispose()
		{
		}

		public string CreateConnectionString()
		{
			return $"Data Source={Guid.NewGuid()}.sqdb;Mode=ReadWriteCreate;";
		}
	}
}