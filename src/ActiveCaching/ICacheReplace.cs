// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ActiveCaching
{
	public interface ICacheReplace
	{
		bool Replace(string key, object value);
		bool Replace(string key, object value, DateTimeOffset absoluteExpiration);
		bool Replace(string key, object value, TimeSpan slidingExpiration);
		bool Replace(string key, object value, ICacheDependency dependency);
		bool Replace(string key, object value, DateTimeOffset absoluteExpiration, ICacheDependency dependency);
		bool Replace(string key, object value, TimeSpan slidingExpiration, ICacheDependency dependency);

		bool Replace<T>(string key, T value);
		bool Replace<T>(string key, T value, DateTimeOffset absoluteExpiration);
		bool Replace<T>(string key, T value, TimeSpan slidingExpiration);
		bool Replace<T>(string key, T value, ICacheDependency dependency);
		bool Replace<T>(string key, T value, DateTimeOffset absoluteExpiration, ICacheDependency dependency);
		bool Replace<T>(string key, T value, TimeSpan slidingExpiration, ICacheDependency dependency);
	}
}