using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nifcslib.NifTypes;
using System.Reflection;
using System.Collections;
using System.IO;

namespace nifcslib.NifUtilities
{
    public class SimpleDebug
    {
        #region variable declarations
        private static int _callcount;
        private static bool _equalscheck;
        private static bool _containscheck;
        private static string _containsstring;
        private static string _equalsstring;
        private static Dictionary<string, List<string>> lists;
        #endregion

        #region Function Declarations
        
        public SimpleDebug()
        {
            _callcount = 0;
            _equalscheck = false;
            _containscheck = false;
            _containsstring = String.Empty;
            _equalsstring = String.Empty;
            lists = new Dictionary<string, List<string>>();
        }
        
        public void PerformChecks()
        {
            for (int i = 0; i < 3; i++)
            {
                switch (i)
                {
                    case 0:
                        _equalscheck = true;
                        _equalsstring = "";
                        lists.Add("Empty", ProcessDebugQuery());
                        _equalscheck = false;
                        continue;
                    case 1:
                        
                        _containscheck = true;
                        _containsstring = "Unknown";
                        lists.Add("Unknown", ProcessDebugQuery());
                        continue;
                    case 2:
                        _containsstring = "?";
                        lists.Add("Question", ProcessDebugQuery());
                        continue;
                    default:
                        continue;
                }
            }
            CreateLogFiles();
        }

        private void CreateLogFiles()
        {
            List<string> list;
            foreach(KeyValuePair<string, List<string>> keypairs in lists)
	        {
                using (StreamWriter writer = new StreamWriter(".\\" + keypairs.Key + ".txt"))
                {
                    list = keypairs.Value.ToList<string>();
                    for (int i = 0; i < keypairs.Value.Count; i++)
                    {
                        writer.Write(list[i].Replace("\n", writer.NewLine.ToString()));
                        writer.Write(writer.NewLine);
                    }
                }
            }
        }

        private List<string> ProcessDebugQuery()
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
            Console.WriteLine(_callcount);
            return list;
        }
        
        private List<string> PerformPropertyCheck<T>(Dictionary<string, T> dict, string property)
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

        private List<string> PerformOptionPropertyCheck<T>(Dictionary<string, T> dict, string listname, string property)
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

        private Object CreateGenericList(Type typeX)
        {
            Type listType = typeof(List<>);
            Type[] typeArgs = { typeX };
            Type genericType = listType.MakeGenericType(typeArgs);
            object o = Activator.CreateInstance(genericType);
            return o;
        }

        
        #endregion

        #region Property Accessors
        public int callcount
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

        public string containsstring
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

        public bool emptycheck
        {
            get
            {
                return _equalscheck;
            }
            set
            {
                _equalscheck = value;
            }
        }
        
        public bool containscheck
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

        public bool checkString(string comparison)
        {
            if (_equalscheck)
                if(comparison.Trim().Equals(_equalsstring))
                    return true;
            if (_containscheck)
                if (comparison.Contains(_containsstring))
                    return true;
            return false;
        }
        #endregion

    }
}

