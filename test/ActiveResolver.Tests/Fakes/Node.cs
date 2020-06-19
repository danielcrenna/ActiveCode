using System;
using System.Collections.Generic;

namespace ActiveResolver.Tests.Fakes
{
	public class Node : INode<string>, IEquatable<Node>
	{
		public Node(string id)
		{
			Id = id;
		}

		public string Id { get; }

		public void DependsOn(INode<string> node)
		{
			_nodes.Add(node);
		}

		private readonly List<INode<string>> _nodes = new List<INode<string>>();
		public IEnumerable<INode<string>> Dependents => _nodes;

		public bool Equals(Node other)
		{
			return !ReferenceEquals(null, other) && (ReferenceEquals(this, other) || string.Equals(Id, other.Id));
		}

		public bool Equals(INode<string> other)
		{
			return Id == other?.Id;
		}

		public override bool Equals(object obj)
		{
			return ReferenceEquals(this, obj) || obj is Node other && Equals(other);
		}

		public override int GetHashCode()
		{
			return (Id != null ? Id.GetHashCode() : 0);
		}

		public static bool operator ==(Node left, Node right)
		{
			return Equals(left, right);
		}

		public static bool operator !=(Node left, Node right)
		{
			return !Equals(left, right);
		}
	}
}