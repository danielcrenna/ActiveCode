// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Threading;

namespace ActiveResolver
{
    public static class InstanceIsUnique
    {
        public static Func<T> PerProcess<T>(Func<T> f)
        {
            var cache = new ConcurrentDictionary<Type, T>();

            return () => cache.GetOrAdd(typeof(T), v => f());
        }

        public static Func<DependencyContainer, T> PerProcess<T>(DependencyContainer host,
            Func<DependencyContainer, T> f)
        {
            var cache = new ConcurrentDictionary<Type, T>();

            return r => cache.GetOrAdd(typeof(T), v => f(host));
        }

        public static Func<T> PerThread<T>(Func<T> f)
        {
            var cache = new ThreadLocal<T>(f);

            return () => cache.Value;
        }

        public static Func<DependencyContainer, T> PerThread<T>(DependencyContainer host,
            Func<DependencyContainer, T> f)
        {
            var cache = new ThreadLocal<T>(() => f(host));

            return r => cache.Value;
        }

        //public static Func<T> PerHttpRequest<T>(DependencyContainer host, Func<T> f)
        //{
        //    return () =>
        //    {
        //        var accessor = host.Resolve<IHttpContextAccessor>();
        //        if (accessor?.HttpContext == null)
        //        {
        //            return f(); // always new
        //        }

        //        var cache = accessor.HttpContext.Items;
        //        var cacheKey = f.ToString();
        //        if (cache.TryGetValue(cacheKey, out var item))
        //        {
        //            return (T) item; // got it
        //        }

        //        item = f(); // need it
        //        cache.Add(cacheKey, item);
        //        if (item is IDisposable disposable)
        //        {
        //            accessor.HttpContext.Response.RegisterForDispose(disposable);
        //        }

        //        return (T) item;
        //    };
        //}

        //public static Func<DependencyContainer, T> PerHttpRequest<T>(DependencyContainer host, Func<DependencyContainer, T> f)
        //{
        //    return r =>
        //    {
        //        var accessor = r.Resolve<IHttpContextAccessor>();
        //        if (accessor?.HttpContext == null)
        //        {
        //            return f(host); // always new
        //        }

        //        var cache = accessor.HttpContext.Items;
        //        var cacheKey = f.ToString();
        //        if (cache.TryGetValue(cacheKey, out var item))
        //        {
        //            return (T) item; // got it
        //        }

        //        item = f(host); // need it
        //        cache.Add(cacheKey, item);
        //        if (item is IDisposable disposable)
        //        {
        //            accessor.HttpContext.Response.RegisterForDispose(disposable);
        //        }

        //        return (T) item;
        //    };
        //}
    }
}