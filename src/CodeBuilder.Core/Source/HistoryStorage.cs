// -----------------------------------------------------------------------
// <copyright license="GPL"
//      company="fireasy.cn"
//      email="faib920@126.com"
//      qq="55570729">
//   (c) Copyright Fireasy. All rights reserved.
// </copyright>
// -----------------------------------------------------------------------
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace CodeBuilder.Core.Source
{
    public class HistoryStorage<T>
    {
        private readonly string _fileName;
        private readonly int _size;

        public HistoryStorage(string fileName, int size)
        {
            _fileName = fileName;
            _size = size;

            if (File.Exists(fileName))
            {
                Items = JsonConvert.DeserializeObject<List<T>>(File.ReadAllText(fileName));
            }
        }

        public List<T> Items { get; private set; } = new List<T>();

        public void Add(T item)
        {
            if (Items.Contains(item))
            {
                if (Items.Count == 1)
                {
                    return;
                }

                Items.Remove(item);
            }
            else if (Items.Count == _size)
            {
                Items.RemoveAt(_size - 1);
            }

            Items.Insert(0, item);

            Util.TryOperateFile(_fileName, () => File.WriteAllText(_fileName, JsonConvert.SerializeObject(Items)));
        }

        public void Clear()
        {
            Items.Clear();

            Util.TryOperateFile(_fileName, () => File.WriteAllText(_fileName, "[]"));
        }

        public void Delete(T record)
        {
            for (var i = Items.Count - 1; i >= 0; i--)
            {
                if (Items[i].Equals(record))
                {
                    Items.RemoveAt(i);
                }
            }

            Util.TryOperateFile(_fileName, () => File.WriteAllText(_fileName, JsonConvert.SerializeObject(Items)));
        }
    }
}
