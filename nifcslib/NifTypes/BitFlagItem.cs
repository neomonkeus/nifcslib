using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nifcslib.NifTypes
{
    public class BitFlagItem
    {
        #region Variable Declarations
        private string _name = "";
        private string _description = "";
        private string _storage = "";
        private List<BitFlagItemOption> _optionlist = new List<BitFlagItemOption>();
        #endregion

        #region Property Accessors
        public string name
        {
            set
            {
                _name = value.Substring(0, 1).ToUpper() + value.Substring(1).Replace(" ", "");
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

        public string storage
        {
            set
            {
                _storage = value;
            }
            get
            {
                return _storage;
            }
        }

        public List<BitFlagItemOption> optionlist
        {
            set
            {
                _optionlist = value;
            }
            get
            {
                return _optionlist;
            }
        }
        #endregion

        #region Function Declaration
        public void addOption(String name, String description, int value)
        {
            BitFlagItemOption item = new BitFlagItemOption();
            item.name = name;
            item.description = description;
            item.value = value;

            _optionlist.Add(item);
        }


        public override string ToString()
        {
            String s = "";
            s = s + "Name [" + name + "] Description [" + description + "] Storage [ " + storage + "]";

            foreach (BitFlagItemOption item in optionlist)
            {
                s = s + item.ToString();
            }

            return s;
        }
        #endregion
    }
}
