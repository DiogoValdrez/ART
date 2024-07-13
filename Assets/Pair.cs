using System;
using System.Collections.Generic;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

  

public class Pair<T1, T2>
{
    public T1 First { get; set; }
    public T2 Second { get; set; }

    public Pair(T1 first, T2 second)
    {
        First = first;
        Second = second;
    }

    public void set_first(T1 first) {
        this.First = first;
    }

    public void set_second(T2 second) {
        this.Second = second;
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