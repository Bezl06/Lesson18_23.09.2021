using System;

namespace Lesson18
{
    class Program
    {
        static void Main(string[] args)
        {
            MyList<string> myList = new MyList<string>();
            myList.Add("Zero");
            System.Console.WriteLine(myList[0]);
            myList.Add("Next");
            System.Console.WriteLine(myList[1]);
            myList[0] = "Quick";
            System.Console.WriteLine(myList[0]);
            System.Console.WriteLine(myList.Count + "\n****************");
            MyDictionary<string, int> myDictionary = new MyDictionary<string, int>();
            myDictionary.Add("Hello", 5);
            System.Console.WriteLine(myDictionary["Hello"]);
            myDictionary["GG"] = 16;
            System.Console.WriteLine(myDictionary["GG"]);
            System.Console.WriteLine(myDictionary.Count);
        }
    }
    class MyList<T>
    {
        private T[] arr;
        public int Count { get; set; } = 0;
        public int Capasity => arr.Length;
        public T this[int index]
        {
            get
            {
                if (index >= Count) throw new ArgumentOutOfRangeException();
                return arr[index];
            }
            set
            {
                if (index >= Count) throw new ArgumentOutOfRangeException();
                arr[index] = value;
            }
        }
        public MyList() : this(1) { }
        public MyList(int capasity) => arr = new T[capasity];
        public void Add(T item)
        {
            if (++Count > Capasity)
                Array.Resize(ref arr, Capasity * 2);
            arr[Count - 1] = item;
        }
        public bool Remove(T item)
        {
            int deleted = Array.IndexOf<T>(arr, item, 0, Count);
            if (deleted < 0) return false;
            for (int i = 0, t = 0; i < Count; i++)
            {
                if (i == deleted) continue;
                arr[t++] = arr[i];
            }
            Count--;
            return true;
        }
    }
    class MyDictionary<TKey, TValue>
    {
        private struct MyPair
        {
            public int Next;
            public TKey Key;
            public TValue Value;
            public MyPair(TKey key, TValue value, int next)
            {
                Key = key;
                Value = value;
                Next = next;
            }
        }
        private int[] bucket;
        private MyPair[] pairs;
        public int Capasity => pairs.Length;
        public int Count { get; private set; } = 0;
        public TValue this[TKey key]
        {
            get
            {
                int index = FindIndex(key);
                if (index >= 0) return pairs[index].Value;
                throw new ArgumentException();
            }
            set
            {
                int index = FindIndex(key);
                if (index >= 0) pairs[index].Value = value;
                else Add(key, value);
            }
        }
        public MyDictionary() : this(2) { }
        public MyDictionary(int capasity)
        {
            pairs = new MyPair[capasity];
            bucket = new int[capasity];
            for (int i = 0; i < capasity; i++)
                bucket[i] = -1;
        }
        public void Add(TKey key, TValue value)
        {
            if (FindIndex(key) >= 0) throw new ArgumentException();
            if (Count + 1 >= pairs.Length) Resize(pairs.Length * 2);
            int bucketIndex = (key.GetHashCode() & 0x7FFFFFFF) % bucket.Length;
            int pairIndex = Count++;
            pairs[pairIndex].Key = key;
            pairs[pairIndex].Value = value;
            pairs[pairIndex].Next = bucket[bucketIndex];
            bucket[bucketIndex] = pairIndex;
        }
        public bool ContainsKey(TKey key) => FindIndex(key) >= 0;
        private int FindIndex(TKey key)
        {
            int bucketIndex = (key.GetHashCode() & 0x7FFFFFFF) % bucket.Length;
            for (int i = bucket[bucketIndex]; i >= 0; i = pairs[i].Next)
                if (pairs[i].Key.Equals(key)) return i;
            return -1;
        }
        private void Resize(int newSize)
        {
            int[] newBucket = new int[newSize];
            for (int i = 0; i < newSize; i++)
                newBucket[i] = -1;
            bucket = newBucket;
            Array.Resize<MyPair>(ref pairs, newSize);
            for (int i = 0; i < Count; i++)
            {
                int bucketIndex = (pairs[i].Key.GetHashCode() & 0x7FFFFFFF) % newSize;
                pairs[i].Next = bucket[bucketIndex];
                bucket[bucketIndex] = i;
            }
        }
    }
}
