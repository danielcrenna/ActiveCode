// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Xunit;

namespace ActiveResolver.Tests
{
	public class TopologicalSortTests
	{
		[Fact]
		public void BasicTests_return_self_when_nothing_to_sort()
		{
			var set = new List<string>(new [] { "A", "B", "C" });
			var ordered = set.OrderByTopology(s => Enumerable.Empty<string>());
			Assert.Same(set, ordered.AsList);
		}

		[Fact]
		public void BasicTests_detect_cycle()
		{
			var set = new List<string>(new [] { "A", "B", "C" });

			// B depends on C
			// C depends on B
			Assert.Throws<InvalidOperationException>(() =>
			{
				set.OrderByTopology(x =>
				{
					switch (x)
					{
						case "B":
							return new[] {"C"};
						case "C":
							return new[] {"B"};
						default:
							return Enumerable.Empty<string>();
					}
				});
			}); // "expected a cycle"
		}

		[Fact]
		public void BasicTests_string_sort()
		{
			Assert.Equal("A", "A");

			var set = new List<string>(new [] { "A", "B", "C" });

			// B depends on C
			// C depends on A
			var ordered = set.OrderByTopology(x =>
			{
				switch (x)
				{
					case "C":
						return new[] {"B"};
					case "A":
						return new[] {"C"};
					default:
						return Enumerable.Empty<string>();
				}
			});

			var sb = new StringBuilder();
			foreach (var node in ordered)
				sb.Append(node);

			Assert.Equal("ACB", sb.ToString());
		}
	}
}