﻿using System;
using System.Collections.Generic;
using System.Text;

namespace PMAPortal.Web.Extensions
{
    public static class CollectionExtension
    {
        public static void AddRange<T>(this ICollection<T> collection, IEnumerable<T> items)
        {
            foreach (var item in items)
                collection.Add(item);
        }
    }
}
