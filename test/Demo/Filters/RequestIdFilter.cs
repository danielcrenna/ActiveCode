// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Demo.Filters
{
	public sealed class RequestIdFilter : IActionFilter
	{
		private const string RequestIdKey = "requestId";

		public void OnActionExecuting(ActionExecutingContext context)
		{
			if (!context.ActionArguments.TryGetValue(RequestIdKey, out var requestId) || requestId != null)
				return;
			context.ActionArguments[RequestIdKey] = ResolveRequestId(context);
		}

		private static string ResolveRequestId(ActionContext context)
		{
			var activityId = Activity.Current?.Id;
			if (!string.IsNullOrWhiteSpace(activityId))
				return activityId;

			var traceIdentifier = context.HttpContext.TraceIdentifier;
			if (!string.IsNullOrWhiteSpace(traceIdentifier))
				return traceIdentifier;

			if(context.HttpContext.Items[RequestIdKey] == null)
				context.HttpContext.Items[RequestIdKey] = $"{Guid.NewGuid()}";

			return context.HttpContext.Items[RequestIdKey]?.ToString();
		}

		public void OnActionExecuted(ActionExecutedContext context) { }
	}
}