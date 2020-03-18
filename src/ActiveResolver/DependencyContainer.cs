// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TypeKitchen.Creation;

namespace ActiveResolver
{
    public class DependencyContainer
    {
        private const string EnumerableSlot = "__ENUMERABLE_SLOT__";

        private readonly ConcurrentDictionary<NameAndType, List<Func<object>>> _registrations;

        public DependencyContainer()
        {
            _registrations = new ConcurrentDictionary<NameAndType, List<Func<object>>>();
        }

        public DependencyContainer Register(string name, Type type, Func<object> builder, Func<Func<object>, Func<object>> memoFunc = null)
        {
            var cacheKey = new NameAndType(name, type);
			
            var next = memoFunc == null ? builder : memoFunc(builder);

            if (!_registrations.TryGetValue(cacheKey, out var list))
                _registrations.TryAdd(cacheKey, list = new List<Func<object>>());

            list.Add(next);
			
            var enumerableCacheKey = new NameAndType($"{EnumerableSlot}{name}", typeof(IEnumerable<>).MakeGenericType(type));
			
            if (_registrations.TryGetValue(enumerableCacheKey, out var enumerableList))
                return this;
			
            if(_registrations.TryAdd(enumerableCacheKey, enumerableList = new List<Func<object>>()))
            {
                enumerableList.Add(() =>
                {
                    var collection = (IList) Instancing.CreateInstance(typeof(List<>).MakeGenericType(type));
                    foreach (var itemBuilder in list)
                        collection.Add(itemBuilder());
                    return collection;
                });
            }

            return this;
        }
		
        public bool TryResolve(string name, Type type, out object instance)
        {
            var cacheKey = new NameAndType(name, type);

            if (typeof(IEnumerable).IsAssignableFrom(type))
            {
                var enumerableCacheKey = new NameAndType($"{EnumerableSlot}{name}", typeof(IEnumerable<>).MakeGenericType(type.GetGenericArguments()));
                if(_registrations.TryGetValue(enumerableCacheKey, out var enumerableList))
                {
                    foreach (var entry in enumerableList)
                    {
                        instance = entry();

                        if (!_registrations.ContainsKey(cacheKey))
                            return true; // nothing to merge

                        var merged = (IList) Instancing.CreateInstance(typeof(List<>).MakeGenericType(type.GetGenericArguments()));
                        foreach (var x in (IList) instance)
                            merged.Add(x);

                        if (_registrations.TryGetValue(cacheKey, out var singleList))
                        {
                            foreach (var y in singleList)
                            {
                                foreach (var x in (IList) y())
                                    merged.Add(x);

                                break;
                            }
                        }

                        instance = merged;
                        return true;
                    }
                }
            }

            if (_registrations.TryGetValue(cacheKey, out var list))
            {
                foreach (var entry in list)
                {
                    instance = entry();
                    return true;
                }
            }

            instance = default;
            return false;
        }

        public struct NameAndType : IEquatable<NameAndType>
        {
            public readonly Type Type;
            public readonly string Name;

            public NameAndType(string name, Type type)
            {
                Name = name;
                Type = type;
            }

            public bool Equals(NameAndType other) => Type == other.Type && Name == other.Name;
			public override bool Equals(object obj) => obj is NameAndType other && Equals(other);

            public static bool operator ==(NameAndType left, NameAndType right) => left.Equals(right);
            public static bool operator !=(NameAndType left, NameAndType right) => !left.Equals(right);

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Type != null ? Type.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                }
            }
        }
    }
}