using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace nifcslib
{
    class XMLParser
    {
        //private static XMLWriter _writer;
        private static Nifxmlreader _reader;


        public static void Main()
        {
            _reader = new Nifxmlreader(Properties.Settings.Default.NIF_XML);
            _reader.LoadXml();
            _reader.processXml();

            #region debug
            
            NifUtilities.SimpleDebug.emptycheck = true;
            NifUtilities.SimpleDebug.containscheck = true;
            NifUtilities.SimpleDebug.containsstring = "Unknown";
            
            NifUtilities.SimpleDebug.PerformDescriptionCheck(NifDataHolder.getInstance().basiclist, "description");
            NifUtilities.SimpleDebug.PerformDescriptionCheck(NifDataHolder.getInstance().bitflagitemlist, "description");
            NifUtilities.SimpleDebug.PerformDescriptionCheck(NifDataHolder.getInstance().compoundlist, "description");
            NifUtilities.SimpleDebug.PerformDescriptionCheck(NifDataHolder.getInstance().compoundtemplatelist, "description");
            NifUtilities.SimpleDebug.PerformDescriptionCheck(NifDataHolder.getInstance().enumitemlist, "description");
            NifUtilities.SimpleDebug.PerformDescriptionCheck(NifDataHolder.getInstance().niobjectlist, "description");
            NifUtilities.SimpleDebug.PerformDescriptionCheck(NifDataHolder.getInstance().versionlist, "description");
            NifUtilities.SimpleDebug.PerformOptionDescriptionCheck(NifDataHolder.getInstance().bitflagitemlist, "optionlist", "description");
            NifUtilities.SimpleDebug.PerformOptionDescriptionCheck(NifDataHolder.getInstance().compoundlist, "addlist", "description");
            NifUtilities.SimpleDebug.PerformOptionDescriptionCheck(NifDataHolder.getInstance().enumitemlist, "optionlist", "description");
            NifUtilities.SimpleDebug.PerformOptionDescriptionCheck(NifDataHolder.getInstance().compoundtemplatelist, "addlist", "description");
            Console.WriteLine(NifUtilities.SimpleDebug.callcount);
            #endregion

            Console.ReadKey();
        }
    }
}
