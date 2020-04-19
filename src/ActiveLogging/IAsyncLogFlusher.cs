// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Threading;
using System.Threading.Tasks;

namespace ActiveLogging
{
	public interface IAsyncLogFlusher
	{
		Task BeginAsync(CancellationToken cancellationToken = default);
		Task Add<T>(T entry, CancellationToken cancellationToken = default);
		Task EndAsync(CancellationToken cancellationToken = default);
	}
}