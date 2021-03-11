using System;
using System.Linq;
using System.Collections.Generic;
using System.Collections;

namespace UnityUtility.Linq
{
    //public static partial class LinqExtensions
    //{
    //    /// <summary>
    //    /// 今は使わないけどあとで使う値をキャッシュします
    //    /// </summary>
    //    /// <typeparam name="T">シーケンスの型</typeparam>
    //    /// <typeparam name="TCache">キャッシュの型</typeparam>
    //    /// <param name="source"></param>
    //    /// <param name="cacheSelector"></param>
    //    /// <returns></returns>
    //    public static IEnumerable<Cached<T, TCache>> Cache<T, TCache>(this IEnumerable<T> source, Func<T, TCache> cacheSelector)
    //    {
    //        return source.Select(item => new Cached<T, TCache>(item, cacheSelector(item)));
    //    }
    //    /// <summary>
    //    /// キャッシュ済みの値を取り出します
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <typeparam name="TCache"></typeparam>
    //    /// <param name="source"></param>
    //    /// <returns></returns>
    //    public static IEnumerable<(T value, TCache cache)> UnCache<T, TCache>(this IEnumerable<Cached<T, TCache>> source)
    //    {
    //        return source.Select(cache =>
    //        {
    //            ICache<TCache> icache = cache;
    //            return (cache.Value, icache.Cache);
    //        });
    //    }
    //    /// <summary>
    //    /// キャッシュ済みの値のみを取り出します
    //    /// </summary>
    //    /// <typeparam name="T"></typeparam>
    //    /// <typeparam name="TCache"></typeparam>
    //    /// <param name="cachedSource"></param>
    //    /// <returns></returns>
    //    public static IEnumerable<TCache> UnCacheCacheOnly<T, TCache>(this IEnumerable<Cached<T, TCache>> cachedSource)
    //    {
    //        foreach (var cache in cachedSource)
    //        {
    //            yield return ((ICache<TCache>)cache).Cache;
    //        }
    //    }

    //    public static IEnumerable<Cached<TResult, TCache>> Select<TSource, TResult, TCache>(
    //        this IEnumerable<Cached<TSource, TCache>> cachedSource,
    //        Func<TSource, TResult> resultSelector)
    //    {
    //        foreach (var cache in cachedSource)
    //        {
    //            yield return new Cached<TResult, TCache>(resultSelector(cache.Value), ((ICache<TCache>)cache).Cache);
    //        }
    //    }
    //}

    //internal interface ICache<TCache>
    //{
    //    TCache Cache { get; set; }
    //}
    //public class Cached<T, TCache> : ICache<TCache>
    //{
    //    private TCache m_cache;

    //    internal Cached(T value, TCache cache)
    //    {
    //        this.Value = value;
    //        this.m_cache = cache;
    //    }

    //    public T Value { get; internal set; }

    //    public override string ToString() => Value.ToString();

    //    TCache ICache<TCache>.Cache
    //    {
    //        get => m_cache;
    //        set => m_cache = value;
    //    }

    //    public static implicit operator T(Cached<T, TCache> cached) => cached.Value;
    //}
}