using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nifcslib.NifTypes
{
    public class Add
    {
        #region Variable Declarations  
        private string _name = "";
        private string _nameindex = "";
        private string _description = "";
        private int _version1 = 0;
        private int _version2 = 0;
        private string _versioncondition = "";
        private string _userversion = "";
        private string _type = "";
        private string _originaltype = "";
        private string _returntype = "";
        private string _array1 = "";
        private string _array2 = "";
        private bool _array2array = false;
        private string _array3 = "";
        private bool _array3array = false;
        private string _defaultvalue = "";
        private string _template = "";
        private string _condition = "";
        private string _arguement = "";    
        #endregion

        #region Property Accessors
        public string name
        {
            set
            {
                _name = value.Substring(0, 1).ToLower() + value.Substring(1).Replace(" ", "").Replace("\\?", "TEST");
            }
            get
            {
                return _name;
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

        public int version1
        {
            set
            {
                _version1 = value;
            }
            get
            {
                return _version1;
            }

        }

        public int version2
        {
            set
            {
                _version2 = value;
            }
            get
            {
                return _version2;
            }

        }

        public string type
        {
            get
            {
                return _type;
            }
            set
            {
                _originaltype = value;
                _type = Utilies.convertType(value, template);
            }
        }

        public String originaltype
        {
            get
            {
                return _originaltype;
            }
        }

        public String array1
        {
            get
            {
                return _array1;
            }
            set
            {
                if (array1.Length == 0)
                {
                    return;
                }
                int result;
                if (int.TryParse(array1, out result))
                {
                    _array1 = array1;
                }
                else
                {
                    _array1 = findVariableAndConvert(array1, false);
                }

            }
        }

        public string array2
        {
            get
            {
                return _array2;
            }
            set
            {
                if (array2.Length == 0)
                {
                    return;
                }
                int result;
                if (int.TryParse(array2, out result))
                {
                    _array2 = array2;
                }
                else
                {
                    _array2 = findVariableAndConvert(array2, false);
                }

            }
        }

        public String array3
        {
            get
            {
                return _array3;
            }
            set
            {
                if (array3.Length == 0)
                {
                    return;
                }
                int result;
                if (int.TryParse(array3, out result))
                {
                    _array3 = array3;
                }
                else
                {
                    _array3 = findVariableAndConvert(array3, false);
                }

            }
        }

        public string template
        {
            get
            {
                return _template;
            }
            set
            {
                _template = value;
            }
        }

        public string defaultvalue
        {
            get
            {
                return _defaultvalue;
            }
            set
            {
                _defaultvalue = value;
            }
        }

        public string nameindex
        {
            get
            {
                return _nameindex;
            }
            set
            {
                _nameindex = value.Substring(0, 1).ToLower() + value.Substring(1).Replace(" ", "").Replace("\\?", "TEST");
            }
        }

        public string userversion
        {
            get
            {
                return _userversion;
            }
            set
            {
                if (value.Length == 0)
                {
                    _userversion = value;
                }
                else
                {
                    _userversion = "header.userversion == " + value;
                }
            }
        }

        public string returntype
        {
            get
            {
                return _returntype;
            }
            set
            {
                _returntype = Utilies.convertReturnType(value, template, array1.Trim().Length != 0);
            }
        }

        public bool isarray2array
        {
            get
            {
                return _array2array;
            }
            set
            {
                _array2array = value;
            }
        }

        public bool isarray3array
        {
            get
            {
                return _array3array;
            }
            set
            {
                _array3array = value;
            }
        }

        public string arguement
        {
            get
            {
                return _arguement;
            }
            set
            {
                if (arguement.Length != 0)
                {
                    int result;
                    if (int.TryParse(arguement, out result))
                    {
                        _arguement = value;
                    }
                    else
                    {
                        _arguement = value.Substring(0, 1).ToLower() + value.Substring(1).Replace(" ", "").Replace("\\?", "TEST");
                    }
                }
            }
        }

        public String versioncondition
        {
            get
            {
                return _versioncondition;
            }
            set
            {
                if (value.Length == 0)
                {
                    return;
                }
                else
                {
                    _versioncondition = value.Replace("User Version 2", "header.userversion2");
                    _versioncondition = value.Replace("User Version", "header.userversion");

                    int locationVersionGreaterEqual = value.IndexOf("Version >=");

                    while (locationVersionGreaterEqual != -1)
                    {
                        // we have a version condition with a Version, its in a non standard format so we need to get it and convert it
                        // find the ) after
                        // add 10 to move past the whole text
                        locationVersionGreaterEqual = locationVersionGreaterEqual + 10;
                        int bracket = value.IndexOf(")", locationVersionGreaterEqual);
                        
                        value = Utilies.ReplaceFirstOccurance(value, value.Substring(locationVersionGreaterEqual, bracket), "" + versionConvert(value.Substring(locationVersionGreaterEqual, bracket)));
                        value = Utilies.ReplaceFirstOccurance(value, "Version >=", "int.Parse(header.version) >=");
                        locationVersionGreaterEqual = value.IndexOf("Version >=");
                    }

                    int locationVersionEqual = value.IndexOf("Version ==");

                    while (locationVersionEqual != -1)
                    {
                        // we have a version condition with a Version, its in a non standard format so we need to get it and convert it
                        // find the ) then add 10 to move past the whole text
                        locationVersionEqual = locationVersionEqual + 10;
                        int bracket = value.IndexOf(")", locationVersionEqual);
                        value = Utilies.ReplaceFirstOccurance(value, value.Substring(locationVersionEqual, bracket), "" + versionConvert(value.Substring(locationVersionEqual, bracket)));
                        value = Utilies.ReplaceFirstOccurance(value, "Version ==", "int.Parse(header.version)) ==");
                        locationVersionEqual = value.IndexOf("Version ==");
                    }

                    _versioncondition = value;
                }
            }
        }

        public string condition
        {
            get
            {
                if (_condition.Length != 0 && _versioncondition.Length != 0 && _userversion.Length == 0)
                {
                    return _condition + " && " + _versioncondition;
                }
                else if (_condition.Length != 0 && _versioncondition.Length == 0 && _userversion.Length != 0)
                {
                    return _condition + " && " + _userversion;
                }
                else if (_condition.Length == 0 && _versioncondition.Length != 0 && _userversion.Length != 0)
                {
                    return _userversion + " && " + _versioncondition;
                }
                else if (_condition.Length != 0 && _versioncondition.Length != 0 && _userversion.Length != 0)
                {
                    return _condition + " && " + _userversion + " && " + _versioncondition;
                }
                else if (_condition.Length == 0 && _versioncondition.Length != 0 && _userversion.Length == 0)
                {
                    return _versioncondition;
                }
                else if (_condition.Length == 0 && _versioncondition.Length == 0 && _userversion.Length != 0)
                {
                    return _userversion;
                }
                else if (_condition.Length != 0 && _versioncondition.Length == 0 && _userversion.Length == 0)
                {
                    return _condition;
                }
                else
                {
                    return "";
                }
            }
            set
            {
                if (condition.Length == 0)
                {
                    _condition = "";
                }
                else
                {
                    //_condition = findVariableAndConvert(condition, true);
                    _condition = condition;
                }
            }

        }
        #endregion

        #region Functions
        public Add Clone()
        {
            return (Add)this.MemberwiseClone();
        }

        public override string ToString()
        {
            return "<Name>" + name + "<Description>" + description + "<Type>" + type + "<Version1>"
                 + version1 + "<Version2>" + version2 + "<Version Condition>" + versioncondition
                 + "<User Version>" + userversion + "<Array1>" + array1 + "<Array2>" + array2
                 + "<array3>" + array3 + "<Template>" + template + "<DefaultValue>"
                 + defaultvalue + "<Condition>" + condition + "</>";
        }

        public string getFunctionName()
        {
            return _name.Substring(0, 1).ToUpper() + _name.Substring(1);
        }

        public int versionConvert(String version)
        {
            int vers = 0;
            String[] versionSplit = version.Split('.');
            String versionString = "";
            for (int i = 0; i < versionSplit.Length; i++)
            {
                String bitOfString = versionSplit[i];
                if (bitOfString.Length == 1)
                {
                    bitOfString = "0" + bitOfString;
                }
                versionString = versionString + bitOfString;
            }

            if (versionString.Trim().Length != 0)
            {
                vers = int.Parse(versionString.Trim());
            }

            return vers;
        }
        /*
        private string findVariableAndConvert(string original, bool makebool)
        {
            // this tries to find variables and make them lower case to match the 
            // way we store variables

            // first remove all the spaces
            original = original.Replace(" ", "");

            int bracket = original.IndexOf("(");

            if (bracket == -1)
            {
                original = original.Substring(0, 1).ToLower() + original.Substring(1);
                if (original.IndexOf('&') != -1)
                {
                    // we hava a bit wise operation, lets equate to not zero for true
                    original = "(" + original + ")!=0";
                }
            }

            while (bracket != -1)
            {

                if (original.Substring(bracket + 1, bracket + 2).CompareTo("(") == 0)
                {
                    bracket = original.IndexOf("(", bracket + 1);
                }
                else
                {
                    original = original.Substring(0, bracket + 1) + original.Substring(bracket + 1, bracket + 2).ToLower() + original.Substring(bracket + 2);
                    int openBracket = original.IndexOf("(", bracket + 1);
                    int closebracket = original.IndexOf(")", bracket + 1);

                    if (closebracket < openBracket || (openBracket == -1 && closebracket != -1))
                    {
                        // we are at a root part of the brackets (ie where a boolean expression should be
                        if (original.Substring(bracket + 1, closebracket).IndexOf('&') != -1 && makebool)
                        {
                            // we hava a bit wise operation, lets equate to not zero for true
                            original = original.Substring(0, bracket + 1) + "(" + original.Substring(bracket + 1, closebracket) + ")!=0" + original.Substring(closebracket);
                            bracket = original.IndexOf("(", bracket + 1);
                        }

                        bracket = original.IndexOf("(", bracket + 1);
                    }
                    else
                    {
                        bracket = original.IndexOf("(", bracket + 1);
                    }
                }
            }
            return original.Replace("\\?", "TEST");
        }
        */


        private String findVariableAndConvert(String original, bool makebool)
        {
            // this tries to find variables and make them lower case to match the 
            // way we store variables

            // first remove all the spaces
            original = original.Replace(" ", "");

            int bracket = original.IndexOf("(");

            if (bracket == -1)
            {
                original = original.Substring(0, 1).ToLower() + original.Substring(1);
                if (original.IndexOf('&') != -1)
                {
                    // we hava a bit wise operation, lets equate to not zero for true
                    original = "(" + original + ")!=0";
                }
            }

            while (bracket != -1)
            {

                if (original.Substring(bracket + 1, bracket + 2).CompareTo("(") == 0)
                {
                    bracket = original.IndexOf("(", bracket + 1);
                }
                else
                {
                    original = original.Substring(0, bracket + 1) + original.Substring(bracket + 1, bracket + 2).ToLower() + original.Substring(bracket + 2);
                    int openBracket = original.IndexOf("(", bracket + 1);
                    int closeBracket = original.IndexOf(")", bracket + 1);

                    if (closeBracket < openBracket || (openBracket == -1 && closeBracket != -1))
                    {
                        // we are at a root part of the brackets (ie where a boolean expression should be
                        if (original.Substring(bracket + 1, closeBracket).IndexOf('&') != -1 && makebool)
                        {
                            // we hava a bit wise operation, lets equate to not zero for true
                            original = original.Substring(0, bracket + 1) + "(" + original.Substring(bracket + 1, closeBracket) + ")!=0" + original.Substring(closeBracket);
                            bracket = original.IndexOf("(", bracket + 1);
                        }

                        bracket = original.IndexOf("(", bracket + 1);
                    }
                    else
                    {
                        bracket = original.IndexOf("(", bracket + 1);
                    }
                }

            }

            return original.Replace("\\?", "TEST");
        }
        #endregion
    }
}
