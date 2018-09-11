using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ENGyn.Nodes
{
    public static class MainTools
    {
        public static bool IsString(object a)
        {
            if (a.GetType() == typeof(string))
                return true;
            return false;
        }

        /// <summary>
        /// Check if object is a list
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsList(object obj)
        {
            var t = obj.GetType();

            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(List<>))
            {
                return true;
            }
            if (t.IsGenericType && t.GetGenericTypeDefinition() == typeof(IList<>))
            {
                return true;
            }
            return false;

        }

        
        /// <summary>
        /// Check if object can contain type - Used after IsList()
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ListContainsType(object obj, Type type)
        {
            return obj.GetType().IsAssignableFrom(type);
        }

        public static int Count<TSource>(this IEnumerable<TSource> source)
        {
            if (source != null)
            { }
            ICollection<TSource> collectionoft = source as ICollection<TSource>;
            if (collectionoft != null) return collectionoft.Count;

            ICollection collection = source as ICollection;
            if (collection != null) return collection.Count;

            int count = 0;
            using (IEnumerator<TSource> e = source.GetEnumerator())
            {
                checked
                {
                    while (e.MoveNext()) count++;
                }
            }

            return count;
        }
    }
}
