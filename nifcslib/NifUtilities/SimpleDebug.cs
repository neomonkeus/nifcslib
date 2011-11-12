using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nifcslib.NifTypes;
using System.Reflection;
using System.Collections;

namespace nifcslib.NifUtilities
{
    public static class SimpleDebug
    {
        private static int _callcount = 0;
        private static bool _emptycheck = false;
        private static bool _containscheck = false;
        private static string _containsstring = String.Empty;
        
        public static void PerformDescriptionCheck<T>(Dictionary<string, T> dict, string property)
        {
            foreach (KeyValuePair<string, T> item in dict)
            {
                string propstring = item.Value.GetType().GetProperty(property).GetGetMethod().Invoke(item.Value, null).ToString();
                if (checkString(propstring))
                {
                    string name = item.Value.GetType().GetProperty("name").GetGetMethod().Invoke(item.Value, null).ToString();
                    Console.WriteLine(name + ": " + propstring);
                    _callcount++;
                }
            }
        }

        public static void PerformOptionDescriptionCheck<T>(Dictionary<string, T> dict, string listname, string property)
        {
            foreach (KeyValuePair<string, T> item in dict)
            {
                //obtain the runtime listtype.
                object obj = CreateGenericList(item.Value.GetType().GetProperty(listname).GetGetMethod().Invoke(item.Value, null).GetType());
                obj = item.Value.GetType().GetProperty(listname).GetGetMethod().Invoke(item.Value, null);
                string objname = item.Value.GetType().GetProperty("name").GetGetMethod().Invoke(item.Value, null).ToString();
                
                if (obj is IEnumerable)
                {
                    foreach (object o in (obj as IEnumerable))
                    {
                        string propstring = o.GetType().GetProperty(property).GetGetMethod().Invoke(o, null).ToString();
                        if (checkString(propstring))
                        {
                            string name = o.GetType().GetProperty("name").GetGetMethod().Invoke(o, null).ToString();
                            Console.WriteLine(objname + ": " + name + ": " + propstring);
                            _callcount++;
                        }
                    }
                }
            }
        }

        private static Object CreateGenericList(Type typeX)
        {
            Type listType = typeof(List<>);
            Type[] typeArgs = { typeX };
            Type genericType = listType.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(genericType);
            return o;
        }

        public static int callcount
        {
            set
            {
                value = _callcount;
            }
            get
            {
                return _callcount;
            }
        }

        public static string containsstring
        {
            set
            {
                //see if the node contain this string
                _containsstring = value;
            }
            get
            {
                return _containsstring;
            }
        }

        public static bool emptycheck
        {
            get
            {
                return _emptycheck;
            }
            set
            {
                _emptycheck = value;
            }
        }
        
        public static bool containscheck
        {
            get
            {
                return _containscheck;
            }
            set
            {
                _containscheck = value;
            }
        }

        public static bool checkString(string comparison)
        {
            if(_emptycheck)
                if(comparison.Trim().Equals(""))
                    return true;
            if (_containscheck)
                if (comparison.Contains(_containsstring))
                    return true;
            return false;
        }
    }
}

