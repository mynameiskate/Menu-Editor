using System;

namespace DishesHierarchy
{
    [Serializable]
    public class Meat : Ingredient
    {
        public MeatType Origin { get; set; }

        public enum MeatType
        {
            Pork, 
            Chicken,
            Beef,
            Mutton,
            Turkey,
            Venison,
            Duck,
            WildBoar,
            Rabbit,
            Ostrich,
            Fish
        }

        public Meat (string name, float calories, MeatType origin, float weight) : base(name, calories, weight)
        {
            Origin = origin;
        }

        public Meat() {}
    }
}
