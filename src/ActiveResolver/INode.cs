using System;
using System.Collections.Generic;

namespace ActiveResolver
{
	public interface INode<TKey> : IEquatable<INode<TKey>> where TKey : IEquatable<TKey>
	{
		TKey Id { get; }
		IEnumerable<INode<TKey>> Dependents { get; }
	}
}