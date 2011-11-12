using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nifcslib.NifTypes
{
    public class Basic
    {
        #region variable declarations
        private string _name = "";
        private int _count = 0;
        private string _niflibtype = "";
        private string _nifskopetype = "";
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

        public int count
        {
            set
            {
                _count = value;
            }
            get
            {
                return _count;
            }
        }
        
        public string niflibtype 
        {
            set
            {
                _niflibtype = value;
            }
            get
            {
                return _niflibtype;
            }
        }
        
        public string nifskopetype
        {
            set
            {
                _nifskopetype = value;
            }
            get
            {
                return _nifskopetype;
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

        #region Function Declarations
        public override string ToString()
        {
 	        return "Name [" + name + "] Count [" + count + "] Niflibtype [" + niflibtype + "] Nifskopetype [" + nifskopetype + "] Description [" + description + "]";
        }
        #endregion
    }
}
