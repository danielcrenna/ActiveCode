// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

namespace ActiveResolver.Tests
{
    public class RegisterTests
    {
        public bool Can_register_type_with_transient_lifetime()
        {
            var container = new DependencyContainer();
            container.Register("A", typeof(IFoo), () => new Bar());
            container.TryResolve<IFoo>("A", out var one);
            container.TryResolve("A", typeof(IFoo), out var two);

            return one != null && two != null && one != two && one is Bar && two is Bar;
        }

        public bool Can_register_type_with_singleton_lifetime()
        {
            var container = new DependencyContainer();
            container.Register<IFoo>("A", () => new Bar(), InstanceIsUnique.PerProcess);
            container.TryResolve<IFoo>("A", out var one);
            container.TryResolve<IFoo>("A", out var two);
            return one == two && one is Bar;
        }

        public bool Can_register_without_name()
        {
            var container = new DependencyContainer();
            container.Register<IFoo>(() => new Bar());
            container.TryResolve<IFoo>(out var one);
            container.TryResolve(typeof(IFoo), out var two);
            return one != null && two != null && one != two && one is Bar && two is Bar;
        }

        public bool Can_resolve_multiple_registrations_as_enumerable()
        {
            var container = new DependencyContainer();
            container.Register(typeof(IFoo), () => new Bar(), InstanceIsUnique.PerProcess);
            container.Register(typeof(IFoo), () => new Bar(), InstanceIsUnique.PerProcess);
            return container.TryResolve<IEnumerable<IFoo>>(out var instance) && 
                instance is List<IFoo> list && list.Count == 2 && list[0] != list[1];
        }

        public bool Can_resolve_single_enumerable_registration()
        {
            var container = new DependencyContainer();
            container.Register<IEnumerable<IFoo>>(() => new List<IFoo> { new Bar(), new Bar()}, InstanceIsUnique.PerProcess);
            return container.TryResolve<IEnumerable<IFoo>>(out var instance) && 
                   instance is List<IFoo> list && list.Count == 2 && list[0] != list[1];
        }

        public bool Can_resolve_single_and_multiple_enumerable_registrations_as_merged_list()
        {
            var container = new DependencyContainer();
            
            container.Register<IEnumerable<IFoo>>(() => new List<IFoo> { new Bar(), new Bar()}, InstanceIsUnique.PerProcess);
            container.Register<IFoo>(() => new Bar(), InstanceIsUnique.PerProcess);
            container.Register<IFoo>(() => new Bar(), InstanceIsUnique.PerProcess);

            return container.TryResolve<IEnumerable<IFoo>>(out var instance) && instance is List<IFoo> list &&
                   list.Count == 4 && list[0] != list[1] && list[1] != list[2] && list[2] != list[3];
        }

        public bool Can_resolve_through_service_provider()
        {
            var container = new DependencyContainer();
            container.Register(typeof(IFoo), () => new Bar());

            var serviceProvider = (IServiceProvider) container;
            var resolved = serviceProvider.GetService<IFoo>();

            return resolved != null;
        }

        public bool Can_resolve_through_fallback()
        {
            var fallback = new DependencyContainer();
            fallback.Register(typeof(IFoo), () => new Bar());

            var container = new DependencyContainer(fallback);

            var serviceProvider = (IServiceProvider) fallback;
            var resolved = serviceProvider.GetService<IFoo>();

            return resolved != null;
        }
		
        public interface IFoo { }
		public class Bar : IFoo { }
    }
}