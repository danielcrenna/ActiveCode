// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ActiveResolver
{
    public static class DependencyContainerExtensions
    {
        private const string DefaultName = "__DEFAULT_SLOT__";
        public static DependencyContainer Register(this DependencyContainer container, Type type, Func<object> builder, Func<Func<object>, Func<object>> memoFunc = null) => container.Register(DefaultName, type, builder, memoFunc);
        public static bool TryResolve(this DependencyContainer container, Type type, out object instance) => container.TryResolve(DefaultName, type, out instance);
    }
}