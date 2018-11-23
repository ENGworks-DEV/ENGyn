using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TUM.CMS.VplControl.Core;

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

        //erase
        public delegate object ParamsAction(params object[] arguments);

        


        private static void StrechArgumentLists(List<Port> ports, List<Tuple<int, object>> values)
        {
            //Fill values list
            foreach (var arg in ports)
            {

                System.Collections.IList argList = null;
                if (MainTools.IsList(arg.Data))
                {
                    argList = (System.Collections.IList)arg.Data;
                }
                //Check if argument is a list and include it on a Tuple with its count number
                var argResult = argList != null ? new Tuple<int, object>(argList.Count, argList) : new Tuple<int, object>(1, arg.Data);
                //if args are list, count the number of object to set how many times it should run
                values.Add(argResult);

            }
        }


        //The sleep of reason produces monsters, or in my case horrible methods
        public static object RunFunction(Func<object, object, object, object, object, object> method, List<Port> ports)
        {

            var properties = method.GetType().GetProperties();

            var result = new List<object>();
            var values = new List<Tuple<int, object>>();

            StrechArgumentLists(ports, values);

            //Get highest value == longest list count
            int Highest = values.OrderBy(x => x.Item1).Reverse().ElementAt(0).Item1;



            for (int i = 0; i < Highest; i++)
            {
                var ActualArgList = new List<object>();
                foreach (var item in values)
                {
                    object currentArg = null;
                    if (item.Item1 > 1 || MainTools.IsList(item.Item2))
                    {
                        var tempArgList = (List<object>)item.Item2;
                        //get latest object

                        var currentArgListLenght = tempArgList.Count - 1;
                        currentArg = currentArgListLenght <= i ? tempArgList[currentArgListLenght] : tempArgList[i];
                        ActualArgList.Add(currentArg);
                    }
                    else
                    {
                        currentArg = item.Item2;
                        ActualArgList.Add(currentArg);
                    }
                }

                result.Add(method.DynamicInvoke(ActualArgList[0], ActualArgList[1], ActualArgList[2], ActualArgList[3], ActualArgList[4]));
            }
            return result;
        }

        public static object RunFunction(Func<object, object, object, object, object> method, List<Port> ports)
        {

            var properties = method.GetType().GetProperties();

            var result = new List<object>();
            var values = new List<Tuple<int, object>>();

            StrechArgumentLists(ports, values);

            //Get highest value == longest list count
            int Highest = values.OrderBy(x => x.Item1).Reverse().ElementAt(0).Item1;



            for (int i = 0; i < Highest; i++)
            {
                var ActualArgList = new List<object>();
                foreach (var item in values)
                {
                    object currentArg = null;
                    if (item.Item1 > 1 || MainTools.IsList(item.Item2))
                    {
                        var tempArgList = (List<object>)item.Item2;
                        //get latest object

                        var currentArgListLenght = tempArgList.Count - 1;
                        currentArg = currentArgListLenght <= i ? tempArgList[currentArgListLenght] : tempArgList[i];
                        ActualArgList.Add(currentArg);
                    }
                    else
                    {
                        currentArg = item.Item2;
                        ActualArgList.Add(currentArg);
                    }
                }

                result.Add(method.DynamicInvoke(ActualArgList[0], ActualArgList[1], ActualArgList[2], ActualArgList[3]));
            }
            return result;
        }

        public static object RunFunction(Func<object, object, object, object> method, List<Port> ports)
        {

            var properties = method.GetType().GetProperties();

            var result = new List<object>();
            var values = new List<Tuple<int, object>>();

            StrechArgumentLists(ports, values);

            //Get highest value == longest list count
            int Highest = values.OrderBy(x => x.Item1).Reverse().ElementAt(0).Item1;



            for (int i = 0; i < Highest; i++)
            {
                var ActualArgList = new List<object>();
                foreach (var item in values)
                {
                    object currentArg = null;
                    if (item.Item1 > 1 || MainTools.IsList(item.Item2))
                    {
                        var tempArgList = (List<object>)item.Item2;
                        //get latest object

                        var currentArgListLenght = tempArgList.Count - 1;
                        currentArg = currentArgListLenght <= i ? tempArgList[currentArgListLenght] : tempArgList[i];
                        ActualArgList.Add(currentArg);
                    }
                    else
                    {
                        currentArg = item.Item2;
                        ActualArgList.Add(currentArg);
                    }
                }

                result.Add(method.DynamicInvoke(ActualArgList[0], ActualArgList[1], ActualArgList[2]));
            }
            return result;
        }

        public static object RunFunction(Func<object, object, object> method, List<Port> ports)
        {

            var properties = method.GetType().GetProperties();
            var values = new List<Tuple<int, object>>();

            var result = new List<object>();

            StrechArgumentLists(ports, values);

            //Get highest value == longest list count
            int Highest = values.OrderBy(x => x.Item1).Reverse().ElementAt(0).Item1;

            for (int i = 0; i < Highest; i++)
            {
                var ActualArgList = new List<object>();
                foreach (var item in values)
                {
                    object currentArg = null;
                    if (item.Item1 > 1 || MainTools.IsList(item.Item2))
                    {
                        var tempArgList = (List<object>)item.Item2;
                        //get latest object

                        var currentArgListLenght = tempArgList.Count - 1;
                        currentArg = currentArgListLenght <= i ? tempArgList[currentArgListLenght] : tempArgList[i];
                        ActualArgList.Add(currentArg);
                    }
                    else
                    {
                        currentArg = item.Item2;
                        ActualArgList.Add(currentArg);
                    }
                }
                var m = method.DynamicInvoke(ActualArgList[0], ActualArgList[1]);
                result.Add(m);
            }
            return result;
        }

        public static object RunFunction(Func<object, object> method, List<Port> ports)
        {

            var properties = method.GetType().GetProperties();
            var values = new List<Tuple<int, object>>();

            var result = new List<object>();

            //Fill values list
            StrechArgumentLists(ports, values);

            //Get highest value == longest list count
            int Highest = values.OrderBy(x => x.Item1).Reverse().ElementAt(0).Item1;



            for (int i = 0; i < Highest; i++)
            {
                var ActualArgList = new List<object>();
                foreach (var item in values)
                {
                    object currentArg = null;
                    if (item.Item1 > 1 || MainTools.IsList(item.Item2))
                    {
                        var tempArgList = (List<object>)item.Item2;
                        //get latest object

                        var currentArgListLenght = tempArgList.Count - 1;
                        currentArg = currentArgListLenght <= i ? tempArgList[currentArgListLenght] : tempArgList[i];
                        ActualArgList.Add(currentArg);
                    }
                    else
                    {
                        currentArg = item.Item2;
                        ActualArgList.Add(currentArg);
                    }
                }

                result.Add(method.DynamicInvoke(ActualArgList[0]));
            }
            return result;
        }

        public static List<object> RunFunction(Delegate method, TUM.CMS.VplControl.Core.Port[] port, params object[] args)
        {

            var properties =  method.GetType().GetProperties();
            var values = new List<Tuple<int, object>>();

            var result = new List<object>();

            //Fill values list
            foreach (var arg in args)
            {
                   
                    var argList = (System.Collections.IList)arg;
                    //Check if argument is a list and include it on a Tuple with its count number
                    var argResult = argList != null ? new Tuple<int,object>( 1, arg ) :new Tuple<int, object>( argList.Count, argList );
                    //if args are list, count the number of object to set how many times it should run
                    values.Add(argResult);
                
            }

            //Get highest value == longest list count
            int Highest = values.OrderBy(x => x.Item1).Reverse().ElementAt(0).Item1;

           

            for (int i = 0; i < Highest; i++)
            {
                var ActualArgList = new List<object>();
                foreach (var item in values)
                {
                    object currentArg = null;
                    if (item.Item1 > 1)
                    {
                        var tempArgList = (List<object>)item.Item2;
                        //get latest object

                        var currentArgListLenght = tempArgList.Count -1;
                        currentArg = currentArgListLenght <= i? tempArgList[currentArgListLenght]: tempArgList;

                    }
                    else
                    {
                        currentArg = item.Item2;
                    }
                }

                result.Add(method.DynamicInvoke(ActualArgList));
            }
            return result;
  
        }

        /// <summary>
        /// Check if object can contain type - Used after IsList()
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static bool ListContainsType(object obj, Type type)

        {
            bool output = ((System.Collections.IList)obj)[0].GetType() == type;
            
            return output;
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
