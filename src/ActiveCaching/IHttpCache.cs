// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ActiveCaching
{
	public interface IHttpCache
	{
		bool TryGetETag(string key, out string etag);
		bool TryGetLastModified(string key, out DateTimeOffset lastModified);
		void Save(string key, string etag);
		void Save(string key, DateTimeOffset lastModified);
	}
}