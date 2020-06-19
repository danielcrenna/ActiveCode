// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ActiveResolver.Api
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public class ShapeAttribute : Attribute
	{
		public ShapeAttribute(Shape shape) => Shape = shape;
		public Shape Shape { get; set; }
		public string Name => Enum.GetName(typeof(Shape), (byte) Shape);
	}
}