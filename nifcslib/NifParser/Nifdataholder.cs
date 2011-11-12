using System;
using System.Collections.Generic;
using System.Collections;
using System.Linq;
using System.Text;
using nifcslib.NifTypes;

namespace nifcslib
{
    public class NifDataHolder
    {
        private Dictionary<string, NifTypes.Version> _versionlist;
        private Dictionary<string, Basic> _basiclist;
        private Dictionary<string, EnumItem> _enumItemList; 
        private Dictionary<string, BitFlagItem> _bitFlagItemList; 
        private Dictionary<string, Compound> _compoundList;
        private Dictionary<string, Compound>  _compoundTemplateList;
        private Dictionary<string, Niobject> _niobjectList;

        private static NifDataHolder instance = null;

        private NifDataHolder()
        {
            _versionlist = new Dictionary<String, NifTypes.Version>();
            _basiclist = new Dictionary<String, Basic>();
            _enumItemList = new Dictionary<string, EnumItem>();
            _bitFlagItemList = new Dictionary<string, BitFlagItem>();
            _compoundList = new Dictionary<string, Compound>();
            _compoundTemplateList = new Dictionary<string, Compound>();
            _niobjectList = new Dictionary<string, Niobject>();
        }

        public static NifDataHolder getInstance()
        {
            if (instance == null)
            {
                instance = new NifDataHolder();
            }
            return instance;
        }

        public Dictionary<String, NifTypes.Version> versionlist
        {
            get
            {
                return _versionlist;
            }
        }

        public Dictionary<String, Basic> basiclist
        {
            get
            {
                return _basiclist;
            }
        }

        public Dictionary<String, EnumItem> enumitemlist
        {
            get
            {
                return _enumItemList;
            }
        }


        public Dictionary<String, BitFlagItem> bitflagitemlist
        {
            get
            {
                return _bitFlagItemList;
            }
        }


        public Dictionary<String, Compound> compoundlist
        {
            get
            {
                return _compoundList;
            }
        }


        public Dictionary<String, Compound> compoundtemplatelist
        {
            get
            {
                return _compoundTemplateList;
            }
        }


        public Dictionary<String, Niobject> niobjectlist
        {
            get
            {
                return _niobjectList;
            }
        }

    }
}