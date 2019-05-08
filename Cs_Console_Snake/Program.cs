using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs_Console_Snake {
    static class Einstellungen {
        public static IniDatei ini = new IniDatei($@"{Environment.GetEnvironmentVariable("userprofile")}\Documents\Cs_Console_Snake_Einstellungen.ini");

        public static Random Random = new Random();
        public static bool InGame = false;
        public static bool ErsterStart = true;

        public static Spielfeld AktuellesSpielfeld;
        public static List<Schlange> Schlangen = new List<Schlange> { };
    }

    class Program {
        static void Main(string[] args) {
            #region ini-Datei erstellen, wenn sie nicht existiert
            if (!System.IO.File.Exists($@"{Environment.GetEnvironmentVariable("userprofile")}\Documents\Cs_Console_Snake_Einstellungen.ini")) {
                Einstellungen.ini.Write("hoehe", "50", "Spielfeld");
                Einstellungen.ini.Write("breite", "100", "Spielfeld");
                Einstellungen.ini.Write("titel", "Das Schlangenspiel", "Spielfeld");
                Einstellungen.ini.Write("anzahlHindernisseSterben", "20", "Spielfeld");
                Einstellungen.ini.Write("farbeHindernisseSterben", "Red", "Spielfeld");
                Einstellungen.ini.Write("anzahlHindernisseLaengerWerden", "50", "Spielfeld");
                Einstellungen.ini.Write("farbeHindernisseLaengerWerden", "DarkGreen", "Spielfeld");
                Einstellungen.ini.Write("anzahlHindernisseKuerzerWerden", "10", "Spielfeld");
                Einstellungen.ini.Write("farbeHindernisseKuerzerWerden", "DarkBlue", "Spielfeld");
                Einstellungen.ini.Write("farbeBegrenzung", "Red", "Spielfeld");

                Einstellungen.ini.Write("startPositionX", "50", "Schlange0");
                Einstellungen.ini.Write("startPositionY", "25", "Schlange0");
                Einstellungen.ini.Write("tasteHoch", "UpArrow", "Schlange0");
                Einstellungen.ini.Write("tasteRunter", "DownArrow", "Schlange0");
                Einstellungen.ini.Write("tasteLinks", "LeftArrow", "Schlange0");
                Einstellungen.ini.Write("tasteRechts", "RightArrow", "Schlange0");
                Einstellungen.ini.Write("schlangenFarbe", "Yellow", "Schlange0");
                Einstellungen.ini.Write("highscore", "0", "Schlange0");
            }
            #endregion

            #region Spielfeld vorbereiten
            Einstellungen.AktuellesSpielfeld = new Spielfeld();
            #endregion

            #region Schlangen einlesen und erstellen (Maximal 10 Schlangen)
            for (int i = 0; i < 10; i++) { //10 Maximale Spieler
                if (Einstellungen.ini.KeyExists("startPositionX", $"Schlange{i}")
                && Einstellungen.ini.KeyExists("startPositionY", $"Schlange{i}")
                && Einstellungen.ini.KeyExists("tasteHoch", $"Schlange{i}")
                && Einstellungen.ini.KeyExists("tasteRunter", $"Schlange{i}")
                && Einstellungen.ini.KeyExists("tasteLinks", $"Schlange{i}")
                && Einstellungen.ini.KeyExists("tasteRechts", $"Schlange{i}")
                && Einstellungen.ini.KeyExists("schlangenFarbe", $"Schlange{i}")) {

                    Einstellungen.Schlangen.Add(new Schlange(
                        Convert.ToInt32(Einstellungen.ini.Read("startPositionX", $"Schlange{i}")),
                        Convert.ToInt32(Einstellungen.ini.Read("startPositionY", $"Schlange{i}")),
                        (ConsoleKey)Enum.Parse(typeof(ConsoleKey), Einstellungen.ini.Read("tasteHoch", $"Schlange{i}")),
                        (ConsoleKey)Enum.Parse(typeof(ConsoleKey), Einstellungen.ini.Read("tasteRunter", $"Schlange{i}")),
                        (ConsoleKey)Enum.Parse(typeof(ConsoleKey), Einstellungen.ini.Read("tasteLinks", $"Schlange{i}")),
                        (ConsoleKey)Enum.Parse(typeof(ConsoleKey), Einstellungen.ini.Read("tasteRechts", $"Schlange{i}")),
                        (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Einstellungen.ini.Read("schlangenFarbe", $"Schlange{i}")),
                        i
                        ));
                }
                else {
                    break;
                }
            }

            if (Einstellungen.Schlangen.Count < 1) {
                Environment.Exit(1);
            }
            #endregion

            #region Konsole vorbereiten
            Console.CursorVisible = false;
            Console.Clear();
            #endregion

            Spielfeld.Hauptmenu();
        }

        public static void WriteAt(int x, int y, string s, ConsoleColor farbe = ConsoleColor.White, ConsoleColor hintergrundFarbe = ConsoleColor.Black) {
            try {
                Console.ForegroundColor = farbe;
                Console.BackgroundColor = hintergrundFarbe;
                Console.SetCursorPosition(x, y);
                Console.Write(s);
                Console.SetCursorPosition(0, 0);
                Console.ResetColor();
            } catch (ArgumentOutOfRangeException) {
                Environment.Exit(1);
                return;
            }
        }
    }
}

