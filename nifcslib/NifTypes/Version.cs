using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nifcslib.NifTypes
{
    public class Version
    {
        #region Variable Declarations
        private string _name = "";
        private string _description = "";
        #endregion

        #region Property Accessors
        public string name
        { 
            set
            {
                _name = value;
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
        #endregion

        #region Function Declaration
        public override string ToString()
        {
            return "Version [" + name + "] Games using this version [" + description + "]";
        }
        #endregion
    }
}
