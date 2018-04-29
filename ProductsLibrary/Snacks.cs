using System;

namespace DishesHierarchy
{
    [Serializable]
    public class Snacks : Dish
    {
        public Snacks(string name, IngredientList ingredients, Snack type, Drinks drink) : base (name, ingredients)
        {
            SnackType = type;
            ServedWith = drink;
        }

        public Snacks() : base()
        {
            SnackType = Snack.Sandwich;
        }

        public Drinks ServedWith { get; set; }

        public Snack SnackType { get; set; }

        public enum Snack
        {
            Nuts,
            Sandwich,
            Crisps,
            DriedSquid,
            DriedFish,
            Nachos
        }
        
    }
}
