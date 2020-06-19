// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Data;
using Microsoft.Data.Sqlite;

namespace ActiveConnection.Tests
{
	public class SqliteConnectionFactory : ConnectionFactory
	{
		public override IDbConnection CreateConnection() => new SqliteConnection(ConnectionString);
	}
}