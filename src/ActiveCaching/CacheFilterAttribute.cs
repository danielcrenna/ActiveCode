// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace ActiveCaching
{
	public sealed class CacheFilterAttribute : ActionFilterAttribute
	{
		private readonly ICache _cache;

		public CacheFilterAttribute(ICache cache)
		{
			_cache = cache;
			Order = int.MaxValue;
		}

		public override void OnActionExecuting(ActionExecutingContext filterContext)
		{
			var http = filterContext.HttpContext;
			if (!http.Items.TryGetValue(Constants.ContextKeys.CacheKeyArgument, out var cacheKey))
			{
				var keyBuilder = new CacheKeyBuilder(filterContext);
				cacheKey = keyBuilder.Build();
			}

			if (filterContext.ActionArguments.ContainsKey(Constants.ContextKeys.CacheArgument))
			{
				filterContext.ActionArguments[Constants.ContextKeys.CacheArgument] =
					http.RequestServices.GetService(typeof(ICache));
			}

			if (filterContext.ActionArguments.ContainsKey(Constants.ContextKeys.CacheKeyArgument))
			{
				filterContext.ActionArguments[Constants.ContextKeys.CacheKeyArgument] = cacheKey;
			}

			var existing = _cache.Get(cacheKey.ToString());
			if (existing is IActionResult result)
			{
				filterContext.Result = result;
				return;
			}

			http.Items.Add(Constants.ContextKeys.CacheKeyArgument, cacheKey);
			base.OnActionExecuting(filterContext);
		}

		public override void OnResultExecuted(ResultExecutedContext filterContext)
		{
			if (!(filterContext.HttpContext.Items[Constants.ContextKeys.CacheKeyArgument] is string key))
				return;

			var value = filterContext.Result;
			if (value != null)
				_cache.Set(key, value);

			base.OnResultExecuted(filterContext);
		}
	}
}