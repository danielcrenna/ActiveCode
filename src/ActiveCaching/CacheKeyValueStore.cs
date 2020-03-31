// Copyright (c) Daniel Crenna & Contributors. All rights reserved.
// Licensed under the Apache License, Version 2.0. See License.txt in the project root for license information.

using ActiveStorage;
using System.Collections.Generic;

namespace ActiveCaching
{
	public sealed class CacheKeyValueStore<TKey, TValue> : IKeyValueStore<TKey, TValue>
	{
		private readonly ICache _cache;
		private readonly string _keyGroup;

		public CacheKeyValueStore(ICache cache, string keyGroup = null)
		{
			_cache = cache;
			_keyGroup = keyGroup;
		}

		public TValue this[TKey key] => _cache.Get<TValue>(CacheKey(key));

		public TValue GetOrAdd(TKey key, TValue metric)
		{
			var m = _cache.GetOrAdd(CacheKey(key), metric);
			UpdateKeyGroup(key);
			return m;
		}

		public bool TryGetValue(TKey key, out TValue metric)
		{
			metric = _cache.Get<TValue>(CacheKey(key));
			return metric != null;
		}

		public bool Contains(TKey key)
		{
			return TryGetValue(key, out _);
		}

		void IKeyValueStore<TKey, TValue>.AddOrUpdate<T>(TKey key, T value)
		{
			AddOrUpdate(key, value);
		}

		public string CacheKey(TKey key) => key.ToString();

		public void AddOrUpdate<T>(TKey key, T metric) where T : TValue
		{
			_cache.Set(CacheKey(key), metric);
			UpdateKeyGroup(key);
		}

		private void UpdateKeyGroup(TKey key)
		{
			if (string.IsNullOrWhiteSpace(_keyGroup))
				return;
			var list = _cache.GetOrAdd(_keyGroup, () => new List<TKey>());
			if (list.Contains(key))
				return;
			list.Add(key);
			_cache.Set(_keyGroup, list);
		}
	}
}