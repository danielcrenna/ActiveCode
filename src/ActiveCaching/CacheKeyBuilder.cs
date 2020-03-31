// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using TypeKitchen;

namespace ActiveCaching
{
	public sealed class CacheKeyBuilder
	{
		private readonly ActionContext _context;

		public CacheKeyBuilder(ActionContext context) => _context = context;

		public string Build()
		{
			return Pooling.StringBuilderPool.Scoped(sb =>
			{
				foreach (var item in _context.RouteData.DataTokens)
				{
					sb.Append(item.Key)
						.Append(":")
						.Append(item.Value);
				}

				sb.Append(":");
				sb.Append(_context.HttpContext.Request.QueryString);
			});
		}
	}
}