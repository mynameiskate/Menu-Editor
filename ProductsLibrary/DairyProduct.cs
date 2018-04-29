using System;

namespace DishesHierarchy
{
    [Serializable]
    public class DairyProduct : Ingredient
    {
        public Group Origin { get; set; }

        public enum Group
        {
            Milk,
            Cheese,
            Butter,
            Yoghurt,
            IceCream,
            SourCream
        }

        public DairyProduct (string type, float calories, Group name, float weight) : base (type, calories, weight)
        {
            Origin = name;
        }

        public DairyProduct()
        {
        }
    }
}
