// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace ActiveErrors
{
	public static class StatusCodeErrorExtensions
	{
		public static Task<IActionResult> StatusCodeErrorAsync(this ControllerBase controller, long eventId,
			string errorMessage, HttpStatusCode statusCode, params object[] args)
		{
			return Task.FromResult(
				(IActionResult) new ErrorObjectResult(new Error(eventId, errorMessage, statusCode), args));
		}

		public static Task<IActionResult> BadRequestErrorAsync(this ControllerBase controller, long eventId,
			string errorMessage, params object[] args)
		{
			return controller.StatusCodeErrorAsync(eventId, errorMessage, HttpStatusCode.BadRequest, args);
		}

		public static Task<IActionResult> NotAcceptableErrorAsync(this ControllerBase controller, long eventId,
			string errorMessage, params object[] args)
		{
			return controller.StatusCodeErrorAsync(eventId, errorMessage, HttpStatusCode.NotAcceptable, args);
		}

		public static Task<IActionResult> NotFoundErrorAsync(this ControllerBase controller, long eventId,
			string errorMessage, params object[] args)
		{
			return controller.StatusCodeErrorAsync(eventId, errorMessage, HttpStatusCode.NotFound, args);
		}

		public static Task<IActionResult> InternalServerErrorAsync(this ControllerBase controller, long eventId,
			string errorMessage, params object[] args)
		{
			return controller.StatusCodeErrorAsync(eventId, errorMessage, HttpStatusCode.InternalServerError, args);
		}

		public static Task<IActionResult> UnprocessableEntityErrorAsync(this ControllerBase controller, long eventId,
			string errorMessage, params object[] args)
		{
			return controller.StatusCodeErrorAsync(eventId, errorMessage, HttpStatusCode.UnprocessableEntity, args);
		}
	}
}