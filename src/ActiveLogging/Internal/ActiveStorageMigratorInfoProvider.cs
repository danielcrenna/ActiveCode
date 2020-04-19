// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System.Collections.Generic;
using ActiveStorage;
using TypeKitchen;

namespace ActiveLogging.Internal
{
	internal sealed class ActiveStorageMigratorInfoProvider : IDataMigratorInfoProvider
	{
		public IEnumerable<AccessorMembers> GetMigrationSubjects()
		{
			yield return AccessorMembers.Create(typeof(LogEntry), AccessorMemberTypes.Properties, AccessorMemberScope.Public);
		}
	}
}