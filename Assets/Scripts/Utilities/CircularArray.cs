using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vanaring_Utility_Tool
{
    public class CircularArray<T> : IEnumerable<T>
    {
        private List<T> data;
        public CircularArray(IEnumerable<T> initialData)
        {
            data = new List<T>(initialData);
        }

        public void Add(T item)
        {
            data.Add(item);
        }

        public void Reset()
        {
            data.Clear(); 
        }

        public List<T> GetDataAsList()
        {
            return data; 
        }
        public void Progress(bool forward)
        {
            if (data.Count <= 1)
            {
                return; // No need to progress if there's only one element or none.
            }

            if (forward)
            {
                T lastItem = data[data.Count - 1];
                data.RemoveAt(data.Count - 1);
                data.Insert(0, lastItem);
            }
            else
            {
                T firstItem = data[0];
                data.RemoveAt(0);
                data.Insert(data.Count, firstItem);
            }
        }


        public IEnumerator<T> GetEnumerator()
        {
            return data.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public T this[int index]
        {
            get
            {
                if (index >= 0 && index < data.Count)
                {
                    return data[index];
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
            set
            {
                if (index >= 0 && index < data.Count)
                {
                    data[index] = value;
                }
                else
                {
                    throw new IndexOutOfRangeException();
                }
            }
        }
    }
}
