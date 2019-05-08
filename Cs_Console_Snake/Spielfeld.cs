using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs_Console_Snake {
    class Spielfeld {

        private static int hoehe;
        public static int Hoehe {
            get { return hoehe; }
            set {
                hoehe = value;
                Console.WindowHeight = hoehe;
                Console.BufferHeight = hoehe + 1;
            }
        }

        private static int breite;
        public static int Breite {
            get { return breite; }
            set {
                breite = (value % 2 == 0) ? value + 1 : value;
                Console.WindowWidth = breite;
                Console.BufferWidth = breite + 1;
            }
        }

        private static string titel;
        public static string Titel {
            get { return titel; }
            set {
                titel = value;
                Console.Title = titel;
            }
        }


        public Spielfeld() {
            Breite = Convert.ToInt32(Einstellungen.ini.Read("breite", "Spielfeld"));
            Hoehe = Convert.ToInt32(Einstellungen.ini.Read("hoehe", "Spielfeld"));
            Titel = Einstellungen.ini.Read("titel", "Spielfeld");
            Console.Clear();
        }

        public static void SpielStarten() {

            Rendern();

            Einstellungen.InGame = true;
            while (Einstellungen.InGame) {
                if (Console.KeyAvailable) {
                    ConsoleKey aktuelleTaste = Console.ReadKey().Key;

                    if (aktuelleTaste == ConsoleKey.Escape) {
                        Einstellungen.InGame = false;
                        break;
                    }

                    for (int i = 0; i < Einstellungen.Schlangen.Count; i++) {
                        if (aktuelleTaste == Einstellungen.Schlangen[i].TasteHoch) {
                            if (Einstellungen.Schlangen[i].LetzteTaste == "runter") { break; }
                            Einstellungen.Schlangen[i].LetzteTaste = "hoch";
                            break;
                        }
                        else if (aktuelleTaste == Einstellungen.Schlangen[i].TasteRunter) {
                            if (Einstellungen.Schlangen[i].LetzteTaste == "hoch") { break; }
                            Einstellungen.Schlangen[i].LetzteTaste = "runter";
                            break;
                        }
                        else if (aktuelleTaste == Einstellungen.Schlangen[i].TasteLinks) {
                            if (Einstellungen.Schlangen[i].LetzteTaste == "rechts") { break; }
                            Einstellungen.Schlangen[i].LetzteTaste = "links";
                            break;
                        }
                        else if (aktuelleTaste == Einstellungen.Schlangen[i].TasteRechts) {
                            if (Einstellungen.Schlangen[i].LetzteTaste == "links") { break; }
                            Einstellungen.Schlangen[i].LetzteTaste = "rechts";
                            break;
                        }
                    }
                }

                for (int i = 0; i < Einstellungen.Schlangen.Count; i++) {
                    if (Einstellungen.Schlangen[i].LetzteTaste != "") {
                        Einstellungen.Schlangen[i].Schritt(Einstellungen.Schlangen[i].LetzteTaste);
                    }
                }
                Program.WriteAt(0, 0, "╔", (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Einstellungen.ini.Read("farbeBegrenzung", "Spielfeld")));
                System.Threading.Thread.Sleep(100);
            }
        }

        public static void Rendern() {
            Console.Clear();

            ConsoleColor begrenzungFarbe = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Einstellungen.ini.Read("farbeBegrenzung", "Spielfeld"));

            #region Begrenzungen generieren
            for (int i = 1; i < Hoehe - 1; i++) {
                System.Threading.Thread.Sleep(1);
                Program.WriteAt(0, i, "║", begrenzungFarbe);
                Program.WriteAt(Breite - 1, Hoehe - i - 1, "║", begrenzungFarbe);
            }
            Program.WriteAt(0, Hoehe - 1, "╚", begrenzungFarbe);
            Program.WriteAt(Breite - 1, 0, "╗", begrenzungFarbe);

            for (int i = 1; i < Breite - 1; i++) {
                System.Threading.Thread.Sleep(1);
                Program.WriteAt(Breite - i - 1, 0, "═", begrenzungFarbe);
                Program.WriteAt(i, Hoehe - 1, "═", begrenzungFarbe);
            }
            Program.WriteAt(0, 0, "╔", begrenzungFarbe);
            Program.WriteAt(Breite - 1, Hoehe - 1, "╝", begrenzungFarbe);
            #endregion

            AlleObjekte.ObjektListe.RemoveRange(0, AlleObjekte.ObjektListe.Count);

            AlleObjekte.MengeHinzufuegen(Convert.ToInt32(Einstellungen.ini.Read("anzahlHindernisseSterben", "Spielfeld")), SpielAktion.Sterben);
            AlleObjekte.MengeHinzufuegen(Convert.ToInt32(Einstellungen.ini.Read("anzahlHindernisseLaengerWerden", "Spielfeld")), SpielAktion.LaengerWerden);
            AlleObjekte.MengeHinzufuegen(Convert.ToInt32(Einstellungen.ini.Read("anzahlHindernisseKuerzerWerden", "Spielfeld")), SpielAktion.KuerzerWerden);
            AlleObjekte.ObjekteRendern();

            for (int i = 0; i < Einstellungen.Schlangen.Count; i++) {
                Einstellungen.Schlangen[i].Zuruecksetzen();
            }
        }

        public static void Hauptmenu() {
            while (true) {
                Einstellungen.InGame = false;
                Console.Clear();
                ConsoleKeyInfo gedrueckteTaste;

                //Begrüßungstext oder Game Over Text
                if (Einstellungen.ErsterStart) {
                    #region Begrüßungs Text
                    Program.WriteAt((Breite / 2) - 21, 3, "╔══════════════════════════════════════════╗", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    Program.WriteAt((Breite / 2) - 21, 4, "║███████╗███╗   ██╗ █████╗ ██╗  ██╗███████╗║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    Program.WriteAt((Breite / 2) - 21, 5, "║██╔════╝████╗  ██║██╔══██╗██║ ██╔╝██╔════╝║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    Program.WriteAt((Breite / 2) - 21, 6, "║███████╗██╔██╗ ██║███████║█████╔╝ █████╗  ║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    Program.WriteAt((Breite / 2) - 21, 7, "║╚════██║██║╚██╗██║██╔══██║██╔═██╗ ██╔══╝  ║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    Program.WriteAt((Breite / 2) - 21, 8, "║███████║██║ ╚████║██║  ██║██║  ██╗███████╗║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    Program.WriteAt((Breite / 2) - 21, 9, "║╚══════╝╚═╝  ╚═══╝╚═╝  ╚═╝╚═╝  ╚═╝╚══════╝║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    Program.WriteAt((Breite / 2) - 21, 10, "╚══════════════════════════════════════════╝", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    #endregion
                }
                else {
                    #region Game Over Text
                    Program.WriteAt((Breite / 2) - 40, 3, "╔═══════════════════════════════════════════════════════════════════════════════╗", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    Program.WriteAt((Breite / 2) - 40, 4, "║  ██████╗  █████╗ ███╗   ███╗███████╗     ██████╗ ██╗   ██╗███████╗██████╗ ██╗ ║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    Program.WriteAt((Breite / 2) - 40, 5, "║ ██╔════╝ ██╔══██╗████╗ ████║██╔════╝    ██╔═══██╗██║   ██║██╔════╝██╔══██╗██║ ║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    Program.WriteAt((Breite / 2) - 40, 6, "║ ██║  ███╗███████║██╔████╔██║█████╗      ██║   ██║██║   ██║█████╗  ██████╔╝██║ ║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    Program.WriteAt((Breite / 2) - 40, 7, "║ ██║   ██║██╔══██║██║╚██╔╝██║██╔══╝      ██║   ██║╚██╗ ██╔╝██╔══╝  ██╔══██╗╚═╝ ║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    Program.WriteAt((Breite / 2) - 40, 8, "║ ╚██████╔╝██║  ██║██║ ╚═╝ ██║███████╗    ╚██████╔╝ ╚████╔╝ ███████╗██║  ██║██╗ ║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    Program.WriteAt((Breite / 2) - 40, 9, "║  ╚═════╝ ╚═╝  ╚═╝╚═╝     ╚═╝╚══════╝     ╚═════╝   ╚═══╝  ╚══════╝╚═╝  ╚═╝╚═╝ ║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    Program.WriteAt((Breite / 2) - 40, 10, "╚═══════════════════════════════════════════════════════════════════════════════╝", ConsoleColor.Red, ConsoleColor.DarkBlue);
                    #endregion
                }

                Einstellungen.ErsterStart = false;

                #region Menü Auswahlmöglichkeiten
                Program.WriteAt((Breite / 2) - 16, 13, "╔═══╦═══════════════════════════╗");
                Program.WriteAt((Breite / 2) - 16, 14, "║ 1 ║  Neues Spiel              ║");
                Program.WriteAt((Breite / 2) - 16, 15, "╚═══╩═══════════════════════════╝");
                Program.WriteAt((Breite / 2) - 16, 16, "╔═══╦═══════════════════════════╗");
                Program.WriteAt((Breite / 2) - 16, 17, "║ 2 ║  Einstellungen            ║");
                Program.WriteAt((Breite / 2) - 16, 18, "╚═══╩═══════════════════════════╝");
                Program.WriteAt((Breite / 2) - 16, 19, "╔═══╦═══════════════════════════╗");
                Program.WriteAt((Breite / 2) - 16, 20, "║ X ║  Beenden                  ║");
                Program.WriteAt((Breite / 2) - 16, 21, "╚═══╩═══════════════════════════╝");
                #endregion

                #region Highscores
                Program.WriteAt((Breite / 2) - 12, 24, "Highscores:");
                for (int i = 0; i < Einstellungen.Schlangen.Count; i++) {
                    if (Einstellungen.Schlangen[i].LaengsteLaenge > Convert.ToInt32(Einstellungen.ini.Read("highscore", $"Schlange{i}"))) {
                        Einstellungen.ini.Write("highscore", $"{Einstellungen.Schlangen[i].LaengsteLaenge.ToString()}", $"Schlange{i}");
                    }
                    Program.WriteAt((Breite / 2) - 12, 25 + i, $"Schlange {i + 1} Highscore: {Einstellungen.ini.Read("highscore", $"Schlange{i}")}", Einstellungen.Schlangen[i].SchlangenFarbe);
                }
                #endregion

                #region Created by Text
                Program.WriteAt(Breite - 20, Hoehe - 1, "Created by KarlKeu00", ConsoleColor.DarkGray);
                #endregion

                gedrueckteTaste = Console.ReadKey();

                switch (gedrueckteTaste.Key) {
                    #region X
                    case ConsoleKey.X:
                    case ConsoleKey.Escape:
                        return;
                    #endregion
                    #region 1
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        Spielfeld.SpielStarten();
                        break;
                    #endregion
                    #region 2
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        EinstellungenMenu();
                        break;
                    #endregion
                    #region Alles andere
                    default:
                        Program.WriteAt(0, 0, " ");
                        Console.Beep();
                        break;
                        #endregion
                }
            }
        }

        public static void EinstellungenMenu() {
            while (true) {
                Einstellungen.InGame = false;
                Console.Clear();
                ConsoleKeyInfo gedrueckteTaste;

                #region Einstellungen Text
                Program.WriteAt((Breite / 2) - 36, 3, "╔════════════════════════════════════════════════════════════════════════╗", ConsoleColor.Red, ConsoleColor.DarkBlue);
                Program.WriteAt((Breite / 2) - 36, 4, "║ _____  _              _         _  _                                   ║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                Program.WriteAt((Breite / 2) - 36, 5, "║| ____|(_) _ __   ___ | |_  ___ | || | _   _  _ __    __ _   ___  _ __  ║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                Program.WriteAt((Breite / 2) - 36, 6, "║|  _|  | || '_ \\ / __|| __|/ _ \\| || || | | || '_ \\  / _` | / _ \\| '_ \\ ║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                Program.WriteAt((Breite / 2) - 36, 7, "║| |___ | || | | |\\__ \\| |_|  __/| || || |_| || | | || (_| ||  __/| | | |║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                Program.WriteAt((Breite / 2) - 36, 8, "║|_____||_||_| |_||___/ \\__|\\___||_||_| \\__,_||_| |_| \\__, | \\___||_| |_|║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                Program.WriteAt((Breite / 2) - 36, 9, "║                                                     |___/              ║", ConsoleColor.Red, ConsoleColor.DarkBlue);
                Program.WriteAt((Breite / 2) - 36, 10, "╚════════════════════════════════════════════════════════════════════════╝", ConsoleColor.Red, ConsoleColor.DarkBlue);
                #endregion

                #region Menü Auswahlmöglichkeiten
                Program.WriteAt((Breite / 2) - 33, 13, "╔═══╦═══════════════════════════╗  ╔═══╦═══════════════════════════╗");
                Program.WriteAt((Breite / 2) - 33, 14, "║ 1 ║  Farbe Hindernisse        ║  ║ 2 ║  Anzahl Hindernisse       ║");
                Program.WriteAt((Breite / 2) - 33, 15, "╚═══╩═══════════════════════════╝  ╚═══╩═══════════════════════════╝");
                Program.WriteAt((Breite / 2) - 33, 16, "╔═══╦═══════════════════════════╗  ╔═══╦═══════════════════════════╗");
                Program.WriteAt((Breite / 2) - 33, 17, "║ 3 ║  Farbe Essen              ║  ║ 4 ║  Anzahl Essen             ║");
                Program.WriteAt((Breite / 2) - 33, 18, "╚═══╩═══════════════════════════╝  ╚═══╩═══════════════════════════╝");
                Program.WriteAt((Breite / 2) - 33, 19, "╔═══╦═══════════════════════════╗  ╔═══╦═══════════════════════════╗");
                Program.WriteAt((Breite / 2) - 33, 20, "║ 5 ║  Farbe Schönheitschirurg  ║  ║ 6 ║  Anzahl Schönheitschirurg ║");
                Program.WriteAt((Breite / 2) - 33, 21, "╚═══╩═══════════════════════════╝  ╚═══╩═══════════════════════════╝");
                Program.WriteAt((Breite / 2) - 33, 22, "╔═══╦═══════════════════════════╗");
                Program.WriteAt((Breite / 2) - 33, 23, "║ 7 ║  Farbe Begrenzung         ║");
                Program.WriteAt((Breite / 2) - 33, 24, "╚═══╩═══════════════════════════╝");
                Program.WriteAt((Breite / 2) - 33, 25, "╔═══╦═══════════════════════════╗  ╔═══╦═══════════════════════════╗");
                Program.WriteAt((Breite / 2) - 33, 26, "║ 8 ║  Höhe des Spielfeldes     ║  ║ 9 ║  Breite des Spielfeldes   ║");
                Program.WriteAt((Breite / 2) - 33, 27, "╚═══╩═══════════════════════════╝  ╚═══╩═══════════════════════════╝");
                Program.WriteAt((Breite / 2) - 33, 28, "╔═══╦═══════════════════════════╗  ╔═══╦═══════════════════════════╗");
                Program.WriteAt((Breite / 2) - 33, 29, "║ S ║  Schlangen verwalten *    ║  ║ Z ║  Zurücksetzen             ║");
                Program.WriteAt((Breite / 2) - 33, 30, "╚═══╩═══════════════════════════╝  ╚═══╩═══════════════════════════╝");

                Program.WriteAt((Breite / 2) - 16, 32, "╔═══╦═══════════════════════════╗");
                Program.WriteAt((Breite / 2) - 16, 33, "║ X ║  Zurück                   ║");
                Program.WriteAt((Breite / 2) - 16, 34, "╚═══╩═══════════════════════════╝");

                Program.WriteAt(Breite - 70, Hoehe - 3, "* Das Programm wird dafür beendet! Änderungen müssen manuell erfolgen.");
                #endregion

                #region Created by Text
                Program.WriteAt(Breite - 20, Hoehe - 1, "Created by KarlKeu00", ConsoleColor.DarkGray);
                #endregion

                gedrueckteTaste = Console.ReadKey();

                switch (gedrueckteTaste.Key) {
                    #region X
                    case ConsoleKey.X:
                    case ConsoleKey.Escape:
                        return;
                    #endregion
                    #region 1
                    case ConsoleKey.D1:
                    case ConsoleKey.NumPad1:
                        EinstellungenMenuAktion("farbeHindernisseSterben", "Spielfeld", "Bitte geben Sie eine neue Farbe für die Hindernisse ein:", "Neue Farbe", Typ.Farbe);
                        break;
                    #endregion
                    #region 2
                    case ConsoleKey.D2:
                    case ConsoleKey.NumPad2:
                        EinstellungenMenuAktion("anzahlHindernisseSterben", "Spielfeld", "Wie viele Hindernisse soll es geben?", "Anzahl Hindernisse", Typ.Zahl, new int[2] { 10, 150 });
                        break;
                    #endregion
                    #region 3
                    case ConsoleKey.D3:
                    case ConsoleKey.NumPad3:
                        EinstellungenMenuAktion("farbeHindernisseLaengerWerden", "Spielfeld", "Bitte geben Sie eine neue Farbe für die Essenspakete ein:", "Neue Farbe", Typ.Farbe);
                        break;
                    #endregion
                    #region 4
                    case ConsoleKey.D4:
                    case ConsoleKey.NumPad4:
                        EinstellungenMenuAktion("anzahlHindernisseLaengerWerden", "Spielfeld", "Wie viele Essenspakete soll es geben?", "Anzahl Essenspakete", Typ.Zahl, new int[2] { 10, 150 });
                        break;
                    #endregion
                    #region 5
                    case ConsoleKey.D5:
                    case ConsoleKey.NumPad5:
                        EinstellungenMenuAktion("farbeHindernisseKuerzerWerden", "Spielfeld", "Bitte geben Sie eine neue Farbe für Schönheitschirurgen ein:", "Neue Farbe", Typ.Farbe);
                        break;
                    #endregion
                    #region 6
                    case ConsoleKey.D6:
                    case ConsoleKey.NumPad6:
                        EinstellungenMenuAktion("anzahlHindernisseKuerzerWerden", "Spielfeld", "Wie viele Schönheitschirurgen soll es geben?", "Anzahl Schönheitschirurgen", Typ.Zahl, new int[2] { 10, 150 });
                        break;
                    #endregion
                    #region 7
                    case ConsoleKey.D7:
                    case ConsoleKey.NumPad7:
                        EinstellungenMenuAktion("farbeBegrenzung", "Spielfeld", "Bitte geben Sie eine neue Farbe für die Spielfeldbegrenzung ein:", "Neue Farbe", Typ.Farbe);
                        break;
                    #endregion
                    #region 8
                    case ConsoleKey.D8:
                    case ConsoleKey.NumPad8:
                        EinstellungenMenuAktion("hoehe", "Spielfeld", "Wie hoch soll das Spielfeld sein?", "Höhe", Typ.Zahl, new int[2] { 40, 60 });
                        Hoehe = Convert.ToInt32(Einstellungen.ini.Read("hoehe", "Spielfeld"));
                        break;
                    #endregion
                    #region 9
                    case ConsoleKey.D9:
                    case ConsoleKey.NumPad9:
                        EinstellungenMenuAktion("breite", "Spielfeld", "Wie breit soll das Spielfeld sein?", "Breite", Typ.Zahl, new int[2] { 80, 200 });
                        Breite = Convert.ToInt32(Einstellungen.ini.Read("breite", "Spielfeld"));
                        break;
                    #endregion
                    #region S
                    case ConsoleKey.S:
                        System.Diagnostics.Process.Start($@"{Environment.GetEnvironmentVariable("userprofile")}\Documents\Cs_Console_Snake_Einstellungen.ini");
                        Environment.Exit(0);
                        break;
                    #endregion
                    #region Z
                    case ConsoleKey.Z:
                        System.IO.File.Delete($@"{Environment.GetEnvironmentVariable("userprofile")}\Documents\Cs_Console_Snake_Einstellungen.ini");

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

                        Hoehe = 50;
                        Breite = 100;

                        break;
                    #endregion
                    #region Alles andere
                    default:
                        Program.WriteAt(0, 0, " ");
                        Console.Beep();
                        break;
                        #endregion
                }
            }
        }

        private static void EinstellungenMenuAktion(string iniVariable, string iniSchluessel, string aufforderungText, string aufforderungTitel, Typ eingabenTyp, int[] limitTypZahl = null) {
            bool einstellungFertigBearbeitet = false;
            string rueckgabeAenderung = "";

            Program.WriteAt(0, 0, " ");
            do {
                try {
                    if (eingabenTyp == Typ.Zahl && limitTypZahl != null && limitTypZahl.Length == 2 && limitTypZahl[0] < limitTypZahl[1]) {
                        rueckgabeAenderung = Microsoft.VisualBasic.Interaction.InputBox($"{aufforderungText} (Min: {limitTypZahl[0]}, Max: {limitTypZahl[1]})", aufforderungTitel, Einstellungen.ini.Read(iniVariable, iniSchluessel));
                    }
                    else {
                        rueckgabeAenderung = Microsoft.VisualBasic.Interaction.InputBox(aufforderungText, aufforderungTitel, Einstellungen.ini.Read(iniVariable, iniSchluessel));
                    }

                    switch (eingabenTyp) {
                        case Typ.Farbe:
                            ConsoleColor neueFarbe = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), rueckgabeAenderung);
                            Einstellungen.ini.Write(iniVariable, neueFarbe.ToString(), iniSchluessel);
                            einstellungFertigBearbeitet = true;
                            break;
                        case Typ.Zahl:
                            int neueZahl = int.Parse(rueckgabeAenderung);
                            if (limitTypZahl != null && limitTypZahl.Length == 2 && limitTypZahl[0] < limitTypZahl[1]) {
                                if (neueZahl >= limitTypZahl[0] && neueZahl <= limitTypZahl[1]) {
                                    Einstellungen.ini.Write(iniVariable, neueZahl.ToString(), iniSchluessel);
                                    einstellungFertigBearbeitet = true;
                                }
                                else {
                                    einstellungFertigBearbeitet = false;
                                }
                            }
                            else {
                                Einstellungen.ini.Write(iniVariable, neueZahl.ToString(), iniSchluessel);
                                einstellungFertigBearbeitet = true;
                            }
                            break;
                        case Typ.Text:
                            Einstellungen.ini.Write(iniVariable, rueckgabeAenderung, iniSchluessel);
                            einstellungFertigBearbeitet = true;
                            break;
                        default:
                            break;
                    }

                } catch (Exception) {
                    einstellungFertigBearbeitet = false;
                }
            } while (!einstellungFertigBearbeitet);
        }
    }

    public enum Typ { Farbe, Zahl, Text }
}
