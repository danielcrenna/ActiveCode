// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using ActiveResolver.Api;
using ActiveResolver.Tests.Fakes;

namespace ActiveResolver.Tests
{
	public class GraphVizOutputFormatterTests
	{
		public bool Can_generate_topology_graph()
		{
			var a = new Node("A");
			var b = new Node("B");
			var c = new Node("C");

			// C -> B -> A
			a.DependsOn(b);
			b.DependsOn(c);

			var topology = new Topology();
			
			topology.FirstRoot.Nodes.Add(a);
			topology.FirstRoot.Nodes.Add(b);
			topology.FirstRoot.Nodes.Add(c);

			var d = new D("D");
			var e = new E("E");
			var f = new F("F");

			// F -> E -> D
			d.DependsOn(e);
			e.DependsOn(f);

			d.DependsOn(a);
			b.DependsOn(e);
			a.DependsOn(f);

			topology.SecondRoot.Nodes.Add(d);
			topology.SecondRoot.Nodes.Add(e);
			topology.SecondRoot.Nodes.Add(f);

			var dotGraph = GraphVizOutputFormatter.GenerateDotGraph(GraphDirection.TopToBottom, topology);

			Trace.WriteLine(dotGraph);
			return dotGraph != null;
		}

		[Order(2)]
		[Shape(Shape.Star)]
		[Color(nameof(Color.Purple)), DisplayName("Dope")]
		public class D : Node
		{
			public D(string id) : base(id) { }
		}

		[Order(3)]
		[Shape(Shape.TripleOctagon)]
		[Color(nameof(Color.MediumPurple)), DisplayName("Exciting")]
		public class E : Node
		{
			public E(string id) : base(id) { }
		}

		[Order(1)]
		[Shape(Shape.Diamond), Color(nameof(Color.Plum)), DisplayName("Funky")]
		public class F : Node
		{
			public F(string id) : base(id) { }
		}

		[DisplayName("My Test Graph")]
		public class Topology
		{
			[TopologyRoot, Order(1), DisplayName("Boring")]
			public Graph FirstRoot { get; set; } = new Graph();

			[TopologyRoot, Order(2), DisplayName("Cool"), Color(nameof(Color.DarkOrange))]
			public Graph SecondRoot { get; set; } = new Graph();
		}
	}
}