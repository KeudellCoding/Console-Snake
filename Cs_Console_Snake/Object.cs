using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cs_Console_Snake {
    class AlleObjekte {
        public static List<SpielObjekt> ObjektListe = new List<SpielObjekt> { };

        public static void EinzelnHinzufuegen(int positionX, int positionY, SpielAktion aktionBeiBeruehrung) {
            positionX = (positionX % 2 == 0) ? positionX : positionX + 1;

            if (AktionVonObjektAnOrt(positionX, positionY) == SpielAktion.Nichts) {
                ObjektListe.Add(new SpielObjekt(positionX, positionY, aktionBeiBeruehrung));
            }
        }
        public static void MengeHinzufuegen(int anzahl, SpielAktion aktion, bool direktRendern = false) {
            for (int i = 0; i < anzahl; i++) {
                int randomNumberX;
                int randomNumberY;

                do {
                    randomNumberX = Einstellungen.Random.Next(1, Spielfeld.Breite - 2);
                    randomNumberY = Einstellungen.Random.Next(1, Spielfeld.Hoehe - 2);

                    randomNumberX = (randomNumberX % 2 == 0) ? randomNumberX : randomNumberX + 1;
                } while (AktionVonObjektAnOrt(randomNumberX, randomNumberY) != SpielAktion.Nichts);

                ObjektListe.Add(new SpielObjekt(randomNumberX, randomNumberY, aktion));
                if (direktRendern) {
                    switch (aktion) {
                        case SpielAktion.Sterben:
                            Program.WriteAt(
                                randomNumberX,
                                randomNumberY,
                                "X",
                                (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Einstellungen.ini.Read("farbeHindernisseSterben", "Spielfeld"))
                                );
                            break;
                        case SpielAktion.LaengerWerden:
                            Program.WriteAt(
                                randomNumberX,
                                randomNumberY,
                                "X",
                                (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Einstellungen.ini.Read("farbeHindernisseLaengerWerden", "Spielfeld"))
                                );
                            break;
                        case SpielAktion.KuerzerWerden:
                            Program.WriteAt(
                                randomNumberX,
                                randomNumberY,
                                "X",
                                (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Einstellungen.ini.Read("farbeHindernisseKuerzerWerden", "Spielfeld"))
                                );
                            break;
                        default:
                            break;
                    }
                }
            }
        }

        public static SpielAktion AktionVonObjektAnOrt(int positionX, int positionY) {
            for (int i = 0; i < ObjektListe.Count; i++) {
                if (ObjektListe[i].PositionX == positionX && ObjektListe[i].PositionY == positionY) {
                    return ObjektListe[i].Aktion;
                }
            }
            return SpielAktion.Nichts;
        }

        public static void ObjektAnAndereStelle(int altePositionX, int altePositionY, int neuePositionX, int neuePositionY) {
            for (int i = 0; i < ObjektListe.Count; i++) {
                if (ObjektListe[i].PositionX == altePositionX && ObjektListe[i].PositionY == altePositionY) {

                    Program.WriteAt(altePositionX, altePositionY, " ");
                    switch (ObjektListe[i].Aktion) {
                        case SpielAktion.Sterben:
                            ObjektListe[i].PositionX = neuePositionX;
                            ObjektListe[i].PositionY = neuePositionY;
                            Program.WriteAt(neuePositionX, neuePositionY, "X", ConsoleColor.Red);
                            break;
                        case SpielAktion.LaengerWerden:
                            ObjektListe[i].PositionX = neuePositionX;
                            ObjektListe[i].PositionY = neuePositionY;
                            Program.WriteAt(neuePositionX, neuePositionY, "X", ConsoleColor.Green);
                            break;
                        case SpielAktion.KuerzerWerden:
                            ObjektListe[i].PositionX = neuePositionX;
                            ObjektListe[i].PositionY = neuePositionY;
                            Program.WriteAt(neuePositionX, neuePositionY, "X", ConsoleColor.Blue);
                            break;
                        default:
                            return;
                    }
                    return;
                }
            }
        }
        public static void ObjektAnAndereStelle(int altePositionX, int altePositionY) {
            for (int i = 0; i < ObjektListe.Count; i++) {
                if (ObjektListe[i].PositionX == altePositionX && ObjektListe[i].PositionY == altePositionY) {
                    Program.WriteAt(altePositionX, altePositionY, " ");
                    MengeHinzufuegen(1, ObjektListe[i].Aktion, true);
                    ObjektListe.RemoveAt(i);
                    return;
                }
            }
        }

        public static void ObjekteRendern() {
            for (int i = 0; i < ObjektListe.Count; i++) {
                switch (ObjektListe[i].Aktion) {
                    case SpielAktion.Sterben:
                        Program.WriteAt(
                            ObjektListe[i].PositionX,
                            ObjektListe[i].PositionY,
                            "X",
                            (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Einstellungen.ini.Read("farbeHindernisseSterben", "Spielfeld"))
                            );
                        break;
                    case SpielAktion.LaengerWerden:
                        Program.WriteAt(
                            ObjektListe[i].PositionX,
                            ObjektListe[i].PositionY,
                            "X",
                            (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Einstellungen.ini.Read("farbeHindernisseLaengerWerden", "Spielfeld"))
                            );
                        break;
                    case SpielAktion.KuerzerWerden:
                        Program.WriteAt(
                            ObjektListe[i].PositionX,
                            ObjektListe[i].PositionY,
                            "X",
                            (ConsoleColor)Enum.Parse(typeof(ConsoleColor), Einstellungen.ini.Read("farbeHindernisseKuerzerWerden", "Spielfeld"))
                            );
                        break;
                    default:
                        break;
                }
            }
        }

    }

    class SpielObjekt {
        public int PositionX;
        public int PositionY;
        public SpielAktion Aktion;

        public SpielObjekt(int positionX, int positionY, SpielAktion aktionBeiBeruehrung) {
            PositionX = positionX;
            PositionY = positionY;
            Aktion = aktionBeiBeruehrung;
        }
    }

    public enum SpielAktion {
        Nichts, Sterben, LaengerWerden, KuerzerWerden
    }
}
