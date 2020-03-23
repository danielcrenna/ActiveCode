// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Drawing;

namespace ActiveResolver.Api
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public class ColorAttribute : Attribute
	{
		private static readonly Color UnknownColor = Color.CornflowerBlue;

		public ColorAttribute(string nameOrHexColor)
		{
			var color = Color.FromName(nameOrHexColor);
			if (!color.IsNamedColor)
				color = ColorExtensions.FromHex(nameOrHexColor);
			if (color == Color.Empty)
				color = UnknownColor;

			Color = color;
		}

		public ColorAttribute(uint argb)
		{
			var color = Color.FromArgb((int) argb);
			if (color.IsEmpty)
				color = UnknownColor;

			Color = color;
		}

		public ColorAttribute(int argb)
		{
			var color = Color.FromArgb(argb);
			if (color.IsEmpty)
				color = UnknownColor;

			Color = color;
		}

		public ColorAttribute(int r, int g, int b)
		{
			var color = Color.FromArgb(r, g, b);
			if (Color.IsEmpty)
				color = UnknownColor;

			Color = color;
		}

		public ColorAttribute(int a, int r, int g, int b)
		{
			var color = Color.FromArgb(a, r, g, b);
			if (color.IsEmpty)
				color = UnknownColor;

			Color = color;
		}

		public Color Color { get; set; }
	}
}