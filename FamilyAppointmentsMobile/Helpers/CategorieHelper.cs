using FamilyAppointmentsMobile.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FamilyAppointmentsMobile.Helpers
{
    public static class CategorieHelper
    {
        public static string HandleEnumCategorieType(ECategorieType categorieType)
        {
            return categorieType switch
            {
                ECategorieType.None => "Sonstiges",
                ECategorieType.BrotUndBackwaren => "Brot und Backwaren",
                ECategorieType.ObstUndGemuese => "Obst und Gemüse",
                ECategorieType.FleischUndWurst => "Fleisch und Wurstwaren",
                ECategorieType.FischUndMeeresfruechte => "Fisch und Meeresfrüchte",
                ECategorieType.Milchprodukte => "Milchprodukte",
                ECategorieType.Tiefkuehlkost => "Tiefkühlkost",
                ECategorieType.Getraenke => "Getränke",
                ECategorieType.TrockenProdukte_Nudeln_Reis_Mehl => "Trockenprodukte",
                ECategorieType.SnacksUndSuesswaren => "Snacks und Süßigkeiten",
                ECategorieType.GewuerzeUndSaucen => "Gewürze und Saucen",
                ECategorieType.KonservenUndFertigGerichte => "Konserven und Fertiggerichte",
                ECategorieType.Drogerie => "Drogerie",
                ECategorieType.Spirituosen => "Spirituosen",
                ECategorieType.Haushaltwaren => "Haushaltwaren",
                ECategorieType.Tierbedarf => "Tierbedarf",
                ECategorieType.Schreibwaren => "Schreibwaren",
                ECategorieType.Elektronik => "Elektronik",
                ECategorieType.KleidungUndSchuhe => "Kleidung und Schuhe",
                ECategorieType.Spielwaren => "Spielwaren",
                ECategorieType.BuecherUndZeitschriften => "Bücher und Zeitschriften",
                ECategorieType.Blumen => "Blumen und Pflanzen",
                ECategorieType.SportUndFreizeit => "Sport und Freizeit",
                _ => "Sonstiges" // Fallback für unbekannte Kategorien
            };
        }

        public static ECategorieType HandleStringCategorieType(string categorie)
        {
            var result = ECategorieType.None;
            switch (categorie)
            {
                case "Sonstiges": result = ECategorieType.None; break;
                case "Brot und Backwaren": result = ECategorieType.BrotUndBackwaren; break;
                case "Obst und gemüse": result = ECategorieType.ObstUndGemuese; break;
                case "Fleisch und Wurstwaren": result = ECategorieType.FleischUndWurst; break;
                case "Fisch und Meeresfrüchte": result = ECategorieType.FischUndMeeresfruechte; break;
                case "Milchprodukte": result = ECategorieType.Milchprodukte; break;
                case "Tiefkühlkost": result = ECategorieType.Tiefkuehlkost; break;
                case "Getränke": result = ECategorieType.Getraenke; break;
                case "Trockenprodukte": result = ECategorieType.TrockenProdukte_Nudeln_Reis_Mehl; break;
                case "Snacks und Süßigkeiten": result = ECategorieType.SnacksUndSuesswaren; break;
                case "Gewürze und Saucen": result = ECategorieType.GewuerzeUndSaucen; break;
                case "Konserven und Fertiggerichte": result = ECategorieType.KonservenUndFertigGerichte; break;
                case "Drogerie": result = ECategorieType.Drogerie; break;
                case "Spirituosen": result = ECategorieType.Spirituosen; break;
                case "Haushaltwaren": result = ECategorieType.Haushaltwaren; break;
                case "Tierbedarf": result = ECategorieType.Tierbedarf; break;
                case "Schreibwaren": result = ECategorieType.Schreibwaren; break;
                case "Elektronik": result = ECategorieType.Elektronik; break;
                case "Kleidung und Schuhe": result = ECategorieType.KleidungUndSchuhe; break;
                case "Spielwaren": result = ECategorieType.Spielwaren; break;
                case "Bücher und Zeitschriften": result = ECategorieType.BuecherUndZeitschriften; break;
                case "Blumen und Pflanzen": result = ECategorieType.Blumen; break;
                case "Sport und Freizeit": result = ECategorieType.SportUndFreizeit; break;
            }
            return result;
        }
    }
}
