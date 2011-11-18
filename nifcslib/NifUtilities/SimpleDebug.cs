using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nifcslib.NifTypes;
using System.Reflection;
using System.Collections;

namespace nifcslib.NifUtilities
{
    public class SimpleDebug
    {
        #region variable declarations
        private static int _callcount = 0;
        private static bool _emptycheck = false;
        private static bool _containscheck = false;
        private static string _containsstring = String.Empty;
        #endregion

        #region Function Declarations
        public static void PerformChecks()
        {
            Dictionary<string, List<string>> lists = new Dictionary<string, List<string>>();
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        NifUtilities.SimpleDebug.emptycheck = true;
                        lists.Add("Empty", ProcessDebugQuery());
                        continue;
                    case 1:
                        NifUtilities.SimpleDebug.emptycheck = false;
                        NifUtilities.SimpleDebug.containscheck = true;
                        NifUtilities.SimpleDebug.containsstring = "Unknown";
                        lists.Add("Unknown", ProcessDebugQuery());
                        continue;
                    case 2:
                        NifUtilities.SimpleDebug.containsstring = "?";
                        lists.Add("?",ProcessDebugQuery());
                        continue;
                    default:
                        continue;
                }
            }
        }

        private static List<string> ProcessDebugQuery()
        {
            List<string> list = new List<string>();
            list.AddRange(PerformPropertyCheck(NifDataHolder.getInstance().basiclist, "description"));
            list.AddRange(PerformPropertyCheck(NifDataHolder.getInstance().bitflagitemlist, "description"));
            list.AddRange(PerformPropertyCheck(NifDataHolder.getInstance().compoundlist, "description"));
            list.AddRange(PerformPropertyCheck(NifDataHolder.getInstance().compoundtemplatelist, "description"));
            list.AddRange(PerformPropertyCheck(NifDataHolder.getInstance().enumitemlist, "description"));
            list.AddRange(PerformPropertyCheck(NifDataHolder.getInstance().niobjectlist, "description"));
            list.AddRange(PerformPropertyCheck(NifDataHolder.getInstance().versionlist, "description"));
            list.AddRange(PerformOptionPropertyCheck(NifDataHolder.getInstance().bitflagitemlist, "optionlist", "description"));
            list.AddRange(PerformOptionPropertyCheck(NifDataHolder.getInstance().compoundlist, "addlist", "description"));
            list.AddRange(PerformOptionPropertyCheck(NifDataHolder.getInstance().enumitemlist, "optionlist", "description"));
            list.AddRange(PerformOptionPropertyCheck(NifDataHolder.getInstance().compoundtemplatelist, "addlist", "description"));
            Console.WriteLine(NifUtilities.SimpleDebug.callcount);
            return list;
        }
        
        public static List<string> PerformPropertyCheck<T>(Dictionary<string, T> dict, string property)
        {
            List<string> list = new List<string>();
            foreach (KeyValuePair<string, T> item in dict)
            {
                string propstring = item.Value.GetType().GetProperty(property).GetGetMethod().Invoke(item.Value, null).ToString();
                if (checkString(propstring))
                {
                    string name = item.Value.GetType().GetProperty("name").GetGetMethod().Invoke(item.Value, null).ToString();
                    list.Add(name + ": " + propstring);
                    _callcount++;
                }
            }
            return list;
        }

        public static List<string> PerformOptionPropertyCheck<T>(Dictionary<string, T> dict, string listname, string property)
        {
            List<string> list = new List<string>();
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
                            list.Add(objname + ": " + name + ": " + propstring);
                            _callcount++;
                        }
                    }
                }
            }
            return list;
        }

        private static Object CreateGenericList(Type typeX)
        {
            Type listType = typeof(List<>);
            Type[] typeArgs = { typeX };
            Type genericType = listType.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(genericType);
            return o;
        }

        
        #endregion

        #region Property Accessors
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
        #endregion

    }
}

