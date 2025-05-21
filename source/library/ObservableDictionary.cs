// Copyright (c) 2010 Shimmy Weitzhandler, MS-PL License
// http://blogs.microsoft.co.il/blogs/shimmy/archive/2010/12/26/observabledictionary-lt-tkey-tvalue-gt-c.aspx

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;

namespace PoshGUIExample {
    public class ObservableDictionary<TKey, TValue> : IDictionary<TKey, TValue>, INotifyCollectionChanged, INotifyPropertyChanged {
        #region Fields

        private readonly IDictionary<TKey, TValue> _dictionary;

        #endregion Fields

        #region Properties

        protected IDictionary<TKey, TValue> Dictionary {
            get => _dictionary;
        }

        #endregion Properties

        #region Constructors

        public ObservableDictionary () {
            _dictionary = new Dictionary<TKey, TValue>();
        }

        public ObservableDictionary (IDictionary<TKey, TValue> dictionary) {
            _dictionary = new Dictionary<TKey, TValue>(dictionary);
        }

        public ObservableDictionary (IEqualityComparer<TKey> comparer) {
            _dictionary = new Dictionary<TKey, TValue>(comparer);
        }

        public ObservableDictionary (int capacity) {
            _dictionary = new Dictionary<TKey, TValue>(capacity);
        }

        public ObservableDictionary (IDictionary<TKey, TValue> dictionary, IEqualityComparer<TKey> comparer) {
            _dictionary = new Dictionary<TKey, TValue>(dictionary, comparer);
        }

        public ObservableDictionary (int capacity, IEqualityComparer<TKey> comparer) {
            _dictionary = new Dictionary<TKey, TValue>(capacity, comparer);
        }

        #endregion Constructors

        #region ICollection<KeyValuePair<TKey,TValue>> Members

        public int Count {
            get => Dictionary.Count;
        }

        public bool IsReadOnly {
            get => Dictionary.IsReadOnly;
        }

        public void Add (KeyValuePair<TKey, TValue> item) {
            Insert(item.Key, item.Value, true);
        }

        public void Clear () {
            if (Dictionary.Count > 0) {
                Dictionary.Clear();

                OnCollectionChanged();
            }
        }

        public bool Contains (KeyValuePair<TKey, TValue> item) {
            return Dictionary.Contains(item);
        }

        public void CopyTo (KeyValuePair<TKey, TValue>[] array, int arrayIndex) {
            Dictionary.CopyTo(array, arrayIndex);
        }

        public bool Remove (KeyValuePair<TKey, TValue> item) {
            return Remove(item.Key);
        }

        #endregion ICollection<KeyValuePair<TKey,TValue>> Members

        #region IDictionary<TKey,TValue> Members

        public TValue this[TKey key] {
            get => Dictionary[key];
            set => Insert(key, value, false);
        }

        public ICollection<TKey> Keys {
            get => Dictionary.Keys;
        }

        public ICollection<TValue> Values {
            get => Dictionary.Values;
        }

        public void Add (TKey key, TValue value) {
            Insert(key, value, true);
        }

        public bool ContainsKey (TKey key) {
            return Dictionary.ContainsKey(key);
        }

        public bool Remove (TKey key) {
            Dictionary.TryGetValue(key, out TValue value);

            bool result = Dictionary.Remove(key);

            if (result)
                OnCollectionChanged(NotifyCollectionChangedAction.Remove, new KeyValuePair<TKey, TValue>(key, value));

            return result;
        }

        public bool TryGetValue (TKey key, out TValue value) {
            return Dictionary.TryGetValue(key, out value);
        }

        #endregion IDictionary<TKey,TValue> Members

        #region IEnumerable<KeyValuePair<TKey,TValue>> Members

        public IEnumerator<KeyValuePair<TKey, TValue>> GetEnumerator () {
            return Dictionary.GetEnumerator();
        }

        #endregion IEnumerable<KeyValuePair<TKey,TValue>> Members

        #region IEnumerable Members

        IEnumerator IEnumerable.GetEnumerator () {
            return ((IEnumerable) Dictionary).GetEnumerator();
        }

        #endregion IEnumerable Members

        #region INotifyCollectionChanged Members

        public event NotifyCollectionChangedEventHandler CollectionChanged;

        #endregion INotifyCollectionChanged Members

        #region INotifyPropertyChanged Members

        public event PropertyChangedEventHandler PropertyChanged;

        #endregion INotifyPropertyChanged Members

        #region Private Methods

        private void Insert (TKey key, TValue value, bool add) {
            if (Dictionary.TryGetValue(key, out TValue oldValue)) {
                if (add)
                    throw new ArgumentException($"An item with the same key has already been added. Key: {key}");

                if (Equals(oldValue, value))
                    return;

                Dictionary[key] = value;

                OnCollectionChanged(NotifyCollectionChangedAction.Replace, new KeyValuePair<TKey, TValue>(key, value), new KeyValuePair<TKey, TValue>(key, oldValue));
            } else {
                Dictionary[key] = value;

                OnCollectionChanged(NotifyCollectionChangedAction.Add, new KeyValuePair<TKey, TValue>(key, value));
            }
        }

        #endregion Private Methods

        #region Event Handlers

        private void OnCollectionChanged () {
            OnPropertyChanged();

            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
        }

        private void OnCollectionChanged (NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> changedItem) {
            Trace.Assert(action == NotifyCollectionChangedAction.Add || action == NotifyCollectionChangedAction.Remove);

            OnPropertyChanged();

            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, changedItem));
        }

        private void OnCollectionChanged (NotifyCollectionChangedAction action, KeyValuePair<TKey, TValue> newItem, KeyValuePair<TKey, TValue> oldItem) {
            Trace.Assert(action == NotifyCollectionChangedAction.Replace);

            OnPropertyChanged("Values");

            if (CollectionChanged != null)
                CollectionChanged(this, new NotifyCollectionChangedEventArgs(action, newItem, oldItem));
        }

        private void OnPropertyChanged () {
            OnPropertyChanged("Count");
            OnPropertyChanged("Keys");
            OnPropertyChanged("Values");
        }

        private void OnPropertyChanged (string propertyName) {
            if (PropertyChanged != null)
                PropertyChanged(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    #endregion Event Handlers
}
