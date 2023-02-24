using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public static  class ArrayExtensions
{
    public static bool IsNullOrEmpty(this Array arr)
    {
        return arr != null && arr.Length > 0;
    }

    public static T Random<T>(this IEnumerable<T> source ) {
        var rnd = new Random();
        var buffer = source.ToList();
        return buffer[rnd.Next(buffer.Count)];
    }

    public static IEnumerable<T> Shuffle<T>(this IEnumerable<T> source ) {
        return source.Shuffle( new Random() );
    }

    public static IEnumerable<T> Shuffle<T>( this IEnumerable<T> source, Random rng ) {
        return source.ShuffleIterator( rng );
    }

    private static IEnumerable<T> ShuffleIterator<T>(
        this IEnumerable<T> source, Random rng ) {
        var buffer = source.ToList();
        for( int i = 0; i < buffer.Count; i++ ) {
            int j = rng.Next( i, buffer.Count );
            yield return buffer[j];

            buffer[j] = buffer[i];
        }
    }

    public static IEnumerable<T> Take<T>(this IEnumerable<T> source, int[] indexes) {
        return source.Where( (o, index) => indexes.Contains(index) );
    }

    public static void ForEach<T>( this IEnumerable<T> source, Action<T> action )
    {
        if ( source == null )
            throw new ArgumentException( "Argument cannot be null.", "source" );

        foreach ( T value in source )
            action( value );
    }
}
