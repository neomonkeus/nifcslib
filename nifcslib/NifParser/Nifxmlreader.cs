using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using nifcslib.NifTypes;
using nifcslib.NifUtilities;
using System.Globalization;

namespace nifcslib
{
    class Nifxmlreader
    {
        private string _xmlpath;
        private DateTime _begintime, _endtime;
        private TimeSpan _processtime;
        private XmlDocument _doc;

        #region Function Declarations
        public Nifxmlreader(string path)
        {
            _xmlpath = path;
            _doc = new XmlDocument();
        }

        public void LoadXml()
        {
            XmlReaderSettings settings = new XmlReaderSettings { DtdProcessing = DtdProcessing.Parse, IgnoreComments = true, IgnoreWhitespace = true };
            XmlReader _reader = XmlReader.Create(_xmlpath, settings);
            _doc.Load(_reader);
        }

        public void processXml()
        {
            XmlElement root = (XmlElement)_doc.DocumentElement;

            //get list of version elements
            XmlNodeList xmlversionlist = root.GetElementsByTagName("version");
            if (xmlversionlist != null && xmlversionlist.Count > 0)
            {
                foreach (XmlElement xmlversion in xmlversionlist)
                {
                    //get the version object
                    NifTypes.Version version = genversionfromxml(xmlversion);

                    //add it to list
                    NifDataHolder.getInstance().versionlist.Add(version.name, version);
                }
            }

            //get list of vasic elements
            XmlNodeList xmlbasiclist = root.GetElementsByTagName("basic");
            if (xmlbasiclist != null && xmlbasiclist.Count > 0)
            {
                foreach (XmlElement xmlbasic in xmlbasiclist)
                {
                    //get the basic object
                    Basic basic = genbasicfromxml(xmlbasic);

                    //add it to list
                    NifDataHolder.getInstance().basiclist.Add(basic.name, basic);
                }
            }

            //get list of enum elements
            XmlNodeList xmlenumitemlist = root.GetElementsByTagName("enum");
            if (xmlenumitemlist != null && xmlenumitemlist.Count > 0)
            {
                foreach (XmlElement xmlenumitem in xmlenumitemlist)
                {
                    //get the enumItem object
                    EnumItem enumitem = getEnumItem(xmlenumitem);

                    //add it to list
                    NifDataHolder.getInstance().enumitemlist.Add(enumitem.name, enumitem);
                }
            }

            //get a nodelist of  elements
            XmlNodeList xmlbitflagslist = root.GetElementsByTagName("bitflags");
            if (xmlbitflagslist != null && xmlbitflagslist.Count > 0)
            {
                foreach (XmlElement xmlbitflag in xmlbitflagslist)
                {
                    //get the bitflags object
                    BitFlagItem bitflag = getBitFlagItem(xmlbitflag);

                    //add it to list
                    NifDataHolder.getInstance().bitflagitemlist.Add(bitflag.name, bitflag);

                }
            }

            //get a nodelist of  elements
            XmlNodeList xmlcompoundlist = root.GetElementsByTagName("compound");
            if (xmlcompoundlist != null && xmlcompoundlist.Count > 0)
            {
                foreach (XmlElement xmlcompound in xmlcompoundlist)
                {

                    //get the compound object
                    Compound compound = getCompound(xmlcompound);

                    //add it to list
                    if (bool.Parse(compound.istemplate))
                    {
                        NifDataHolder.getInstance().compoundtemplatelist.Add(compound.name, compound);
                    }
                    else
                    {
                        foreach (Add item in compound.addlist)
                        {
                            checkForTemplatesAndProcess(item);
                        }

                        NifDataHolder.getInstance().compoundlist.Add(compound.name, compound);
                    }
                }
            }


            //get a nodelist of  elements
            XmlNodeList xmlniobjectlist = root.GetElementsByTagName("niobject");
            if (xmlniobjectlist != null && xmlniobjectlist.Count > 0)
            {
                foreach (XmlElement xmlniobject in xmlniobjectlist)
                {
                    //get the niobject object
                    Niobject niobject = getNiobject(xmlniobject);

                    //add it to list
                    NifDataHolder.getInstance().niobjectlist.Add(niobject.name, niobject);

                    //Lets see if we need to create some template clones and also 
                    // change the name of the type


                    foreach (Add item in niobject.addlist)
                    {
                        checkForTemplatesAndProcess(item);
                    }
                }
            }
        }

        private NifTypes.Version genversionfromxml(XmlElement xmlversion)
        {

            // for each <version> element get version details 
            // and what implements it

            NifTypes.Version version = new NifTypes.Version();
            version.name = xmlversion.GetAttribute("num");
            version.description = xmlversion.FirstChild.Value;
            return version;
        }

        private Basic genbasicfromxml(XmlElement xmlbasic)
        {

            // for each <basic> element get basic details 
            // and what implements it
            Basic basic = new Basic();
            basic.name = xmlbasic.GetAttribute("name");
            basic.count = int.Parse(xmlbasic.GetAttribute("count"));
            basic.niflibtype = xmlbasic.GetAttribute("niflibtype");
            basic.nifskopetype = xmlbasic.GetAttribute("nifskopetype");
            basic.description = xmlbasic.FirstChild.Value.Trim();
            return basic;
        }

        private EnumItem getEnumItem(XmlElement xmlenumitem)
        {

            // for each <version> element get basic details 
            // and what implements it
            EnumItem enumitem = new EnumItem();
            enumitem.name = xmlenumitem.GetAttribute("name");
            enumitem.storage = xmlenumitem.GetAttribute("storage");
            if ((xmlenumitem.HasChildNodes) && (xmlenumitem.FirstChild.NodeType == XmlNodeType.Text))
                enumitem.description = xmlenumitem.FirstChild.Value.Trim();
            else
                enumitem.description = "";
            //get a nodelist of enumoption elements
            XmlNodeList enumoptionlist = xmlenumitem.GetElementsByTagName("option");
            if (enumoptionlist != null && enumoptionlist.Count > 0)
            {
                for (int i = 0; i < enumoptionlist.Count; i++)
                {
                    //get the option element
                    XmlElement xmloption = (XmlElement)enumoptionlist.Item(i);

                    //get the option object
                    //value is mostly an integer but for some it is hex, try int and 
                    // if fails try hex
                    int value = -1;

                    try
                    {
                        value = int.Parse(xmloption.GetAttribute("value"));
                    }
                    catch (FormatException numberformatexception)
                    {
                        // must be a hex 
                        value = int.Parse(xmloption.GetAttribute("value").Substring(2), NumberStyles.AllowHexSpecifier);
                    }

                    if ((xmloption.HasChildNodes) && (xmloption.FirstChild.NodeType == XmlNodeType.Text))
                    {
                        enumitem.addOption(xmloption.GetAttribute("name"), xmloption.FirstChild.Value.Trim(), value);
                    }
                    else
                    {
                        enumitem.addOption(xmloption.GetAttribute("name"), "", value);
                    }
                }
            }
            return enumitem;
        }

        private BitFlagItem getBitFlagItem(XmlElement xmlbitflagitem)
        {

            // for each <version> element get basic details 
            // and what implements it

            BitFlagItem bitflagitem = new BitFlagItem();
            bitflagitem.name = xmlbitflagitem.GetAttribute("name");
            bitflagitem.storage = xmlbitflagitem.GetAttribute("storage");
            if ((xmlbitflagitem.HasChildNodes) && (xmlbitflagitem.FirstChild.NodeType == XmlNodeType.Text))
                bitflagitem.description = xmlbitflagitem.FirstChild.Value.Trim();
            else
                bitflagitem.description = "";

            //get a nodelist of  elements
            XmlNodeList xmloptionlist = xmlbitflagitem.GetElementsByTagName("option");
            if (xmloptionlist != null && xmloptionlist.Count > 0)
            {
                foreach (XmlElement xmloption in xmloptionlist)
                {
                    //get the option object
                    //value is mostly an integer but for some it is hex, try int and 
                    // if fails try hex
                    int value = -1;

                    try
                    {
                        value = int.Parse(xmloption.GetAttribute("value"));
                    }
                    catch (FormatException numberformatexception)
                    {
                        // must be a hex 
                        value = int.Parse(xmloption.GetAttribute("value").Substring(2), NumberStyles.AllowHexSpecifier);
                    }
                    if ((xmloption.HasChildNodes) && (xmloption.FirstChild.NodeType == XmlNodeType.Text))
                    {
                        bitflagitem.addOption(xmloption.GetAttribute("name"), xmloption.FirstChild.Value.Trim(), value);
                    }
                    else
                    {
                        bitflagitem.addOption(xmloption.GetAttribute("name"), "", value);
                    }
                }
            }
            return bitflagitem;
        }

        private Compound getCompound(XmlElement xmlcompound)
        {
            // for each <version> element get basic details 
            // and what implements it
            Compound compound = new Compound();
            compound.name = xmlcompound.GetAttribute("name");
            compound.istemplate = xmlcompound.GetAttribute("istemplate");
            compound.niflibtype = xmlcompound.GetAttribute("niflibtype");
            compound.nifskopetype = xmlcompound.GetAttribute("nifskopetype");
            if ((xmlcompound.HasChildNodes) && (xmlcompound.FirstChild.NodeType == XmlNodeType.Text))
                compound.description = xmlcompound.FirstChild.Value.Trim();
            else
                compound.description = "";

            //get a nodelist of  elements
            XmlNodeList xmladdlist = xmlcompound.GetElementsByTagName("add");
            if (xmladdlist != null && xmladdlist.Count > 0)
            {
                foreach (XmlElement add in xmladdlist)
                {
                    compound.AddAdd(add.GetAttribute("name"), add.InnerText,
                        add.GetAttribute("ver1"), add.GetAttribute("ver2"),
                        add.GetAttribute("type"), add.GetAttribute("arr1"),
                        add.GetAttribute("arr2"), add.GetAttribute("arr2"),
                        add.GetAttribute("default"), add.GetAttribute("template"),
                        add.GetAttribute("userver"), add.GetAttribute("cond"),
                        add.GetAttribute("arg"));
                }
            }
            return compound;
        }

        private Niobject getNiobject(XmlElement xmlniobject)
        {
            // for each <version> element get basic details 
            // and what implements it
            Niobject niobject = new Niobject();

            niobject.name = xmlniobject.GetAttribute("name");
            niobject.isabstract = xmlniobject.GetAttribute("abstract");
            niobject.inherit = xmlniobject.GetAttribute("inherit");
            if ((xmlniobject.HasChildNodes) && (xmlniobject.FirstChild.NodeType == XmlNodeType.Text))
                niobject.description = xmlniobject.FirstChild.Value.Trim();
            else
                niobject.description = "";

            //get a nodelist of  elements
            XmlNodeList xmladdlist = xmlniobject.GetElementsByTagName("add");
            if (xmladdlist != null && xmladdlist.Count > 0)
            {
                foreach (XmlElement add in xmladdlist)
                {
                    niobject.AddAdd(
                    add.GetAttribute("name"), "",
                    add.GetAttribute("ver1"), add.GetAttribute("ver2"),
                    add.GetAttribute("type"), add.GetAttribute("arr1"),
                    add.GetAttribute("arr2"), add.GetAttribute("arr3"),
                    add.GetAttribute("default"), add.GetAttribute("template"),
                    add.GetAttribute("userver"), add.GetAttribute("vercond"),
                    add.GetAttribute("cond"), add.GetAttribute("arg"));
                }
            }
            return niobject;
        }

        private void checkForTemplatesAndProcess(Add add)
        {
            // we aren't interested in templates for NiObjects or ones with TEMPLATE
            // the TEMPLATE ones we will process recursively

            if (add.template.Length != 0 && add.originaltype.CompareTo("Ref") != 0 &&
                add.originaltype.CompareTo("Ptr") != 0 && add.originaltype.CompareTo("TEMPLATE") != 0
                && add.template.CompareTo("TEMPLATE") != 0)
            {
                Compound value;
                // check to see if we have created a new compound already
                if (NifDataHolder.getInstance().compoundlist.TryGetValue(add.type +
                    add.template.Substring(0, 1).ToUpper() + add.template.Substring(1), out value))
                {
                    string type = add.type;
                    add.type = type + add.template.Substring(0, 1).ToUpper() + add.template.Substring(1);
                    add.returntype = type + add.template.Substring(0, 1).ToUpper() + add.template.Substring(1);
                }
                else
                {
                    Compound original = NifDataHolder.getInstance().compoundtemplatelist[add.type]; //, out value);
                    Compound clone = (Compound)original.Clone();
                    clone.name = clone.name + add.template.Substring(0, 1).ToUpper() + add.template.Substring(1);
                    clone.setAddTypesForTemplate(add.template);

                    foreach (Add cloneadd in clone.addlist)
                    {
                        if (cloneadd.template.CompareTo("TEMPLATE") == 0)
                        {
                            // found a template, set the actual template and process
                            cloneadd.template = add.template;
                            checkForTemplatesAndProcess(cloneadd);
                        }
                    }

                    NifDataHolder.getInstance().compoundlist.Add(clone.name, clone);
                    String type = add.type;
                    add.type = type + add.template.Substring(0, 1).ToUpper() + add.template.Substring(1);
                    add.returntype = type + add.template.Substring(0, 1).ToUpper() + add.template.Substring(1);
                }
            }
        }
        #endregion
    }
}
