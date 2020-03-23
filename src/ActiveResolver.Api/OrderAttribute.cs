// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ActiveResolver.Api
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public class OrderAttribute : Attribute
	{
		public OrderAttribute(int order) => Order = order;

		public int Order { get; set; }
	}
}