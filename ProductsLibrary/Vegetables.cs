using System;

namespace DishesHierarchy
{
    [Serializable]
    public class Vegetable : Ingredient
    {
        public string Colour { get; set; }
        public PungencyDegree Pungency { get; set; }

        public enum PungencyDegree
        {
            None,
            Mild,
            Hot,
            Extreme
        }

        public Vegetable (string name, float calories, PungencyDegree pungency, string colour, float weight) : base (name, calories, weight)
        {
            Pungency = pungency;
            Colour = colour;
        }

        public Vegetable() {}
    }
}
