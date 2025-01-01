using FamilyAppointmentsMobile.Models;

namespace FamilyAppointmentsMobile.Helpers
{
    public static class CategorieHelper
    {
        public static string HandleEnumCategorieType(ECategorieType categorieType)
        {
            return categorieType switch
            {
                ECategorieType.None => Constants.Sonstiges,
                ECategorieType.BrotUndBackwaren => Constants.BrotUndBackwaren,
                ECategorieType.ObstUndGemuese => Constants.ObstUndGemuese,
                ECategorieType.FleischUndWurst => Constants.FleischUndWurst,
                ECategorieType.FischUndMeeresfruechte => Constants.FischUndMeer,
                ECategorieType.Milchprodukte => Constants.Milchprodukte,
                ECategorieType.Tiefkuehlkost => Constants.Tiefkuehlkost,
                ECategorieType.Getraenke => Constants.Getraenke,
                ECategorieType.TrockenProdukte_Nudeln_Reis_Mehl => Constants.TrockenProdukte,
                ECategorieType.SnacksUndSuesswaren => Constants.Snacks,
                ECategorieType.GewuerzeUndSaucen => Constants.Gewuerze,
                ECategorieType.KonservenUndFertigGerichte => Constants.Konserven,
                ECategorieType.Drogerie => Constants.Drogerie,
                ECategorieType.Spirituosen => Constants.Spirituosen,
                ECategorieType.Haushaltwaren => Constants.Haushaltwaren,
                ECategorieType.Tierbedarf => Constants.Tierbedarf,
                ECategorieType.Schreibwaren => Constants.Schreibwaren,
                ECategorieType.Elektronik => Constants.Elektronik,
                ECategorieType.KleidungUndSchuhe => Constants.Kleidung,
                ECategorieType.Spielwaren => Constants.Spielwaren,
                ECategorieType.BuecherUndZeitschriften => Constants.BuecherZeitschriften,
                ECategorieType.Blumen => Constants.BlumenPflanzen,
                ECategorieType.SportUndFreizeit => Constants.SportFreizeit,
                _ => Constants.Sonstiges 
            };
        }

        public static ECategorieType HandleStringCategorieType(string categorie)
        {
            return categorie switch
            {
                Constants.Sonstiges => ECategorieType.None,
                Constants.BrotUndBackwaren => ECategorieType.BrotUndBackwaren,
                Constants.ObstUndGemuese => ECategorieType.ObstUndGemuese,
                Constants.FleischUndWurst => ECategorieType.FleischUndWurst,
                Constants.FischUndMeer => ECategorieType.FischUndMeeresfruechte,
                Constants.Milchprodukte => ECategorieType.Milchprodukte,
                Constants.Tiefkuehlkost => ECategorieType.Tiefkuehlkost,
                Constants.Getraenke => ECategorieType.Getraenke,
                Constants.TrockenProdukte => ECategorieType.TrockenProdukte_Nudeln_Reis_Mehl,
                Constants.Snacks => ECategorieType.SnacksUndSuesswaren,
                Constants.Gewuerze => ECategorieType.GewuerzeUndSaucen,
                Constants.Konserven => ECategorieType.KonservenUndFertigGerichte,
                Constants.Drogerie => ECategorieType.Drogerie,
                Constants.Spirituosen => ECategorieType.Spirituosen,
                Constants.Haushaltwaren => ECategorieType.Haushaltwaren,
                Constants.Tierbedarf => ECategorieType.Tierbedarf,
                Constants.Schreibwaren => ECategorieType.Schreibwaren,
                Constants.Elektronik => ECategorieType.Elektronik,
                Constants.Kleidung => ECategorieType.KleidungUndSchuhe,
                Constants.Spielwaren => ECategorieType.Spielwaren,
                Constants.BuecherZeitschriften => ECategorieType.BuecherUndZeitschriften,
                Constants.BlumenPflanzen => ECategorieType.Blumen,
                Constants.SportFreizeit => ECategorieType.SportUndFreizeit,
                _ => ECategorieType.None
            };
        }

        public static List<Categorie> SetCategories()
        {
            return new List<Categorie>
            {
                new Categorie { Name = Constants.Sonstiges },
                new Categorie { Name = Constants.BrotUndBackwaren },
                new Categorie { Name = Constants.ObstUndGemuese },
                new Categorie { Name = Constants.FleischUndWurst },
                new Categorie { Name = Constants.FischUndMeer },
                new Categorie { Name = Constants.Milchprodukte },
                new Categorie { Name = Constants.Tiefkuehlkost },
                new Categorie { Name = Constants.Getraenke },
                new Categorie { Name = Constants.TrockenProdukte },
                new Categorie { Name = Constants.Snacks },
                new Categorie { Name = Constants.Gewuerze },
                new Categorie { Name = Constants.Konserven },
                new Categorie { Name = Constants.Drogerie },
                new Categorie { Name = Constants.Spirituosen },
                new Categorie { Name = Constants.Haushaltwaren },
                new Categorie { Name = Constants.Tierbedarf },
                new Categorie { Name = Constants.Schreibwaren },
                new Categorie { Name = Constants.Elektronik },
                new Categorie { Name = Constants.Kleidung },
                new Categorie { Name = Constants.Spielwaren },
                new Categorie { Name = Constants.BuecherZeitschriften },
                new Categorie { Name = Constants.BlumenPflanzen },
                new Categorie { Name = Constants.SportFreizeit }
            };
        }
    }
}
