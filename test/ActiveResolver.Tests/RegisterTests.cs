// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

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

        public interface IFoo
        {
        }

        public class Bar : IFoo
        {
        }
    }
}