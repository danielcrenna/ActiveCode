// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Net;

namespace ActiveErrors
{
	public sealed class ErrorAndObjectResult<T> : ErrorObjectResult
	{
		public ErrorAndObjectResult(T data, Error error, HttpStatusCode statusCode = HttpStatusCode.OK, params object[] arguments) : base(error, arguments)
		{
			Value = new {data, error};
			StatusCode = (int?) statusCode;
		}
	}
}