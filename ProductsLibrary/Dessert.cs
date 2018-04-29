using System;
using System.Collections.Generic;
using System.Linq;

namespace DishesHierarchy
{
    [Serializable]
    public class Dessert : Dish
    {
        public Cream CreamType { get; set; }
        public Chocolate ChocolateType { get; set; }
        public List<Fruit> Fruits { get; set; }

        public Dessert(string name, List<Ingredient> ingredients, Cream cream, Chocolate chocolate) : base(name)
        {
            Fruits = ingredients.OfType<Fruit>().ToList();
            CreamType = cream;
            ChocolateType = chocolate;
        }

        public Dessert() : base()
        {
            Fruits = new List<Fruit>();
            CreamType = Cream.Buttercream;
            ChocolateType = Chocolate.Bittersweet;
        }

        public enum Cream
        {
            Buttercream,
            Creamcheese,
            Clotted,
            Double,
            Soured,
            Single,
            Whipping,
            None
        }

        public enum Chocolate
        {
            Milk,
            Dark,
            White,
            Bittersweet,
            Unsweetened,
            Semisweet,
            Couverture,
            Ruby,
            Modeling,
            None
        }
    }
}
