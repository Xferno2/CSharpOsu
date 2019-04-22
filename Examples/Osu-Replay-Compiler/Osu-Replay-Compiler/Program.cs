using System;
using CSharpOsu;
using System.IO;
using System.Diagnostics;
using CSharpOsu.Util.Enums;
using System.Text;
using System.Collections.Generic;

namespace Osu_Replay_Compiler
{
    class Program
    {

        public static void Main(string[] args)
        {
            string osukey = "";
            try {
                osukey = File.ReadAllText(@"Osu.txt");
            }
            catch { Console.WriteLine("API File not found!"); }

            if (osukey == "")
            {
                Console.WriteLine("API key not found!");
                Console.Write("Enter API key: ");
                File.WriteAllText(@"Osu.txt", Console.ReadLine());
                osukey = File.ReadAllText(@"Osu.txt");
            } else{ Console.WriteLine("API Key found!"); }

            OsuClient Osu = new OsuClient(osukey);

            Console.WriteLine("0. Osu!");
            Console.WriteLine("1. Taiko");
            Console.WriteLine("2. CatchTheBeat");
            Console.WriteLine("3. Osu!Mania");
            Console.Write("Mode(int 0-3): ");
            mode _m =0;
            switch (Convert.ToInt32(Console.ReadLine()))
            {
                    case 0:
                    _m = mode.osu;
                    break;
                    case 1:
                    _m = mode.Taiko;
                        break;
                   case 2:
                    _m = mode.CtB;
                    break;
                   case 3:
                    _m = mode.osuMania;
                    break;
            }

            Console.Write("Beatmap_id(/b/): ");
            long _b = Convert.ToInt64(Console.ReadLine());

            Console.Write("User(/u/): ");
            string _u = Console.ReadLine();

            List<Mods> mods=new List<Mods>();
            Console.Write("Mods(Eg: Hidden,DoubleTime): ");
            var cacheRead = Console.ReadLine();
            if (cacheRead != null)
            {
                var modsStrings =cacheRead.Split(',');
                foreach (var mod in modsStrings)
                {
                    mods.Add((Mods)Enum.Parse(typeof(Mods),mod));
                }

            }

            Console.WriteLine("[WARNING! If the file already exist it will be overwritten.]");
            Console.Write("File name(without extension): ");
            string y = Console.ReadLine();

            File.WriteAllBytes(@"" + y + ".osr", Osu.GetReplay(_m, _u,_b,Osu.modsCalculator(mods)));

            openreplay(y);
            
        }

        public static void openreplay(string y)
        {
            Console.Write("Open replay?[Y/N]: ");
            string x = Console.ReadLine();
            if (x == "Y" || x == "y")
            {
                Process.Start(@"" + y + ".osr");
            }
            else if (x == "N"|| x == "n")
            {
                Environment.Exit(0x12);
            }
            else
            {
                openreplay(y);
            }
        }

    }
}

