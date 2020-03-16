// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ActiveCaching
{
	public interface ICacheSet
	{
		bool Set(string key, object value);
		bool Set(string key, object value, DateTimeOffset absoluteExpiration);
		bool Set(string key, object value, TimeSpan slidingExpiration);
		bool Set(string key, object value, ICacheDependency dependency);
		bool Set(string key, object value, DateTimeOffset absoluteExpiration, ICacheDependency dependency);
		bool Set(string key, object value, TimeSpan slidingExpiration, ICacheDependency dependency);

		bool Set<T>(string key, T value);
		bool Set<T>(string key, T value, DateTimeOffset absoluteExpiration);
		bool Set<T>(string key, T value, TimeSpan slidingExpiration);
		bool Set<T>(string key, T value, ICacheDependency dependency);
		bool Set<T>(string key, T value, DateTimeOffset absoluteExpiration, ICacheDependency dependency);
		bool Set<T>(string key, T value, TimeSpan slidingExpiration, ICacheDependency dependency);
	}
}