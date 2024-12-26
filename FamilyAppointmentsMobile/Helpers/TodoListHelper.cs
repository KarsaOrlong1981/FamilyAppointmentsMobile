using FamilyAppointmentsMobile.Models;

namespace FamilyAppointmentsMobile.Helpers
{
    public static class TodoListHelper
    {
        private static List<TodoTask> GetPredefinedShoppingList(Action<TodoTask> removeAction)
        {
            return new List<TodoTask>()
            {
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Milch", IsDone = false,CategorieType = ECategorieType.Milchprodukte },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Brot", IsDone = false,CategorieType = ECategorieType.BrotUndBackwaren },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Butter", IsDone = false,CategorieType = ECategorieType.Milchprodukte },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Käse", IsDone = false , CategorieType = ECategorieType.Milchprodukte},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Eier", IsDone = false ,CategorieType = ECategorieType.Milchprodukte },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Kartoffeln", IsDone = false, CategorieType = ECategorieType.ObstUndGemuese },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Tomaten", IsDone = false ,CategorieType = ECategorieType.ObstUndGemuese },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Zwiebeln", IsDone = false , CategorieType = ECategorieType.ObstUndGemuese},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Äpfel", IsDone = false,CategorieType = ECategorieType.ObstUndGemuese },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Bananen", IsDone = false , CategorieType = ECategorieType.ObstUndGemuese},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Fleisch", IsDone = false , CategorieType = ECategorieType.FleischUndWurst},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Fisch", IsDone = false, CategorieType = ECategorieType.FischUndMeeresfruechte },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Reis", IsDone = false ,CategorieType = ECategorieType.TrockenProdukte_Nudeln_Reis_Mehl },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Nudeln", IsDone = false , CategorieType = ECategorieType.TrockenProdukte_Nudeln_Reis_Mehl},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Mehl", IsDone = false, CategorieType = ECategorieType.TrockenProdukte_Nudeln_Reis_Mehl },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Zucker", IsDone = false,CategorieType = ECategorieType.TrockenProdukte_Nudeln_Reis_Mehl },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Kaffee", IsDone = false , CategorieType = ECategorieType.TrockenProdukte_Nudeln_Reis_Mehl},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Tee", IsDone = false, CategorieType = ECategorieType.TrockenProdukte_Nudeln_Reis_Mehl},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Seife", IsDone = false,CategorieType = ECategorieType.Drogerie },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Zahnpasta", IsDone = false ,CategorieType = ECategorieType.Drogerie },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Wurst", IsDone = false, CategorieType = ECategorieType.FleischUndWurst},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Leberwurst", IsDone = false, CategorieType = ECategorieType.FleischUndWurst },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Kochschinken", IsDone = false, CategorieType = ECategorieType.FleischUndWurst},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Toilettenpapier", IsDone = false, CategorieType = ECategorieType.Drogerie },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Chips", IsDone = false,CategorieType = ECategorieType.SnacksUndSuesswaren },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Puddings oder Joghurt", IsDone = false,CategorieType = ECategorieType.Milchprodukte},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Skyr", IsDone = false,CategorieType = ECategorieType.Milchprodukte },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Fertig Schnitzel", IsDone = false,CategorieType = ECategorieType.FleischUndWurst },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Katzenfutter", IsDone = false,CategorieType = ECategorieType.Tierbedarf },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Shampoo", IsDone = false,CategorieType = ECategorieType.Drogerie },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Duschgel", IsDone = false,CategorieType = ECategorieType.Drogerie },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Süßigkeiten", IsDone = false,CategorieType = ECategorieType.SnacksUndSuesswaren },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Spezi", IsDone = false, CategorieType = ECategorieType.Getraenke },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Fanta", IsDone = false, CategorieType = ECategorieType.Getraenke },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Cola", IsDone = false, CategorieType = ECategorieType.Getraenke },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Bier", IsDone = false, CategorieType = ECategorieType.Getraenke },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Süßer Wein", IsDone = false , CategorieType = ECategorieType.Spirituosen},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Rotwein", IsDone = false, CategorieType = ECategorieType.Spirituosen },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Pfeffer", IsDone=false, CategorieType = ECategorieType.GewuerzeUndSaucen },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Salz", IsDone=false, CategorieType = ECategorieType.GewuerzeUndSaucen },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Chilli Pulver", IsDone=false, CategorieType = ECategorieType.GewuerzeUndSaucen },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Tabasco", IsDone=false, CategorieType = ECategorieType.GewuerzeUndSaucen },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Oregano", IsDone=false, CategorieType = ECategorieType.GewuerzeUndSaucen },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Thymian", IsDone=false, CategorieType = ECategorieType.GewuerzeUndSaucen },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Curry Pulver", IsDone=false, CategorieType = ECategorieType.GewuerzeUndSaucen },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Salat", IsDone=false, CategorieType = ECategorieType.ObstUndGemuese },
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Salat Krönung", IsDone=false , CategorieType = ECategorieType.GewuerzeUndSaucen},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Mayo", IsDone=false , CategorieType = ECategorieType.GewuerzeUndSaucen},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Ketchup", IsDone=false , CategorieType = ECategorieType.GewuerzeUndSaucen},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Toast", IsDone=false , CategorieType = ECategorieType.BrotUndBackwaren},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Spülmaschinen Tabs", IsDone=false , CategorieType = ECategorieType.Haushaltwaren},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Waschmittel", IsDone=false , CategorieType = ECategorieType.Haushaltwaren},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Weichspühler", IsDone=false , CategorieType = ECategorieType.Haushaltwaren},
                new TodoTask (removeAction)  { Id = Guid.NewGuid().ToString(), Description = "Nutela", IsDone=false , CategorieType = ECategorieType.BrotUndBackwaren},
            };
        }

        public static List<TodoTask> GetSortedPredefiendShoppingItemsList(Action<TodoTask> removeAction) 
        {
            return GetPredefinedShoppingList(removeAction)
                .OrderBy(task => task.Description)
                .ToList();
        }

        //public static List<string> GetSortedPredefiendShoppingStringItemsList()
        //{
        //    var stringList = new List<string>();
        //    foreach (var item in GetPredefinedShoppingList())
        //    {
        //        stringList.Add(item.Description);
        //    }
        //    return stringList.OrderBy(s => s).ToList();
        //}
    }
}
