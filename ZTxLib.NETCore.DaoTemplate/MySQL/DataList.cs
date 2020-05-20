using System.Collections;
using System.Collections.Generic;

namespace ZTxLib.NETCore.DaoTemplate.MySQL
{
    public class DataList<T> : IList<T>
    {
        private readonly List<T> _list = new List<T>();

        public DataList(IRowMapper<T> mapper, Dao dao)
        {
            var reader = dao.ExecuteReader();
            var index = 0;
            while (reader.Read())
            {
                _list.Add(mapper.MapRow(reader, index));
                index++;
            }
            dao.Close();
        }

        public IEnumerator<T> GetEnumerator() => _list.GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => _list.GetEnumerator();

        public void Add(T item) => _list.Add(item);

        public void Clear() => _list.Clear();

        public bool Contains(T item) => _list.Contains(item);

        public void CopyTo(T[] array, int arrayIndex) => _list.CopyTo(array, arrayIndex);

        public bool Remove(T item) => _list.Remove(item);

        public int Count => _list.Count;

        public bool IsReadOnly => true;

        public int IndexOf(T item) => _list.IndexOf(item);

        public void Insert(int index, T item) => _list.Insert(index, item);

        public void RemoveAt(int index) => _list.RemoveAt(index);

        public T this[int index]
        {
            get => _list[index];
            set => _list[index] = value;
        }
    }
}