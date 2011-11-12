using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nifcslib.Niftypes
{
    class BitFlagItemOption
    {
        #region Variable Declarations
        private string _name = "";
        private string _description = "";
        private int _value = -1;
        #endregion

        #region Property Accessors
        public string name
        {
            set
            {
                _name = value.ToUpper().Replace(" ", "_");
            }
            get
            {
                return _name;
            }
        }

        public string description
        {
            set
            {
                _description = value;
            }
            get
            {
                return _description;
            }
        }

        public int value
        {
            set
            {
                _value = value;
            }

            get
            {
                return _value;
            }
        }
        #endregion

        #region Function Declaration
        public override string ToString()
        {
            string s = "";
            s = s + "Name [" + name + "] Description [" + description + "] Value [ " + value + "]";
            return s;
        }
        #endregion
    }
}
