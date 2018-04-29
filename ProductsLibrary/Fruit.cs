using System;

namespace DishesHierarchy
{
    [Serializable]
    public class Fruit : Ingredient
    {
        public string Colour { get; set; }
        public float Sugars { get; set; }

        public Fruit(string name, float calories, string colour, float sugars, float weight) : base (name, calories, weight)
        {
            Colour = colour;
            Sugars = sugars;   
        }

        public Fruit() { }
    }
}
