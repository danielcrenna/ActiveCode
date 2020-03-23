// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Generic;
using System.Linq;
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

        public bool Can_resolve_enumerable_through_service_provider()
        {
            var container = new DependencyContainer();
            container.Register(typeof(IFoo), () => new Bar());
            container.Register(typeof(IFoo), () => new Bar());

            var serviceProvider = (IServiceProvider) container;
            var resolved = serviceProvider.GetServices<IFoo>();

            return resolved != null && resolved.Count() == 2;
        }

        public bool Can_resolve_through_service_provider()
        {
            var container = new DependencyContainer();
            container.Register(typeof(IFoo), () => new Bar());

            var serviceProvider = (IServiceProvider) container;
            var resolved = serviceProvider.GetRequiredService<IFoo>();

            return resolved != null;
        }

        public bool Can_resolve_through_fallback()
        {
            var fallback = new DependencyContainer();
            fallback.Register(typeof(IFoo), () => new Bar());

            var container = new DependencyContainer(fallback);

            var serviceProvider = (IServiceProvider) container;
            var resolved = serviceProvider.GetService<IFoo>();

            return resolved != null;
        }

        public bool Can_auto_resolve_dependent_type()
        {
            var container = new DependencyContainer();
            container.Register(typeof(IFoo), () => new Bar());

            container.TryResolve(typeof(Baz), out var baz);
            return baz != null;
        }

        public bool Can_resolve_inner_dependencies_with_service_provider()
        {
            var container = new DependencyContainer();
            container.Register("A", typeof(IFoo), () => new Bar());

			if(!container.CanResolve<IFoo>("A"))
                return false; // not in the right slot

            if (container.CanResolve("B", typeof(IFoo)))
                return false; // not in the right slot

            if (container.TryResolve(out IFoo _))
                return false; // not in the right slot

            container.Register("B", typeof(Baz), r => new Baz(r.Resolve<IFoo>("A")));

            var result = container.TryResolve<Baz>("B", out var baz);
            return result && baz?.Foo != null;
        }

		public interface IFoo { }
		public class Bar : IFoo { }
        public class Baz
        {
            public IFoo Foo { get; }

            public Baz(IFoo foo)
            {
                Foo = foo;
            }
        }
    }
}