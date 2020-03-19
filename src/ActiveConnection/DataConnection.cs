// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Data;

namespace ActiveConnection
{
	public class DataConnection<TOwner> : DataConnection, IDataConnection<TOwner>
	{
		public DataConnection(DataContext current) : base(current) { }
	}

	public class DataConnection : IDataConnection
	{
		private readonly DataContext _current;

		private volatile IDbTransaction _transaction;

		public DataConnection(DataContext current)
		{
			_current = current;
		}

		public IDbConnection Current => _current.Connection;
		public IDbTransaction Transaction => _transaction;

		public void SetTransaction(IDbTransaction transaction)
		{
			_transaction = transaction;
		}
	}
}