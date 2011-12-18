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
        private static bool debug = true;

        public static void Main()
        {
            _reader = new Nifxmlreader(Properties.Settings.Default.NIF_XML);
            _reader.LoadXml();
            _reader.processXml();
            NifDataHolder.getInstance();
            
            #region debug
            if (debug)
            {
                SimpleDebug logging = new SimpleDebug();
                logging.enableconsoleprinting = true;
                logging.writelogfiles = true;
                logging.PerformChecks();
                Console.ReadKey();
            }
            #endregion
            
        }

    }
}
