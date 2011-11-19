using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using nifcslib.NifUtilities;

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
            SimpleDebug debug = new SimpleDebug();
            debug.enableconsoleprinting = true;
            debug.writelogfiles = true;
            debug.PerformChecks();
            Console.ReadKey();
            #endregion
            
        }

        

    }
}
