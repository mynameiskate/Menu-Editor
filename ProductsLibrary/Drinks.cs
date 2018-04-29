using System;

namespace DishesHierarchy
{
    [Serializable]
    public class Drinks : Dish
    {
        public Drinks(string name, IngredientList ingredients, TemperatureType degree) : base(name, ingredients)
        {
            Temperature = degree;
        }

        public Drinks() : base()
        {
            Temperature = TemperatureType.Cold;
        }

        public TemperatureType Temperature { get; set; }

        public enum TemperatureType
        {
            Warm,
            Cold,
            Icy,
            Hot
        }
    }
}
