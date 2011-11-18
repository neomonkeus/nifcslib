using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nifcslib.NifTypes;

namespace nifcslib.NifUtilities
{
    public class Utilities
    {
        #region Function Declaration
        public static string ReplaceFirstOccurance(string source, string find, string replace)
        {
            int place = source.IndexOf(find);
            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        public static string ReplaceLastOccurance(string source, string find, string replace)
        {
            int place = source.LastIndexOf(find);
            string result = source.Remove(place, find.Length).Insert(place, replace);
            return result;
        }

        public static string convertReturnType(string originaltype, string template, bool isArray)
        {
            originaltype = originaltype.Trim();
            string returntype = originaltype;

            if (originaltype.CompareTo("bool") == 0)
            {
                returntype = "bool";
            }
            else if (originaltype.CompareTo("Ptr") == 0 || originaltype.CompareTo("Ref") == 0)
            {
                returntype = template.Substring(0, 1).ToUpper() + template.Substring(1).Replace(" ", "");
            }
            else if (originaltype.CompareTo("int") == 0)
            {
                returntype = "int";
            }
            else if (originaltype.CompareTo("uint") == 0 || originaltype.CompareTo("long") == 0 ||
                originaltype.CompareTo("ulittle32") == 0)
            {
                returntype = "long";
            }
            else if (originaltype.CompareTo("string") == 0 || originaltype.CompareTo("StringOffset") == 0 ||
                originaltype.CompareTo("HeaderString") == 0 || originaltype.CompareTo("LineString") == 0 ||
                originaltype.CompareTo("FileVersion") == 0 || originaltype.CompareTo("SizedString") == 0 ||
                originaltype.CompareTo("StringIndex") == 0)
            {
                returntype = "string";
            }
            else if (originaltype.CompareTo("short") == 0 || originaltype.CompareTo("Flags") == 0 ||
                originaltype.CompareTo("BlockTypeIndex") == 0)
            {
                returntype = "short";
            }
            else if (originaltype.CompareTo("ushort") == 0)
            {
                returntype = "int";
            }
            else if (originaltype.CompareTo("float") == 0)
            {
                returntype = "float";
            }
            else if (originaltype.CompareTo("char") == 0)
            {
                returntype = "char";
            }
            else if (originaltype.CompareTo("byte") == 0)
            {
                returntype = "byte";
            }
            else if (originaltype.CompareTo("TEMPLATE") == 0)
            {
                returntype = "TEMPLATE";
            }
            else
            {
                // we could have a compound type or enum type
                // check compound first, if it is then just use the originaltype
                if (!(findCompound(originaltype) || findBitFlag(originaltype)))
                {
                    // must be an enum return type, we need to find the enum then find its type
                    returntype = findEnumType(originaltype);
                }
                else if (!(findCompound(originaltype) || findBitFlag(originaltype)))
                {
                    // must be an bitFlag return type, we need to find the bitflag then find its type
                    returntype = findBitFlagType(originaltype);
                }
                else
                {
                    returntype = returntype.Substring(0, 1).ToUpper() + returntype.Substring(1).Replace(" ", "");
                }

            }

            if (isArray)
            {
                returntype = "ArrayList";
            }
            return returntype;
        }

        public static string convertType(string originaltype, string template)
        {
            originaltype = originaltype.Trim();
            string type = originaltype;

            if (originaltype.CompareTo("bool") == 0)
            {
                type = "boolean";
            }
            else if (originaltype.CompareTo("Ptr") == 0 || originaltype.CompareTo("Ref") == 0)
            {
                type = template.Substring(0, 1).ToUpper() + template.Substring(1).Replace(" ", "");
            }
            else if (originaltype.CompareTo("int") == 0 || originaltype.CompareTo("ushort") == 0)
            {
                type = "int";
            }
            else if (originaltype.CompareTo("uint") == 0 || originaltype.CompareTo("ulittle32") == 0)
            {
                type = "long";
            }
            else if (originaltype.CompareTo("string") == 0 || originaltype.CompareTo("StringOffset") == 0 ||
                originaltype.CompareTo("HeaderString") == 0 || originaltype.CompareTo("LineString") == 0 ||
                originaltype.CompareTo("FileVersion") == 0 || originaltype.CompareTo("SizedString") == 0 ||
                originaltype.CompareTo("StringIndex") == 0)
            {
                type = "String";
            }
            else if (originaltype.CompareTo("short") == 0 || originaltype.CompareTo("Flags") == 0 ||
                originaltype.CompareTo("BlockTypeIndex") == 0)
            {
                type = "short";
            }
            else if (originaltype.CompareTo("float") == 0)
            {
                type = "float";
            }
            else if (originaltype.CompareTo("char") == 0)
            {
                type = "char";
            }
            else if (originaltype.CompareTo("byte") == 0)
            {
                type = "byte";
            }
            else if (originaltype.CompareTo("TEMPLATE") == 0)
            {
                type = "TEMPLATE";
            }
            else if (findBitFlag(originaltype))
            {
                type = findBitFlagType(originaltype);
            }
            else
            {
                // we could have a compound type or enum type, doesn't matter for type      
                type = type.Substring(0, 1).ToUpper() + type.Substring(1).Replace(" ", "");
            }

            return type;
        }

        public static string convertToArrayType(string originaltype, string template)
        {

            String arrayType = originaltype;

            if (originaltype.CompareTo("bool") == 0)
            {
                arrayType = "Boolean";
            }
            else if (originaltype.CompareTo("Ptr") == 0 || originaltype.CompareTo("Ref") == 0)
            {
                arrayType = template.Substring(0, 1).ToUpper() + template.Substring(1).Replace(" ", "");
            }
            else if (originaltype.CompareTo("int") == 0 || originaltype.CompareTo("ushort") == 0)
            {
                arrayType = "Integer";
            }
            else if (originaltype.CompareTo("long") == 0 || originaltype.CompareTo("uint") == 0 ||
                originaltype.CompareTo("ulittle32") == 0)
            {
                arrayType = "Long";
            }
            else if (originaltype.CompareTo("string") == 0 || originaltype.CompareTo("StringOffset") == 0 ||
                originaltype.CompareTo("HeaderString") == 0 || originaltype.CompareTo("LineString") == 0 ||
                originaltype.CompareTo("FileVersion") == 0 || originaltype.CompareTo("SizedString") == 0 ||
                originaltype.CompareTo("StringIndex") == 0)
            {
                arrayType = "String";
            }
            else if (originaltype.CompareTo("short") == 0 || originaltype.CompareTo("Flags") == 0 ||
                originaltype.CompareTo("BlockTypeIndex") == 0)
            {
                arrayType = "Short";
            }
            else if (originaltype.CompareTo("float") == 0)
            {
                arrayType = "Float";
            }
            else if (originaltype.CompareTo("char") == 0)
            {
                arrayType = "Character";
            }
            else if (originaltype.CompareTo("byte") == 0)
            {
                arrayType = "Byte";
            }
            else if (originaltype.CompareTo("TEMPLATE") == 0)
            {
                arrayType = "TEMPLATE";
            }
            else
            {
                // we could have a compound type or enum type
                // check compound first, if it is then just use the originaltype
                if (!(findCompound(originaltype) || findBitFlag(originaltype)))
                {
                    // must be an enum return type, we need to find the enum then find its type
                    arrayType = convertToArrayType(findEnumType(originaltype), "");
                }
                else if (!(findCompound(originaltype) || findBitFlag(originaltype)))
                {
                    // must be an bitFlag return type, we need to find the bitflag then find its type
                    arrayType = convertToArrayType(findBitFlagType(originaltype), "");
                }
                else
                {
                    arrayType = arrayType.Substring(0, 1).ToUpper() + arrayType.Substring(1).Replace(" ", "");
                }

            }

            return arrayType;
        }

        public static bool findCompound(String compoundname)
        {
            bool found = false;
            compoundname = compoundname.Substring(0, 1).ToUpper() + compoundname.Substring(1).Replace(" ", "");

            foreach (KeyValuePair<string, Compound> keyvalpair in NifDataHolder.getInstance().compoundlist)
            {
                if (keyvalpair.Key.CompareTo(compoundname) == 0)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        public static bool findBitFlag(String bitflagname)
        {
            bool found = false;
            bitflagname = bitflagname.Substring(0, 1).ToUpper() + bitflagname.Substring(1).Replace(" ", "");

            foreach (KeyValuePair<string, BitFlagItem> keyvalpair in NifDataHolder.getInstance().bitflagitemlist)
            {
                if (keyvalpair.Key.CompareTo(bitflagname) == 0)
                {
                    found = true;
                    break;
                }
            }
            return found;
        }

        public static String findEnumType(String enumname)
        {
            String enumitemtype = "";
            enumname = enumname.Substring(0, 1).ToUpper() + enumname.Substring(1).Replace(" ", "");

            foreach (KeyValuePair<string, EnumItem> keyvalpair in NifDataHolder.getInstance().enumitemlist)
            {
                if (keyvalpair.Key.CompareTo(enumname) == 0)
                {
                    enumitemtype = convertReturnType(keyvalpair.Value.storage, "", false);
                    break;
                }
            }

            return enumitemtype;
        }

        public static String findEnumTypeOriginal(String enumname)
        {
            String enumitemtype = "";
            enumname = enumname.Substring(0, 1).ToUpper() + enumname.Substring(1).Replace(" ", "");

            foreach (KeyValuePair<string, EnumItem> keyvalpair in NifDataHolder.getInstance().enumitemlist)
            {
                if (keyvalpair.Key.CompareTo(enumname) == 0)
                {
                    enumitemtype = keyvalpair.Value.storage;
                    break;
                }
            }

            return enumitemtype;
        }

        public static String findBitFlagType(String bitflagname)
        {
            string bitflagitemtype = "";
            bitflagname = bitflagname.Substring(0, 1).ToUpper() + bitflagname.Substring(1).Replace(" ", "");

            foreach (KeyValuePair<string, BitFlagItem> keyvalpair in NifDataHolder.getInstance().bitflagitemlist)
            {
                if (keyvalpair.Key.CompareTo(bitflagname) == 0)
                {
                    bitflagitemtype = convertReturnType(keyvalpair.Value.storage, "", false);
                    break;
                }
            }

            return bitflagitemtype;
        }

        public static String findBitFlagTypeOriginal(String bitflagname)
        {
            string bitflagitemtype = "";
            bitflagname = bitflagname.Substring(0, 1).ToUpper() + bitflagname.Substring(1).Replace(" ", "");

            foreach (KeyValuePair<string, BitFlagItem> keyvalpair in NifDataHolder.getInstance().bitflagitemlist)
            {
                if (keyvalpair.Key.CompareTo(bitflagname) == 0)
                {
                    bitflagitemtype = keyvalpair.Value.storage;
                    break;
                }
            }

            return bitflagitemtype;
        }
        #endregion

        #region Byte Reading Functions
        private static string getVersionCode(int version1, int version2)
        {
            if (version1 == 0 && version2 == 0)
            {
                return "";
            }
            else if (version1 == 0 && version2 != 0)
            {
                return "\t\tif (int.Parse(header.version) <= " + version2;
            }
            else if (version1 != 0 && version2 == 0)
            {

                return "\t\tif (int.Parse(header.version) >= " + version1;
            }
            else
            {
                return "\t\tif (int.Parse(header.version) >= " + version1 + "&& int.Parse(header.version) <= " + version2;
            }
        }

        public static String getReadCode(String originalType, 
            int version1, int version2, String name, String condition, 
            String arrayLoop1, String arrayLoop2, String arrayLoop3, 
            String type, bool isArray2Array, bool isArray3Array, String argument)
        {
            String s = "";
            // this will be a multiphase approach
            // 1. Work out what to do for a particular type
            // 2. Work out how man times times to read this particular  variable using arr1 and arr2
            // 3. Work out that if statements to use using verison1, version2 and condition
            s = getTypeCode(type, originalType, name, argument);
            s = getForLoopCode(s, arrayLoop1, arrayLoop2, arrayLoop3, isArray2Array, isArray3Array);
            s = getIfCode(s, version1, version2, condition);
            return s;
        }


        private static String getForLoopCode(String currentstring, String arrayloop1, String arrayloop2, String arrayloop3, bool isarray2array, bool isarray3array)
        {
            StringBuilder s = new StringBuilder();

            if (arrayloop1.Length != 0)
            {
                s.Append("\t\tfor (int i = 0;i < (" + arrayloop1 + ") ;i++)\n");
                s.Append("\t\t{\n");
            }
            if (arrayloop2.Length != 0)
            {
                s.Append("\t\tfor (int j = 0;j < (" + (isarray2array ? "((Number)" + arrayloop2 + ".get(i)).intValue()" : arrayloop2) + ");j++)\n");
                s.Append("\t\t{\n");
            }
            if (arrayloop3.Length != 0)
            {
                s.Append("\t\tfor (int k = 0;k < (" + (isarray2array ? "((Number)" + arrayloop2 + ".get(j)).intValue()" : arrayloop2) + ");k++)\n");
                s.Append("\t\t{\n");
            }
            s.Append(currentstring);

            if (arrayloop1.Length != 0)
            {
                s.Append("\t\t}\n");
            }
            if (arrayloop2.Length != 0)
            {
                s.Append("\t\t}\n");
            }
            if (arrayloop3.Length != 0)
            {
                s.Append("\t\t}\n");
            }

            return s.ToString();
        }

        private static String getIfCode(String currentString, int version1, int version2, String condition)
        {
            StringBuilder s = new StringBuilder();

            if (getVersionCode(version1, version2).CompareTo("") != 0 || condition.Trim().Length != 0)
            {
                if (getVersionCode(version1, version2).CompareTo("") != 0 && condition.Trim().Length == 0)
                {
                    s.Append(getVersionCode(version1, version2) + ")\n");
                }
                else if (getVersionCode(version1, version2).CompareTo("") != 0 && condition.Trim().Length != 0)
                {
                    s.Append(getVersionCode(version1, version2) + " && " + condition + ")\n");
                }
                else
                {
                    s.Append("if (" + condition + ")\n");
                }

                s.Append("\t\t{\n");
                s.Append("\t\t" + currentString);
                s.Append("\t\t}\n");
            }
            else
            {
                s.Append(currentString);
            }

            return s.ToString();
        }

        private static String getTypeCode(String type, String originaltype, String name, String arguement)
        {
            StringBuilder s = new StringBuilder();
            if (originaltype.CompareTo("bool") == 0)
            {
                s.Append("\tif (Integer.parseInt(header.version) <= 4010001)\n");
                s.Append("\t{\n");
                if (type.CompareTo("ArrayList") == 0)
                {
                    s.Append("\t\t\t" + name + ".add(new Boolean((bbuf.getInt()==0?false:true)));");
                }
                else
                {
                    s.Append(name + "\t\t\t" + " = (bbuf.getInt()==0?false:true);\n");
                }
                s.Append("\t} else\n");
                s.Append("\t{\n");
                if (type.CompareTo("ArrayList") == 0)
                {
                    s.Append("\t\t\t" + name + ".add(new Boolean((bbuf.get()==0?false:true)));");
                }
                else
                {
                    s.Append(name + "\t\t\t" + " = (bbuf.get()==0?false:true);\n");
                }
                s.Append("\t}\n");
            }
            else if (originaltype.CompareTo("Ptr") == 0 || originaltype.CompareTo("Ref") == 0)
            {
                if (type.CompareTo("ArrayList") == 0)
                {
                    s.Append("\t\t" + name + "IntegerRef.add(new Integer(bbuf.getInt()));");
                }
                else
                {
                    s.Append("\t\t\t" + name + "IntegerRef = bbuf.getInt();\n");
                }
            }
            else if (originaltype.CompareTo("int") == 0)
            {
                if (type.CompareTo("ArrayList") == 0)
                {
                    s.Append("\t\t\t" + name + ".add(new Integer(bbuf.getInt ()));");
                }
                else
                {
                    s.Append(name + "\t\t\t" + " = bbuf.getInt();\n");
                }
            }
            else if (originaltype.CompareTo("uint") == 0 || originaltype.CompareTo("long") == 0 || originaltype.CompareTo("ulittle32") == 0)
            {
                if (type.CompareTo("ArrayList") == 0)
                {
                    s.Append("\t\t\t" + name + ".add(new Long((long)(bbuf.getInt() & 0xffffffffL)));");
                }
                else
                {
                    s.Append(name + "\t\t\t" + " = (long)(bbuf.getInt() & 0xffffffffL);\n");
                }
            }
            else if (originaltype.CompareTo("string") == 0)
            {
                // version read code changes so we will write the code out
                // doesn't look great here but writes readable code to use
                s.Append("\tif (Integer.parseInt(header.getVersion ()) <= 20000005)\n");
                s.Append("\t\t{\n");
                s.Append(getTypeCode(type, "SizedString", name, arguement));
                s.Append("\t} else\n");
                s.Append("\t{\n");
                s.Append(getTypeCode(type, "StringIndex", name, arguement));
                s.Append("\t}\n");
            }
            else if (originaltype.CompareTo("StringOffset") == 0)
            {
                s.Append("\t\t{\n");
                s.Append("//TODO string offset;");
                s.Append("\t\t}\n");
            }
            else if (originaltype.CompareTo("SizedString") == 0)
            {
                s.Append("\t\t\tint stringSize" + name + " = bbuf.getInt ();\n");
                s.Append("\t\t\tbyte[] byteBuffer" + name + " = new byte[" + "stringSize" + name + "];\n");
                s.Append("\t\t\tbbuf.get (byteBuffer" + name + ");\n");

                if (type.CompareTo("ArrayList") == 0)
                {
                    s.Append("\t\t\t" + name + ".add(new String(byteBuffer" + name + "));");
                }
                else
                {
                    s.Append("\t\t\t" + name + " = new String(byteBuffer" + name + ");");
                }


            }
            else if (originaltype.CompareTo("StringIndex") == 0)
            {
                if (type.CompareTo("ArrayList") == 0)
                {
                    s.Append("\t\t\t" + name + ".add(header.getString (bbuf.getInt ()));");
                }
                else
                {
                    s.Append("\t\t\t" + name + " = header.getString (bbuf.getInt ());");
                }

            }
            else if (originaltype.CompareTo("HeaderString") == 0 || originaltype.CompareTo("LineString") == 0)
            {
                s.Append("\t\t\tString " + name + "Ls = \"\";\n");
                s.Append("\t\t\tchar " + name + "c = ' ';\n");
                s.Append("\t\t\tbyte[] " + name + "b = new byte[1];\n");
                s.Append("\t\t\twhile (" + name + "b[0] != 0x0A)\n");
                s.Append("\t\t\t{\n");
                s.Append("\t\t\t\tbbuf.get(" + name + "b);\n");
                s.Append("\t\t\t\t" + name + "c = new String (" + name + "b).charAt(0);\n");
                s.Append("\t\t\t\t" + name + "Ls = " + name + "Ls + new String (" + name + "b);\n");
                s.Append("\t\t\t}\n");

                if (type.CompareTo("ArrayList") == 0)
                {
                    s.Append("\t\t\t" + name + ".add(" + name + "Ls.substring(0, " + name + "Ls.length()-1));");
                }
                else
                {

                    s.Append("\t\t\t" + name + "=" + name + "Ls.substring(0, " + name + "Ls.length()-1);\n");
                }


            }
            else if (originaltype.CompareTo("FileVersion") == 0)
            {
                s.Append("\t\t\t" + name + " = Integer.toHexString(bbuf.getInt());\n");
                s.Append("\t\t\t" + "\t\t\t" + "if (" + name + ".length() == 7)\n");
                s.Append("\t\t\t" + "{\n");
                s.Append("\t\t\t" + name + " = \"0\" + " + name + ";\n");
                s.Append("\t\t\t" + "}\n");
                s.Append("\t\t\t" + name + " = (Integer.valueOf(" + 
                    name + ".substring(0,2),16).toString().length()==1?\"0\" + Integer.valueOf(" + 
                    name + ".substring(0,2),16).toString():\"\" + Integer.valueOf(" + 
                    name + ".substring(0,2),16).toString()) + (Integer.valueOf(" + 
                    name + ".substring(2,4),16).toString().length()==1?\"0\" + Integer.valueOf(" + 
                    name + ".substring(2,4),16).toString():\"\"+Integer.valueOf(" + 
                    name + ".substring(2,4),16).toString())+(Integer.valueOf(" + 
                    name + ".substring(4,6),16).toString().length()==1?\"0\" + Integer.valueOf(" + 
                    name + ".substring(4,6),16).toString():\"\" + Integer.valueOf(" + 
                    name + ".substring(4,6),16).toString())+(Integer.valueOf(" + 
                    name + ".substring(6,8),16).toString().length()==1?\"0\" + Integer.valueOf(" + 
                    name + ".substring(6,8),16).toString():\"\"+Integer.valueOf(" + 
                    name + ".substring(6,8),16).toString());\n");
            }
            else if (originaltype.CompareTo("SizedString") == 0)
            {
                s.Append("//TODO sized string;");
            }
            else if (originaltype.CompareTo("short") == 0 || originaltype.CompareTo("Flags") == 0 || originaltype.CompareTo("BlockTypeIndex") == 0)
            {
                if (type.CompareTo("ArrayList") == 0)
                {
                    s.Append("\t\t\t" + name + ".add(new Short(bbuf.getShort()));");
                }
                else
                {
                    s.Append("\t\t\t" + name + " = bbuf.getShort();\n");
                }

            }
            else if (originaltype.CompareTo("ushort") == 0)
            {
                if (type.CompareTo("ArrayList") == 0)
                {
                    s.Append("\t\t\t" + name + ".add(new Integer((int)(bbuf.getShort() & 0xffff)));");
                }
                else
                {
                    s.Append("\t\t\t" + name + " = (int)(bbuf.getShort() & 0xffff);\n");
                }

            }
            else if (originaltype.CompareTo("float") == 0)
            {
                if (type.CompareTo("ArrayList") == 0)
                {
                    s.Append("\t\t\t" + name + ".add(new Float (bbuf.getFloat()));");
                }
                else
                {
                    s.Append("\t\t\t" + name + " = bbuf.getFloat();\n");
                }

            }
            else if (originaltype.CompareTo("char") == 0)
            {
                if (type.CompareTo("ArrayList") == 0)
                {
                    s.Append("\t\t\t" + name + ".add(new Character (bbuf.getChar()));");
                }
                else
                {
                    s.Append("\t\t\t" + name + " = bbuf.getChar();\n");
                }

            }
            else if (originaltype.CompareTo("byte") == 0)
            {
                if (type.CompareTo("ArrayList") == 0)
                {
                    s.Append("\t\t\t" + name + ".add(new Byte (bbuf.get()));");
                }
                else
                {
                    s.Append("\t\t\t" + name + " = bbuf.get();\n");
                }
            }
            else
            {
                // we could have a compound type or enum type
                // check compound first, if it is then just use the originaltype

                if (!findCompound(originaltype) && !findBitFlag(originaltype))
                {
                    // must be an enum return type, we need to find the enum type 
                    // then get the code for that type
                    s.Append(getTypeCode(type, findEnumTypeOriginal(originaltype), name, arguement));
                }
                else if (!findCompound(originaltype) && findBitFlag(originaltype))
                {
                    // must be an enum return type, we need to find the enum type 
                    // then get the code for that type
                    s.Append(getTypeCode(type, findBitFlagTypeOriginal(originaltype), name, arguement));
                }
                else
                {
                    if (type.CompareTo("ArrayList") == 0)
                    {

                        s.Append("\t\t" + convertReturnType(originaltype, type, false) + " " + 
                            name + "toLoad = new " + convertToArrayType(originaltype, type) + "(" + 
                            (arguement.Length != 0 ? "(int)" + arguement : "") + ");\n");
                        s.Append("\t\t" + name + "toLoad.load (bbuf,header);");
                        s.Append("\t\t\t" + name + ".add(" + name + "toLoad);");
                    }
                    else
                    {
                        s.Append("\t\t" + name + " = new " + originaltype.Substring(0, 1).ToUpper() + 
                            originaltype.Substring(1).Replace(" ", "") + "(" + 
                            (arguement.Length != 0 ? "(int)" + arguement : "") + ");\n");
                        s.Append("\t\t" + name + ".load (bbuf,header);");
                    }
                }

            }
            return s.ToString();
        }

        #endregion
    }
}