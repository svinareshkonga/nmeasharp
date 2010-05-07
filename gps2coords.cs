using System;
using System.IO;
using gps.parser;

namespace gps
{
    public class Gps2Coords
    {
        public gps.parser.Nmea parser = new gps.parser.Nmea();
        public gps.parser.MinimalNmeaPositionNotifier mn = new gps.parser.MinimalNmeaPositionNotifier();

        [STAThread]
        static void Main(string[] args)
        {
            try
            {
                if ((args == null) || (args.Length < 1))
                {
                    throw new Exception("Usage: gsp2coords gps-nmea-file");
                }
                Gps2Coords g = new Gps2Coords();
                //g.parser.NewMessage += new gps.parser.Nmea.NewMessageEventHandler(HandleNewMessage);
                g.mn.NewGspPosition += new gps.parser.MinimalNmeaPositionNotifier.NewGspPositionEventHandler(g.NewGspPosition);
                g.mn.Init(g.parser);
                using(BufferedStream bf = new BufferedStream(new FileStream(args[0], FileMode.Open, FileAccess.Read)))
                {
                    g.outFile = new System.IO.StreamWriter(args[0] + ".coords.txt");
                    g.parser.Source = bf;
                    g.parser.Start();
                    g.parser.WaitDone();
                    if (g.outFile != null)
                    {
                        g.outFile.Close();
                    }
                }
            }
            catch(Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }

        private StreamWriter outFile = null;
        private void NewGspPosition(gps.parser.GpsPosition pos)
        {
            outFile.WriteLine(pos.x.ToString().Replace(',', '.'));
            outFile.WriteLine(pos.y.ToString().Replace(',', '.'));
        }
    }//EOC

}//EON