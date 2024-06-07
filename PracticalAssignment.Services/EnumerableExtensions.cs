namespace PracticalAssignment.Services;

public static class EnumerableExtensions
{
    public static IEnumerable<T> MergeSort<T>(this IEnumerable<T> source) where T : IComparable<T>
    {
        List<T> items = new List<T>(source);
        return MergeSort(items).AsEnumerable();
    }

    private static List<T> MergeSort<T>(List<T> list) where T : IComparable<T>
    {
        if (list.Count <= 1)
        {
            return list;
        }

        int midIndex = list.Count / 2;
        List<T> left = MergeSort(list.GetRange(0, midIndex));
        List<T> right = MergeSort(list.GetRange(midIndex, list.Count - midIndex));

        return Merge(left, right);
    }

    private static List<T> Merge<T>(List<T> left, List<T> right) where T : IComparable<T>
    {
        int leftIndex = 0, rightIndex = 0;
        List<T> merged = new List<T>();

        while (leftIndex < left.Count && rightIndex < right.Count)
        {
            if (left[leftIndex].CompareTo(right[rightIndex]) <= 0)
            {
                merged.Add(left[leftIndex++]);
            }
            else
            {
                merged.Add(right[rightIndex++]);
            }
        }

        while (leftIndex < left.Count)
        {
            merged.Add(left[leftIndex++]);
        }
        while (rightIndex < right.Count)
        {
            merged.Add(right[rightIndex++]);
        }

        return merged;
    }

    public static IEnumerable<T> InsertionSort<T>(this IEnumerable<T> source) where T : IComparable<T>
    {
        List<T> sortedList = new List<T>(source);
        for (int i = 1; i < sortedList.Count; i++)
        {
            T current = sortedList[i];
            int j = i - 1;

            while (j >= 0 && sortedList[j].CompareTo(current) > 0)
            {
                sortedList[j + 1] = sortedList[j];
                j--;
            }
            sortedList[j + 1] = current;
        }
        return sortedList.AsEnumerable();
    }
}