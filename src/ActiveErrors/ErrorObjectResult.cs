// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using Microsoft.AspNetCore.Mvc;

namespace ActiveErrors
{
	public class ErrorObjectResult : ObjectResult
	{
		public ErrorObjectResult(Error error, params object[] args) : base(error)
		{
			if (args.Length > 0)
				error = new Error(error.EventId, string.Format(error.Message, args),
					error.StatusCode == default ? (short) 500 : error.StatusCode);
			Value = error;
			StatusCode = error.StatusCode;
		}
	}
}