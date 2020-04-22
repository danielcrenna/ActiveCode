// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.Filters
{
	public sealed class CancellationTokenFilter : IActionFilter
	{
		private const string CancellationTokenKey = "cancellationToken";

		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ActionArguments.TryGetValue(CancellationTokenKey, out var token) || token != null)
				return;
			context.ActionArguments[CancellationTokenKey] = context.HttpContext.RequestAborted;
		}

		public void OnActionExecuted(ActionExecutedContext context) { }
	}
}