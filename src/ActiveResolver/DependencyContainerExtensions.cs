// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;

namespace ActiveResolver
{
    public static class DependencyContainerExtensions
    {
        private const string DefaultName = "__DEFAULT__";

        public static DependencyContainer Register(this DependencyContainer container, Type type, Func<object> builder)
        {
            container.Register(DefaultName, type, builder);
            return container;
        }

        public static bool TryResolve(this DependencyContainer container, Type type, out object instance)
        {
            return container.TryResolve(DefaultName, type, out instance);
        }
    }
}