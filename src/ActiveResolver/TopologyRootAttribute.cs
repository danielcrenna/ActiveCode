using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Linq;
using System.Reflection;
using TypeKitchen;

namespace ActiveResolver
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Property)]
	public class TopologyRootAttribute : ValidationAttribute
	{
		public TopologyRootAttribute() => ErrorMessage = "{0} has at least one cycle.";

		protected override ValidationResult IsValid(object value, ValidationContext validationContext)
		{
			var accessor = ReadAccessor.Create(value, AccessorMemberTypes.Properties, AccessorMemberScope.Public,
				out var members);

			var nodes = new HashSet<INode<string>>();
			var visited = new HashSet<object>();
			WalkGraph(visited, value, members, accessor, nodes);

			var cycles = nodes.HasCycles(node => node.Dependents);

			return cycles
				? new ValidationResult(
					string.Format(CultureInfo.CurrentCulture, ErrorMessageString, accessor.Type.Name))
				: ValidationResult.Success;
		}

		private static void WalkGraph(ISet<object> visited, object value, AccessorMembers members, ITypeReadAccessor accessor, ISet<INode<string>> nodes)
		{
			if (value is INode<string> node)
			{
				nodes.Add(node);
				visited.Add(node);
			}

			foreach (var member in members)
			{
				var memberTypeInfo = member.Type.GetTypeInfo();

				if (memberTypeInfo.ImplementedInterfaces.Contains(typeof(INode<string>)) &&
				    accessor.TryGetValue(value, member.Name, out var element) && element is INode<string> memberNode)
					nodes.Add(memberNode);

				if (!memberTypeInfo.ImplementedInterfaces.Contains(typeof(IEnumerable)) ||
				    !accessor.TryGetValue(value, member.Name, out var collection) ||
				    !(collection is IEnumerable enumerable))
					continue;

				if (visited.Contains(enumerable))
					continue;

				visited.Add(enumerable);

				foreach (var item in enumerable)
				{
					var collectionAccessor = ReadAccessor.Create(item, AccessorMemberTypes.Properties,
						AccessorMemberScope.Public, out var collectionMembers);
					WalkGraph(visited, item, collectionMembers, collectionAccessor, nodes);
				}
			}
		}
	}
}