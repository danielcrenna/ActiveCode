// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using System;
using System.Collections.Concurrent;
using System.Reflection;
using System.Text;
using ActiveCaching.Configuration;
using ActiveCaching.Internal;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Internal;
using Microsoft.Extensions.Options;
using TypeKitchen;
using SysOptions = Microsoft.Extensions.Options.Options;

namespace ActiveCaching
{
	public class DistributedCache : ICache
	{
		private static readonly MethodInfo SerializeMethod =
			typeof(ICacheSerializer).GetMethod(nameof(ICacheSerializer.ObjectToBuffer));

		private static readonly ConcurrentDictionary<Type, MethodInfo> SerializeMethods =
			new ConcurrentDictionary<Type, MethodInfo>();

		private static readonly MethodInfo DeserializeMethod =
			typeof(ICacheSerializer).GetMethod(nameof(ICacheDeserializer.BufferToObject));

		private static readonly ConcurrentDictionary<Type, MethodInfo> DeserializeMethods =
			new ConcurrentDictionary<Type, MethodInfo>();

		private static readonly ConcurrentDictionary<Type, byte[]> TypeSignatures =
			new ConcurrentDictionary<Type, byte[]>();

		private readonly IDistributedCache _cache;
		private readonly ICacheDeserializer _deserializer;
		private readonly ICacheSerializer _serializer;

		public DistributedCache(
			ICacheSerializer serializer,
			ICacheDeserializer deserializer,
			ISystemClock clock,
			Func<DateTimeOffset> timestamps)
		{
			_serializer = serializer;
			_deserializer = deserializer;
			_cache = new MemoryDistributedCache(SysOptions.Create(new MemoryDistributedCacheOptions
			{
				CompactionPercentage = 0.05,
				ExpirationScanFrequency = TimeSpan.FromMinutes(1.0),
				SizeLimit = null,
				Clock = clock ?? new DelegatedSystemClock(timestamps)
			}));
		}

		public void Remove(string key)
		{
			_cache.Remove(key);
		}

		private static bool Try(Action closure)
		{
			try
			{
				closure.Invoke();
				return true;
			}
			catch
			{
				return false;
			}
		}

		private bool RemoveByKeyThen(string key, Func<bool> operation)
		{
			try
			{
				_cache.Remove(key);
				return operation();
			}
			catch
			{
				return false;
			}
		}

		private bool EnsureKeyExistsThen(string key, Func<bool> operation)
		{
			try
			{
				return _cache.Get(key) != null && operation();
			}
			catch
			{
				return false;
			}
		}

		private byte[] SerializeInternal(Type type, object value)
		{
			var method = SerializeMethods.GetOrAdd(type, t => SerializeMethod.MakeGenericMethod(t));
			var accessor = CallAccessor.Create(method);
			return (byte[]) accessor.Call(_serializer, new[] {value});
		}

		private byte[] SerializeInternal<T>(T value)
		{
			var method = SerializeMethods.GetOrAdd(typeof(T), t => SerializeMethod.MakeGenericMethod(t));
			var accessor = CallAccessor.Create(method);
			return (byte[]) accessor.Call(_serializer, new object[] {value});
		}

		private object DeserializeInternal(Type type, byte[] bytes)
		{
			var method = DeserializeMethods.GetOrAdd(type, t => DeserializeMethod.MakeGenericMethod(t));
			var accessor = CallAccessor.Create(method);
			return accessor.Call(_serializer, new object[] {bytes});
		}

		private void SetWithType(string key, object value, DistributedCacheEntryOptions entry)
		{
			var type = value.GetType();
			var typeSignature = TypeSignatures.GetOrAdd(type,
				t => Encoding.UTF8.GetBytes(t.AssemblyQualifiedName ?? throw new InvalidOperationException()));
			_cache.Set($"{key}:type", typeSignature, entry);
			_cache.Set(key, SerializeInternal(type, value), entry);
		}

		private void SetWithType<T>(string key, T value, DistributedCacheEntryOptions entry)
		{
			var type = value.GetType();
			var typeSignature = TypeSignatures.GetOrAdd(type,
				t => Encoding.UTF8.GetBytes(t.AssemblyQualifiedName ?? throw new InvalidOperationException()));
			_cache.Set($"{key}:type", typeSignature, entry);
			_cache.Set(key, SerializeInternal(value), entry);
		}

		private static DistributedCacheEntryOptions CreateEntry(DateTimeOffset? absoluteExpiration = null,
			TimeSpan? slidingExpiration = null)
		{
			var policy = new DistributedCacheEntryOptions
			{
				AbsoluteExpiration = absoluteExpiration, SlidingExpiration = slidingExpiration
			};

			return policy;
		}

		#region Set

		public bool Set(string key, object value)
		{
			return Try(() =>
			{
				var entry = CreateEntry();
				SetWithType(key, value, entry);
			});
		}

		public bool Set(string key, object value, DateTimeOffset absoluteExpiration)
		{
			return Try(() =>
			{
				var entry = CreateEntry(absoluteExpiration);
				SetWithType(key, value, entry);
			});
		}

		public bool Set(string key, object value, TimeSpan slidingExpiration)
		{
			return Try(() =>
			{
				var entry = CreateEntry(slidingExpiration: slidingExpiration);
				SetWithType(key, value, entry);
			});
		}

		public bool Set(string key, object value, ICacheDependency dependency)
		{
			return Try(() =>
			{
				var entry = CreateEntry();
				SetWithType(key, value, entry);
			});
		}

		public bool Set(string key, object value, DateTimeOffset absoluteExpiration, ICacheDependency dependency)
		{
			return Try(() =>
			{
				var entry = CreateEntry(absoluteExpiration);
				SetWithType(key, value, entry);
			});
		}

		public bool Set(string key, object value, TimeSpan slidingExpiration, ICacheDependency dependency)
		{
			return Try(() =>
			{
				var entry = CreateEntry(slidingExpiration: slidingExpiration);
				SetWithType(key, value, entry);
			});
		}

		public bool Set<T>(string key, T value)
		{
			return Try(() =>
			{
				var entry = CreateEntry();
				SetWithType(key, value, entry);
			});
		}

		public bool Set<T>(string key, T value, DateTimeOffset absoluteExpiration)
		{
			return Try(() =>
			{
				var entry = CreateEntry(absoluteExpiration);
				SetWithType(key, value, entry);
			});
		}

		public bool Set<T>(string key, T value, TimeSpan slidingExpiration)
		{
			return Try(() =>
			{
				var entry = CreateEntry(slidingExpiration: slidingExpiration);
				SetWithType(key, value, entry);
			});
		}

		public bool Set<T>(string key, T value, ICacheDependency dependency)
		{
			return Try(() =>
			{
				var entry = CreateEntry();
				SetWithType(key, value, entry);
			});
		}

		public bool Set<T>(string key, T value, DateTimeOffset absoluteExpiration, ICacheDependency dependency)
		{
			return Try(() =>
			{
				var entry = CreateEntry(absoluteExpiration);
				SetWithType(key, value, entry);
			});
		}

		public bool Set<T>(string key, T value, TimeSpan slidingExpiration, ICacheDependency dependency)
		{
			return Try(() =>
			{
				var entry = CreateEntry(slidingExpiration: slidingExpiration);
				SetWithType(key, value, entry);
			});
		}

		#endregion

		#region Add

		public bool Add(string key, object value)
		{
			if (_cache.Get(key) != null)
				return false;
			var entry = CreateEntry();
			SetWithType(key, value, entry);
			return true;
		}

		public bool Add(string key, object value, DateTimeOffset absoluteExpiration)
		{
			if (_cache.Get(key) != null)
				return false;
			var entry = CreateEntry(absoluteExpiration);
			SetWithType(key, value, entry);
			return true;
		}

		public bool Add(string key, object value, TimeSpan slidingExpiration)
		{
			if (_cache.Get(key) != null)
				return false;
			var entry = CreateEntry(slidingExpiration: slidingExpiration);
			SetWithType(key, value, entry);
			return true;
		}

		public bool Add(string key, object value, ICacheDependency dependency)
		{
			if (_cache.Get(key) != null)
				return false;
			var entry = CreateEntry();
			SetWithType(key, value, entry);
			return true;
		}

		public bool Add(string key, object value, DateTimeOffset absoluteExpiration, ICacheDependency dependency)
		{
			if (_cache.Get(key) != null)
				return false;
			var entry = CreateEntry(absoluteExpiration);
			SetWithType(key, value, entry);
			return true;
		}

		public bool Add(string key, object value, TimeSpan slidingExpiration, ICacheDependency dependency)
		{
			if (_cache.Get(key) != null)
				return false;
			var entry = CreateEntry(slidingExpiration: slidingExpiration);
			SetWithType(key, value, entry);
			return true;
		}

		public bool Add<T>(string key, T value)
		{
			if (_cache.Get(key) != null)
				return false;
			var entry = CreateEntry();
			SetWithType(key, value, entry);
			return true;
		}

		public bool Add<T>(string key, T value, DateTimeOffset absoluteExpiration)
		{
			if (_cache.Get(key) != null)
				return false;
			var entry = CreateEntry(absoluteExpiration);
			SetWithType(key, value, entry);
			return true;
		}

		public bool Add<T>(string key, T value, TimeSpan slidingExpiration)
		{
			if (_cache.Get(key) != null)
				return false;
			var entry = CreateEntry(slidingExpiration: slidingExpiration);
			SetWithType(key, value, entry);
			return true;
		}

		public bool Add<T>(string key, T value, ICacheDependency dependency)
		{
			if (_cache.Get(key) != null)
				return false;
			var entry = CreateEntry();
			SetWithType(key, value, entry);
			return true;
		}

		public bool Add<T>(string key, T value, DateTimeOffset absoluteExpiration, ICacheDependency dependency)
		{
			if (_cache.Get(key) != null)
				return false;
			var entry = CreateEntry(absoluteExpiration);
			SetWithType(key, value, entry);
			return true;
		}

		public bool Add<T>(string key, T value, TimeSpan slidingExpiration, ICacheDependency dependency)
		{
			if (_cache.Get(key) != null)
				return false;
			var entry = CreateEntry(slidingExpiration: slidingExpiration);
			SetWithType(key, value, entry);
			return true;
		}

		#endregion

		#region Replace

		public bool Replace(string key, object value)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value)));
		}

		public bool Replace(string key, object value, DateTimeOffset absoluteExpiration)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value, absoluteExpiration)));
		}

		public bool Replace(string key, object value, TimeSpan slidingExpiration)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value, slidingExpiration)));
		}

		public bool Replace(string key, object value, ICacheDependency dependency)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value, dependency)));
		}

		public bool Replace(string key, object value, DateTimeOffset absoluteExpiration, ICacheDependency dependency)
		{
			return EnsureKeyExistsThen(key,
				() => RemoveByKeyThen(key, () => Add(key, value, absoluteExpiration, dependency)));
		}

		public bool Replace(string key, object value, TimeSpan slidingExpiration, ICacheDependency dependency)
		{
			return EnsureKeyExistsThen(key,
				() => RemoveByKeyThen(key, () => Add(key, value, slidingExpiration, dependency)));
		}

		public bool Replace<T>(string key, T value)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value)));
		}

		public bool Replace<T>(string key, T value, DateTimeOffset absoluteExpiration)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value, absoluteExpiration)));
		}

		public bool Replace<T>(string key, T value, TimeSpan slidingExpiration)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value, slidingExpiration)));
		}

		public bool Replace<T>(string key, T value, ICacheDependency dependency)
		{
			return EnsureKeyExistsThen(key, () => RemoveByKeyThen(key, () => Add(key, value, dependency)));
		}

		public bool Replace<T>(string key, T value, DateTimeOffset absoluteExpiration, ICacheDependency dependency)
		{
			return EnsureKeyExistsThen(key,
				() => RemoveByKeyThen(key, () => Add(key, value, absoluteExpiration, dependency)));
		}

		public bool Replace<T>(string key, T value, TimeSpan slidingExpiration, ICacheDependency dependency)
		{
			return EnsureKeyExistsThen(key,
				() => RemoveByKeyThen(key, () => Add(key, value, slidingExpiration, dependency)));
		}

		#endregion

		#region Get

		public object Get(string key, TimeSpan? timeout = null) => GetOrAdd(key, null, timeout);

		public object GetOrAdd(string key, Func<object> add = null, TimeSpan? timeout = null)
		{
			var typeBytes = _cache.Get($"{key}:type");
			if (typeBytes == null)
				return null;

			var type = Type.GetType(Encoding.UTF8.GetString(typeBytes));
			if (type == null)
				return null;

			var bytes = _cache.Get(key);
			if (bytes == null)
				return default;

			var item = DeserializeInternal(type, bytes);
			if (item != null)
				return item;

			if (add == null)
				return null;
			
			var itemToAdd = add();
			if (itemToAdd != null)
				Add(key, itemToAdd);
			return itemToAdd;
		}

		public object GetOrAdd(string key, object add = null, TimeSpan? timeout = null) => GetOrAdd(key, () => add, timeout);

		public T Get<T>(string key, TimeSpan? timeout = null) => GetOrAdd<T>(key, null, timeout);

		public T GetOrAdd<T>(string key, T add = default, TimeSpan? timeout = null) => GetOrAdd(key, () => add, timeout);

		public T GetOrAdd<T>(string key, Func<T> add = null, TimeSpan? timeout = null)
		{
			var bytes = _cache.Get(key);
			if (bytes == null)
				return default;

			var deserialized = _deserializer.BufferToObject<T>(bytes);

			var item = deserialized is T typed ? typed : default;
			if (item != null)
				return item;

			if (add == null)
				return default;

			var itemToAdd = add();
			if (itemToAdd != null)
				Add(key, itemToAdd);

			return itemToAdd;
		}

		#endregion
	}
}