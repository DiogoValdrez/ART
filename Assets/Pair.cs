using System;
using System.Collections.Generic;

namespace AuxiliarDataStructures{
public class Pair<T1, T2>
{
    public T1 First { get; }
    public T2 Second { get; }

    public Pair(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }

    public override bool Equals(object obj)
    {
        if (obj == null || GetType() != obj.GetType())
        {
            return false;
        }

        Pair<T1, T2> other = obj as Pair<T1, T2>;
        return EqualityComparer<T1>.Default.Equals(First, other.First) &&
               EqualityComparer<T2>.Default.Equals(Second, other.Second);
    }

    public override int GetHashCode()
    {
        int hashFirst = First == null ? 0 : First.GetHashCode();
        int hashSecond = Second == null ? 0 : Second.GetHashCode();
        return hashFirst ^ hashSecond;
    }

    public override string ToString()
    {
        return $"({First}, {Second})";
    }
}
}
