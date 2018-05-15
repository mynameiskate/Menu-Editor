using System;
using System.Xml.Serialization;

namespace DishesHierarchy
{
    [Serializable]
    public class MainCourse : Dish
    {
        public MainCourse(string name, IngredientList ingredients, bool vegetarian, MealType meal) : base(name, ingredients)
        {
            Meal = meal;
            Vegeterian = vegetarian;
        }

        public MainCourse() : base()
        {
            Vegeterian = false;
            Meal = MealType.Brunch;
        }

        [XmlElement("vegetarian")]
        public bool Vegeterian { get; set; }
        [XmlElement("meal")]
        public MealType Meal { get; set; }

        public enum MealType
        {
            Breakfast,
            Brunch,
            Lunch,
            Dinner,
            Supper,
            Tea
        }
    }
}
