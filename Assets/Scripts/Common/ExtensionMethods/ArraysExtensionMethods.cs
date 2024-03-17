using System;

public static class ArraysExtensionMethods
{
    public static bool IsIndexValid(this string[] array, int index)
    {
        if (array == null)
        {
            return false;
        } 
        
        return index >= 0 && index < array.Length && index < array.Length;
    }
}