using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nifcslib.NifTypes
{
    public class Niobject
    {
        #region Variable Declarations
        private String _name = "";
        private String _isabstract = "";
        private String _inherit = "";
        private String _description = "";
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
                _name = value.Substring(0, 1).ToUpper() + value.Substring(1).Replace(" ", "");
            }
        }

        public string isabstract
        {           
            get
            {
                return _isabstract;
            }
            set
            {
                _isabstract = value;
            }
        }

        public string inherit
        {
            get
            {
                return _inherit;
            }
            set
            {
                if (value.Length > 0)
                {
                    _inherit = value.Substring(0, 1).ToUpper() + value.Substring(1).Replace(" ", "");
                }
                else
                {
                    _inherit = inherit;
                }
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
        #endregion

        #region Functions
        public override string ToString()
        {
            String s = "<Name>" + name + "<IsAbstract>" + isabstract + 
                "<Inherit>" + inherit + "<Description>" + description + "<>";

            int i = 0;
            foreach (Add add in _addlist)
            {
                s = s + "<Add>" + i + "<name>" + add.name + "<>";
                i++;
            }
            return s;
        }

        public void AddAdd(string name, string description, string version1, string version2,
            string type, string array1, string array2, string array3, string defaultvalue, string template,
            string userversion, string condition, string versioncondition, string arguement)
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
                add.versioncondition = versioncondition;
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
                _addlist.Add(add);
            }

        }
        #endregion
    }
}
