// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ActiveResolver
{
    public static class DependencyContainerExtensions
    {
        public static DependencyContainer Register(this DependencyContainer container, Type type, Func<object> builder, Func<Func<object>, Func<object>> memoFunc = null) => container.Register(Constants.DefaultName, type, builder, memoFunc);
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
    }
}