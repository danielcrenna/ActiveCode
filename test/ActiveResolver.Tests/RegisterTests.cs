// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections;
using System.Collections.Generic;

namespace ActiveResolver.Tests
{
    public class RegisterTests
    {
        public bool Can_register_type_with_transient_lifetime()
        {
            var container = new DependencyContainer();
            container.Register("A", typeof(IFoo), () => new Bar());
            container.TryResolve("A", typeof(IFoo), out var one);
            container.TryResolve("A", typeof(IFoo), out var two);
            return one != null && two != null && one != two && one is Bar && two is Bar;
        }

        public bool Can_register_type_with_singleton_lifetime()
        {
            var container = new DependencyContainer();
            container.Register("A", typeof(IFoo), () => new Bar(), InstanceIsUnique.PerProcess);
            container.TryResolve("A", typeof(IFoo), out var one);
            container.TryResolve("A", typeof(IFoo), out var two);
            return one == two && one is Bar;
        }

        public bool Can_register_without_name()
        {
            var container = new DependencyContainer();
            container.Register(typeof(IFoo), () => new Bar());
            container.TryResolve(typeof(IFoo), out var one);
            container.TryResolve(typeof(IFoo), out var two);
            return one != null && two != null && one != two && one is Bar && two is Bar;
        }

        public bool Can_resolve_multiple_registrations_as_enumerable()
        {
            var container = new DependencyContainer();
            container.Register(typeof(IFoo), () => new Bar(), InstanceIsUnique.PerProcess);
            container.Register(typeof(IFoo), () => new Bar(), InstanceIsUnique.PerProcess);
            return container.TryResolve(typeof(IEnumerable<IFoo>), out var instance) && 
                instance is List<IFoo> list && list.Count == 2 && list[0] != list[1];
        }

        public bool Can_resolve_single_enumerable_registration()
        {
            var container = new DependencyContainer();
            container.Register(typeof(IEnumerable<IFoo>), () => new List<IFoo> { new Bar(), new Bar()}, InstanceIsUnique.PerProcess);
            return container.TryResolve(typeof(IEnumerable<IFoo>), out var instance) && 
                   instance is List<IFoo> list && list.Count == 2 && list[0] != list[1];
        }

        public bool Can_resolve_single_and_multiple_enumerable_registrations_as_merged_list()
        {
            var container = new DependencyContainer();
            
            container.Register(typeof(IEnumerable<IFoo>), () => new List<IFoo> { new Bar(), new Bar()}, InstanceIsUnique.PerProcess);
            container.Register(typeof(IFoo), () => new Bar(), InstanceIsUnique.PerProcess);
            container.Register(typeof(IFoo), () => new Bar(), InstanceIsUnique.PerProcess);

            return container.TryResolve(typeof(IEnumerable<IFoo>), out var instance) && instance is List<IFoo> list &&
                   list.Count == 4 && list[0] != list[1] && list[1] != list[2] && list[2] != list[3];
        }
		
        public interface IFoo { }
		public class Bar : IFoo { }
    }
}