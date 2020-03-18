// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Collections.Generic;

namespace ActiveResolver
{
    public class DependencyContainer
    {
        private readonly ConcurrentDictionary<NameAndType, List<Func<object>>> _registrations;

        public DependencyContainer()
        {
            _registrations = new ConcurrentDictionary<NameAndType, List<Func<object>>>();
        }

        public DependencyContainer Register(string name, Type type, Func<object> builder)
        {
            var cacheKey = new NameAndType(name, type);
            if (!_registrations.TryGetValue(cacheKey, out var list))
            {
                list = new List<Func<object>>();
                _registrations.TryAdd(cacheKey, list);
            }

            list.Add(builder);
            return this;
        }

        public bool TryResolve(string name, Type type, out object instance)
        {
            var cacheKey = new NameAndType(name, type);
            if (_registrations.TryGetValue(cacheKey, out var list))
                foreach (var entry in list)
                {
                    instance = entry();
                    return true;
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

            public bool Equals(NameAndType other)
            {
                return Type == other.Type && Name == other.Name;
            }

            public override bool Equals(object obj)
            {
                return obj is NameAndType other && Equals(other);
            }

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((Type != null ? Type.GetHashCode() : 0) * 397) ^ (Name != null ? Name.GetHashCode() : 0);
                }
            }

            public static bool operator ==(NameAndType left, NameAndType right)
            {
                return left.Equals(right);
            }

            public static bool operator !=(NameAndType left, NameAndType right)
            {
                return !left.Equals(right);
            }

            private sealed class TypeNameEqualityComparer : IEqualityComparer<NameAndType>
            {
                public bool Equals(NameAndType x, NameAndType y)
                {
                    return x.Type == y.Type && x.Name == y.Name;
                }

                public int GetHashCode(NameAndType obj)
                {
                    unchecked
                    {
                        return ((obj.Type != null ? obj.Type.GetHashCode() : 0) * 397) ^
                               (obj.Name != null ? obj.Name.GetHashCode() : 0);
                    }
                }
            }
        }
    }
}