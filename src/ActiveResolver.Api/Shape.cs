// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

namespace ActiveResolver.Api
{
	/// <summary>
	///     See: https://www.graphviz.org/doc/info/shapes.html
	/// </summary>
	public enum Shape : byte
	{
		None,
		Box,
		Polygon,
		Ellipse,
		Oval,
		Circle,
		Point,
		Egg,
		Triangle,
		Plain,
		Diamond,
		Trapezium,
		Parallelogram,
		House,
		Pentagon,
		Hexagon,
		Septagon,
		Octagon,
		DoubleCircle,
		DoubleOctagon,
		TripleOctagon,
		InvTriangle,
		InvTrapezium,
		InvHouse,
		MDiamond,
		MSquare,
		MCircle,
		Square,
		Star,
		Underline,
		Cylinder,
		Note,
		Tab,
		Folder,
		Box3d,
		Component,
		Promoter,
		Cds,
		Terminator,
		Utr,
		PrimerSite,
		RestrictionSite,
		FivePOverhang,
		ThreePOverhang,
		NOverhang,
		Assembly,
		Signature,
		Insulator,
		RiboSite,
		RnaStab,
		ProteaseSite,
		ProteinStab,
		RPromoter,
		RArrow,
		LArrow,
		LPromoter,

		Rectangle = Box,
		Rect = Rectangle,
		Plaintext = None
	}
}