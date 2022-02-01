using Ledger8.Common;

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;

namespace Ledger8.DesktopUI.Controls;

public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged
    where TKey : notnull
{
    protected IDictionary<TKey, TValue> Dictionary { get; private set; }

    #region Constructors

    public ObservableDictionary() => Dictionary = new Dictionary<TKey, TValue>();
    public ObservableDictionary(IDictionary<TKey, TValue> source) => Dictionary = new Dictionary<TKey, TValue>(source);
    public ObservableDictionary(IEqualityComparer<TKey> comp) => Dictionary = new Dictionary<TKey, TValue>(comp);
    public ObservableDictionary(int capacity) => Dictionary = new Dictionary<TKey, TValue>(capacity);
    public ObservableDictionary(IDictionary<TKey, TValue> source, IEqualityComparer<TKey> comp) =>
        Dictionary = new Dictionary<TKey, TValue>(source, comp);
    public ObservableDictionary(int capacity, IEqualityComparer<TKey> comp) => Dictionary = new Dictionary<TKey, TValue>(capacity, comp);

    #endregion

    #region IDictionary<TKey, TValue> Methods

    public void Add(TKey key, TValue value) => Insert(key, value, true);

    public bool ContainsKey(TKey key) => Dictionary.ContainsKey(key);

    public ICollection<TKey> Keys => Dictionary.Keys;

    public bool Remove(TKey key)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        var removed = Dictionary.Remove(key);
        if (removed)
        {
            OnCollectionChanged();
        }
        return removed;
    }

    public bool TryGetValue(TKey key, out TValue value) => Dictionary.TryGetValue(key, out value!);

    public ICollection<TValue> Values => Dictionary.Values;

    public TValue this[TKey key]
    {
        get => Dictionary[key];
        set => Insert(key, value, true);
    }

    #endregion

    #region ICollection<KeyValuePair<TKey, TValue>> Methods

    public void Add(KeyValuePair<TKey, TValue> kvp) => Insert(kvp.Key, kvp.Value, true);

    public void Clear()
    {
        if (Dictionary.Any())
        {
            Dictionary.Clear();
            OnCollectionChanged();
        }
    }

    public bool Contains(KeyValuePair<TKey, TValue> kvp) => Dictionary.Contains(kvp);

    public void CopyTo(KeyValuePair<TKey, TValue>[] kvps, int index) => Dictionary.CopyTo(kvps, index);

    public int Count => Dictionary.Count;

    public bool IsReadOnly => Dictionary.IsReadOnly;

    public bool Remove(KeyValuePair<TKey, TValue> kvp) => Remove(kvp.Key);

    #endregion

    #region IEnumerable<KeyValuePair<TKey, TValue>> Methods

    public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator() => Dictionary.GetEnumerator();

    #endregion

    #region IEnumerable Methods

    IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)Dictionary).GetEnumerator();

    #endregion

    #region INotifyCollectionChanged

    public event NotifyCollectionChangedEventHandler? CollectionChanged;

    #endregion

    #region INotifyPropertyChanged

    public event PropertyChangedEventHandler? PropertyChanged;

    #endregion

    public void AddRange(IDictionary<TKey, TValue> items)
    {
        if (items is null)
        {
            throw new ArgumentNullException(nameof(items));
        }
        if (items.Any())
        {
            if (Dictionary.Any())
            {
                if (items.Keys.Any(key => Dictionary.ContainsKey(key)))
                {
                    throw new ArgumentException(Constants.DuplicateKey);
                }
                else
                {
                    foreach (var item in items)
                    {
                        Dictionary.Add(item);
                    }
                }
            }
            else
            {
                Dictionary = new Dictionary<TKey, TValue>(items);
            }
            OnCollectionChanged(NotifyCollectionChangedAction.Add, items.ToArray());
        }
    }

    private void Insert(TKey key, TValue value, bool add)
    {
        if (key is null)
        {
            throw new ArgumentNullException(nameof(key));
        }
        if (Dictionary.TryGetValue(key, out var item))
        {
            if (add)
            {
                throw new ArgumentException(Constants.DuplicateKey);
            }
            if (Equals(value, item))
            {
                return;
            }
            Dictionary[key] = value;
            OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value),
                new KeyValuePair<TKey, TValue>(key, item));
        }
        else
        {
            Dictionary[key] = value;
            OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
        }
    }

    private void OnPropertyChanged()
    {
        OnPropertyChanged(Constants.Count);
        OnPropertyChanged(Constants.Indexer);
        OnPropertyChanged(Constants.Keys);
        OnPropertyChanged(Constants.Values);
    }

    protected virtual void OnPropertyChanged(string property)
    {
        if (!string.IsNullOrWhiteSpace(property))
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(property));
        }
    }

    private void OnCollectionChanged()
    {
        OnPropertyChanged();
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> item)
    {
        OnPropertyChanged();
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, item));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> newitem, KeyValuePair<TKey, TValue> olditem)
    {
        OnPropertyChanged();
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newitem, olditem));
    }

    private void OnCollectionChanged(NotifyCollectionChangedAction action, IList newitems)
    {
        OnPropertyChanged();
        CollectionChanged?.Invoke(this, new NotifyCollectionChangedEventArgs(action, newitems));
    }
}
