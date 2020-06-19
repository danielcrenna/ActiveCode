// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Net;
using Microsoft.AspNetCore.Mvc;

namespace ActiveErrors
{
	public static class OperationExtensions
	{
		public static IActionResult ToResult(this Operation operation)
		{
			switch (operation.Result)
			{
				case OperationResult.Succeeded:
					return new OkResult();
				case OperationResult.Refused:
					return new ForbidResult();

				case OperationResult.Error:
				case OperationResult.SucceededWithErrors:
				{
					var errors = operation.Errors.Count > 1
						? new Error(ErrorEvents.AggregateErrors, ErrorStrings.AggregateErrors,
							HttpStatusCode.InternalServerError, operation.Errors)
						: operation.Errors[0];

					var error = new ErrorObjectResult(errors);
					if (operation.Succeeded)
						error.StatusCode = 200;

					return error;
				}

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		public static IActionResult ToResult<T>(this Operation<T> operation,
			Func<object, IActionResult> inner = null,
			HttpStatusCode forbidStatusCode = HttpStatusCode.Forbidden)
		{
			switch (operation.Result)
			{
				case OperationResult.Refused:
					switch (forbidStatusCode)
					{
						case HttpStatusCode.Unauthorized:
							return new UnauthorizedObjectResult(AggregateErrors(operation));
					}

					return new ForbidResult( /* Forbid does not permit a body */);
				case OperationResult.Succeeded:
					return inner == null ? new OkObjectResult(operation.Data) : inner.Invoke(operation.Data);
			}

			if (operation.Data == null)
				return new NotFoundObjectResult(new ErrorObjectResult(
					new Error(ErrorEvents.ResourceMissing, "Resource not found.", HttpStatusCode.NotFound,
						operation.Errors)
				));

			var error = AggregateErrors(operation);

			if (operation.Result == OperationResult.Error) return new ErrorObjectResult(error);

			return inner != null ? inner(new {operation.Data, Errors = error}) :
				operation.Succeeded ? new ErrorAndObjectResult<T>(operation.Data, error) : new ErrorObjectResult(error);
		}
		
		private static Error AggregateErrors(Operation operation)
		{
			var error = operation.HasErrors
				? new Error(ErrorEvents.AggregateErrors, ErrorStrings.AggregateErrors,
					HttpStatusCode.InternalServerError, operation.Errors)
				: operation.Errors[0];
			return error;
		}
	}
}