// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;

namespace ActiveResolver
{
    public static class InstanceIsUnique
    {
        public static Func<T> PerProcess<T>(Func<T> f)
        {
            var cache = new ConcurrentDictionary<Type, T>();

            return () => cache.GetOrAdd(typeof(T), v => f());
        }
    }
}