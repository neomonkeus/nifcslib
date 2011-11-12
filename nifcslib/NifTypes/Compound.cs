using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nifcslib.NifTypes
{
    public class Compound
    {
        #region Variable Declarations
        private string _name = "";
        private string _niflibtype = "";
        private string _nifskopetype = "";
        private string _description = "";
        private string _istemplate = "";
        private List<Add> _addlist = new List<Add>();
        #endregion

        #region Property Accessors
        public string name
        {
            get
            {
                return _name;
            }
            set
            {
                _name = value.Substring(0, 1).ToUpper() + value.Substring(1).Replace(" ", ""); ;
            }
        }

        public string description
        {
            get
            {
                return _description;
            }
            set
            {
                _description = value;
            }
        }

        public string niflibtype
        {
            get
            {
                return _niflibtype;
            }
            set
            {
                _niflibtype = value;
            }
        }

        public string nifskopetype
        {
            get
            {
                return _nifskopetype;
            }
            set
            {
                _nifskopetype = value;
            }
        }

        public List<Add> addlist
        {
            get
            {
                return _addlist;
            }
            set
            {
                _addlist = value;
            }
        }

        public string istemplate
        {
            //"1" == "true", "0" == "false"
            get
            {
                return _istemplate.CompareTo("1") == 0 ? "true": "false";
            }
            set
            {
                _istemplate = value;
            }
        }

        #endregion

        #region Functions
        public object Clone()
        {
            Compound clone = new Compound();
            clone.description = _description;
            clone.istemplate = _istemplate;
            clone.name = _name;
            clone.niflibtype = _niflibtype;
            clone.nifskopetype = _nifskopetype;
            //clone.addlist = new List<Add>();

            foreach(Add item in _addlist)
            {
                clone._addlist.Add(item.Clone());
            }

            return clone;
        }

        public void AddAdd(string name, string description, string version1, string version2,
            string type, string array1, string array2, string array3, string defaultvalue, string template,
            string userversion, string condition, string arguement)
        {
            {
                Add add = new Add();
                add.name = name;
                add.nameindex = name;
                add.template = template;
                add.description = description;
                add.array1 = array1;
                add.array2 = array2;
                add.array3 = array3;
                add.version1 = add.versionConvert(version1);
                add.version2 = add.versionConvert(version2);
                add.type = type;
                add.returntype = type;
                add.template = template;
                add.defaultvalue = defaultvalue;
                add.userversion = userversion;
                add.condition = condition;
                add.arguement = arguement;

                if (array2.Length != 0)
                {
                    foreach (Add compareitem in _addlist)
                    {
                        if (compareitem.name.CompareTo(add.array2) == 0)
                        {
                            if (compareitem.array1.Length != 0)
                            {
                                add.isarray2array = true;
                            }
                        }
                    }
                }

                if (array3.Length != 0)
                {
                    foreach (Add compareitem in _addlist)
                    {
                        if (compareitem.name.CompareTo(add.array3) == 0)
                        {
                            if (compareitem.array1.Length != 0)
                            {
                                add.isarray3array = true;
                            }
                        }
                    }
                }
                _addlist.Add(add);
            }
        }

        public void setAddTypesForTemplate(String typetochange)
        {
            foreach(Add add in _addlist)
                if (add.type.CompareTo("TEMPLATE") == 0)
                {
                    add.type = typetochange;
                    add.returntype = typetochange;
                }
        }

        public String toString()
        {
            String s = "<Name>" + name + "<Niflibtype>" + niflibtype + "<Nifskopetype>" + 
                nifskopetype + "<Description>" + description + "<>";

            int i = 0;
            foreach (Add add in _addlist)
            {
                s = s + "<Add>" + i + "<name>" + add.name +"<>";
                i++;
            }
            return s;
        }
        #endregion
    }
}
