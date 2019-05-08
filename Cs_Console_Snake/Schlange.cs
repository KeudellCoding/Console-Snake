using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs_Console_Snake {
    class Schlange {
        public List<int[]> KoerperKoordinaten = new List<int[]>();
        public string LetzteTaste = "";
        public int[] StartPosition = new int[2];

        public int LaengsteLaenge = 0;

        public ConsoleKey TasteHoch = ConsoleKey.UpArrow;
        public ConsoleKey TasteRunter = ConsoleKey.DownArrow;
        public ConsoleKey TasteLinks = ConsoleKey.LeftArrow;
        public ConsoleKey TasteRechts = ConsoleKey.RightArrow;

        public ConsoleColor SchlangenFarbe = ConsoleColor.White;

        private int SchlangenNummer;

        public Schlange(int startX, int startY, ConsoleKey tasteHoch, ConsoleKey tasteRunter, ConsoleKey tasteLinks, ConsoleKey tasteRechts, ConsoleColor schlangenFarbe, int schlangenNummer) {
            startX = (startX % 2 == 0) ? startX : startX + 1;

            while (AlleObjekte.AktionVonObjektAnOrt(startX, startY) != SpielAktion.Nichts) {
                startX = Einstellungen.Random.Next(1, Spielfeld.Breite - 2);
                startY = Einstellungen.Random.Next(1, Spielfeld.Hoehe - 2);

                startX = (startX % 2 == 0) ? startX : startX + 1;
            }

            TasteHoch = tasteHoch;
            TasteRunter = tasteRunter;
            TasteLinks = tasteLinks;
            TasteRechts = tasteRechts;

            SchlangenFarbe = schlangenFarbe;

            SchlangenNummer = schlangenNummer;

            KoerperKoordinaten.Add(new int[2] { startX, startY });
            StartPosition[0] = startX;
            StartPosition[1] = startY;
            Program.WriteAt(startX, startY, (SchlangenNummer + 1).ToString(), SchlangenFarbe);
        }

        public void Schritt(string richtung) {
            int[] neuerKoerperPunkt = new int[2] { 0, 0 };

            switch (richtung) {
                case "hoch":
                    neuerKoerperPunkt[0] = KoerperKoordinaten[KoerperKoordinaten.Count - 1][0];
                    neuerKoerperPunkt[1] = KoerperKoordinaten[KoerperKoordinaten.Count - 1][1] - 1;
                    SchrittAktion(neuerKoerperPunkt[0], neuerKoerperPunkt[1], "^");
                    break;
                case "runter":
                    neuerKoerperPunkt[0] = KoerperKoordinaten[KoerperKoordinaten.Count - 1][0];
                    neuerKoerperPunkt[1] = KoerperKoordinaten[KoerperKoordinaten.Count - 1][1] + 1;
                    SchrittAktion(neuerKoerperPunkt[0], neuerKoerperPunkt[1], "v");
                    break;
                case "links":
                    neuerKoerperPunkt[0] = KoerperKoordinaten[KoerperKoordinaten.Count - 1][0] - 2;
                    neuerKoerperPunkt[1] = KoerperKoordinaten[KoerperKoordinaten.Count - 1][1];
                    SchrittAktion(neuerKoerperPunkt[0], neuerKoerperPunkt[1], "<");
                    break;
                case "rechts":
                    neuerKoerperPunkt[0] = KoerperKoordinaten[KoerperKoordinaten.Count - 1][0] + 2;
                    neuerKoerperPunkt[1] = KoerperKoordinaten[KoerperKoordinaten.Count - 1][1];
                    SchrittAktion(neuerKoerperPunkt[0], neuerKoerperPunkt[1], ">");
                    break;
                default:
                    Einstellungen.InGame = false;
                    return;
            }
            if (UeberpruefeKollision(neuerKoerperPunkt[0], neuerKoerperPunkt[1], true)) {
                Einstellungen.InGame = false;
                return;
            }
        }

        private void SchrittAktion(int neuerKoerperPunktX, int neuerKoerperPunktY, string symbol) {
            SpielAktion aktionAnNeuerStelle = AlleObjekte.AktionVonObjektAnOrt(neuerKoerperPunktX, neuerKoerperPunktY);

            if (KoerperKoordinaten.Count > 1 && aktionAnNeuerStelle == SpielAktion.Nichts) {
                Program.WriteAt(KoerperKoordinaten[0][0], KoerperKoordinaten[0][1], " ");
                KoerperKoordinaten.RemoveAt(0);
            }
            else if (aktionAnNeuerStelle == SpielAktion.KuerzerWerden) {
                if (KoerperKoordinaten.Count < 3) {
                    Einstellungen.InGame = false;
                    return;
                }
                else {
                    for (int i = 0; i < 2; i++) {
                        Program.WriteAt(KoerperKoordinaten[0][0], KoerperKoordinaten[0][1], " ");
                        KoerperKoordinaten.RemoveAt(0);
                    }
                    AlleObjekte.ObjektAnAndereStelle(neuerKoerperPunktX, neuerKoerperPunktY);
                }
            }
            else if (aktionAnNeuerStelle == SpielAktion.LaengerWerden) {
                AlleObjekte.ObjektAnAndereStelle(neuerKoerperPunktX, neuerKoerperPunktY);
                LaengsteLaenge = KoerperKoordinaten.Count + 1;
            }
            else if (aktionAnNeuerStelle == SpielAktion.Sterben) {
                Einstellungen.InGame = false;
                return;
            }
            KoerperKoordinaten.Add(new int[2] { neuerKoerperPunktX, neuerKoerperPunktY });
            Program.WriteAt(neuerKoerperPunktX, neuerKoerperPunktY, symbol, SchlangenFarbe);
        }

        private bool UeberpruefeKollision(int posX, int posY, bool schritt = false) {
            //Wenn sich die Schlange selbst berührt
            for (int i = 0; i < KoerperKoordinaten.Count - 1; i++) {
                if (KoerperKoordinaten[i][0] == posX && KoerperKoordinaten[i][1] == posY) {
                    return true;
                }
            }

            //Wenn die Begrenzung berührt wird
            if (posX < 1 || posX > Spielfeld.Breite - 2) {
                return true;
            }
            if (posY < 1 || posY > Spielfeld.Hoehe - 2) {
                return true;
            }
            return false;

        }

        public void Zuruecksetzen() {
            KoerperKoordinaten.RemoveRange(0, KoerperKoordinaten.Count);
            LetzteTaste = "";

            while (AlleObjekte.AktionVonObjektAnOrt(StartPosition[0], StartPosition[1]) != SpielAktion.Nichts) {
                StartPosition[0] = Einstellungen.Random.Next(1, Spielfeld.Breite - 2);
                StartPosition[1] = Einstellungen.Random.Next(1, Spielfeld.Hoehe - 2);

                StartPosition[0] = (StartPosition[0] % 2 == 0) ? StartPosition[0] : StartPosition[0] + 1;
            }

            KoerperKoordinaten.Add(new int[2] { StartPosition[0], StartPosition[1] });
            StartPosition[0] = StartPosition[0];
            StartPosition[1] = StartPosition[1];
            Program.WriteAt(StartPosition[0], StartPosition[1], (SchlangenNummer + 1).ToString(), SchlangenFarbe);
        }
    }
}
