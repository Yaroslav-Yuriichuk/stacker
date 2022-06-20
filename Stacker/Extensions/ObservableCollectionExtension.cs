using System;
using System.Collections.ObjectModel;

namespace Stacker.Extensions
{
    public static class ObservableCollectionExtension
    {
        public static T Find<T>(this ObservableCollection<T> collection, Predicate<T> match)
        {
            foreach (T item in collection)
            {
                if (match(item)) return item;
            }

            return default(T);
        }

    }
}
