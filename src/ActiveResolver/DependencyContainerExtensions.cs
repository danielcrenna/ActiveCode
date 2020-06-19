// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace ActiveResolver
{
    public static class DependencyContainerExtensions
    {
        public static bool CanResolve(this DependencyContainer container, string name, Type type) => container.TryResolve(name, type, out _);
        public static bool CanResolve(this DependencyContainer container, Type type) => container.TryResolve(Constants.DefaultName, type, out _);
        public static bool CanResolve<T>(this DependencyContainer container, string name) => container.TryResolve<T>(name, out _);
        public static bool CanResolve<T>(this DependencyContainer container) => container.TryResolve<T>(Constants.DefaultName, out _);

        public static T Resolve<T>(this DependencyContainer container, string name) => (T) Resolve(container, name, typeof(T));
        public static T Resolve<T>(this DependencyContainer container) => (T) Resolve(container, Constants.DefaultName, typeof(T));
        public static object Resolve(this DependencyContainer container, Type type) => Resolve(container, Constants.DefaultName, type);
        public static object Resolve(this DependencyContainer container, string name, Type type)
        {
            if (!container.TryResolve(name, type, out var instance))
                throw new InvalidOperationException($"No registration for '{(name == Constants.DefaultName ? string.Empty : $"[{name}] ")}{type.FullName}'");

            return instance;
        }

        public static DependencyContainer Register(this DependencyContainer container, Type type, Func<object> builder, Func<Func<object>, Func<object>> memoFunc = null) => Register(container, Constants.DefaultName, type, builder, memoFunc);
        public static DependencyContainer Register(this DependencyContainer container, string name, Type type, Func<object> builder, Func<Func<object>, Func<object>> memoFunc = null)
        {
            if (memoFunc == null)
                return container.Register(name, type, builder);

            var memo = memoFunc(builder);
            object Memo() => memo();
            return container.Register(name, type, Memo);
        }

        public static DependencyContainer Register<T>(this DependencyContainer container, Func<T> builder, Func<Func<T>, Func<T>> memoFunc = null) => Register(container, Constants.DefaultName, builder, memoFunc);
        public static DependencyContainer Register<T>(this DependencyContainer container, string name, Func<T> builder, Func<Func<T>, Func<T>> memoFunc = null)
        {
            if (memoFunc == null)
                return Register(container, name, typeof(T), () => builder());

            var memo = memoFunc(builder);
            object Memo() => memo();
            return container.Register(name, typeof(T), Memo);
        }
		
        public static bool TryResolve(this DependencyContainer container, Type type, out object instance) => container.TryResolve(Constants.DefaultName, type, out instance);
		public static bool TryResolve<T>(this DependencyContainer container, out T instance) => TryResolve(container, Constants.DefaultName, out instance);
		public static bool TryResolve<T>(this DependencyContainer container, string name, out T instance)
        {
            if (!container.TryResolve(name, typeof(T), out var untyped))
            {
                instance = default;
                return false;
            }

            if (untyped is T typed)
            {
                instance = typed;
                return true;
            }

            instance = default;
            return false;
        }

        public static DependencyContainer Register<T>(this DependencyContainer container, Func<DependencyContainer, T> builder) => Register(container, Constants.DefaultName, builder);
        public static DependencyContainer Register<T>(this DependencyContainer container, string name, Func<DependencyContainer, T> builder) => container.Register(name, typeof(T), () => builder(container));
        public static DependencyContainer Register<T>(this DependencyContainer container, T instance) => Register(container, Constants.DefaultName, instance);
        public static DependencyContainer Register<T>(this DependencyContainer container, string name, T instance) => container.Register(name, typeof(T), () => instance);
		
        public static DependencyContainer Register(this DependencyContainer container, Type type, Func<DependencyContainer, object> builder) => Register(container, Constants.DefaultName, type, builder);
        public static DependencyContainer Register(this DependencyContainer container, string name, Type type, Func<DependencyContainer, object> builder) => container.Register(name, type, () => builder(container));
        public static DependencyContainer Register(this DependencyContainer container, object instance) => Register(container, Constants.DefaultName, instance);
        public static DependencyContainer Register(this DependencyContainer container, string name, object instance) => container.Register(name, instance.GetType(), () => instance);

		public static DependencyContainer Register<T>(this DependencyContainer container, Func<DependencyContainer, T> builder, Func<Func<DependencyContainer,T>, Func<DependencyContainer,T>> memoFunc) => Register(container, Constants.DefaultName, builder, memoFunc);
        public static DependencyContainer Register<T>(this DependencyContainer container, string name, Func<DependencyContainer, T> builder, Func<Func<DependencyContainer,T>, Func<DependencyContainer,T>> memoFunc)
        {
            var memo = memoFunc(builder);
            object Memo() => memo(container);
            return container.Register(name, typeof(T), Memo);
        }

        public static IEnumerable<T> ResolveAll<T>(this DependencyContainer container) => ResolveAll<T>(container, Constants.DefaultName);
        public static IEnumerable<T> ResolveAll<T>(this DependencyContainer container, string name)
        {
            container.TryResolve<IEnumerable<T>>(name, out var enumerable);
            return enumerable ?? Enumerable.Empty<T>();
        }

        public static IEnumerable<object> ResolveAll(this DependencyContainer container, Type type) => ResolveAll(container, Constants.DefaultName, type);
        public static IEnumerable<object> ResolveAll(this DependencyContainer container, string name, Type type)
        {
            container.TryResolve(name, typeof(IEnumerable<>).MakeGenericType(type), out var enumerable);
            return enumerable as IEnumerable<object>;
        }
    }
}