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
        public static string Quoted(this string str)
        {
            if (str != null)
            { return "\"" + str + "\""; }
            else
            { return null; }
        }
        /// <summary>
        /// Check if object is a list
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static bool IsList(object obj)
        {
            if (obj!=null)
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
                if (t == typeof(ArrayList))
                {
                    return true;
                }
                return false;
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

        public static System.Collections.IEnumerable Recursive (System.Collections.IEnumerable list)
        {
            List<object> output = new List<object>();

            bool typelist = true;
            int deph = 0;
            foreach (var obj in (IList)list) // If it is a list, check if it contains a sublist
            {
                if (obj is IList) // If it contains a sublist
                {
                    //output[0].Add(new List<object>());
                    //int d = 1 + Recursive((System.Collections.IEnumerable)obj);

                    typelist = true;
                }
                else
                {

                }
            }
            
            return output;
        }
    }
}
