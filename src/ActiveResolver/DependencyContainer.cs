// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using TypeKitchen.Creation;

namespace ActiveResolver
{
    public class DependencyContainer : IServiceProvider
    {
        private readonly IServiceProvider _fallback;
        private readonly ConcurrentDictionary<NameAndType, List<Func<object>>> _registrations;

        public DependencyContainer(IServiceProvider fallback = null)
        {
            _fallback = fallback;
            _registrations = new ConcurrentDictionary<NameAndType, List<Func<object>>>();
        }

        public DependencyContainer Register(string name, Type type, Func<object> builder)
        {
            var cacheKey = new NameAndType(name, type);

            if (!_registrations.TryGetValue(cacheKey, out var list))
                _registrations.TryAdd(cacheKey, list = new List<Func<object>>());

            list.Add(builder);
			
            var enumerableCacheKey = new NameAndType($"{Constants.EnumerableSlot}{name}", typeof(IEnumerable<>).MakeGenericType(type));
			
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
                var enumerableCacheKey = new NameAndType($"{Constants.EnumerableSlot}{name}", typeof(IEnumerable<>).MakeGenericType(type.GetGenericArguments()));
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

            var fallback = _fallback?.GetService(type);
            if (fallback != null)
            {
                instance = fallback;
                return true;
            }

            instance = default;
            return false;
        }

        #region IServiceProvider

        public object GetService(Type serviceType)
        {
            TryResolve(Constants.DefaultName, serviceType, out var instance);
            return instance;
        }

		#endregion

        private struct NameAndType : IEquatable<NameAndType>
        {
            private readonly Type _type;
            private readonly string _name;

            public NameAndType(string name, Type type)
            {
                _name = name;
                _type = type;
            }

            public bool Equals(NameAndType other) => _type == other._type && _name == other._name;
            public override bool Equals(object obj) => obj is NameAndType other && Equals(other);

            public static bool operator ==(NameAndType left, NameAndType right) => left.Equals(right);
            public static bool operator !=(NameAndType left, NameAndType right) => !left.Equals(right);

            public override int GetHashCode()
            {
                unchecked
                {
                    return ((_type != null ? _type.GetHashCode() : 0) * 397) ^ (_name != null ? _name.GetHashCode() : 0);
                }
            }
        }
    }
}