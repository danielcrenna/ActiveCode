// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using Microsoft.Extensions.Primitives;

namespace ActiveCaching
{
	public interface ICacheDependency : IDisposable
	{
		IChangeToken GetChangeToken();
	}
}