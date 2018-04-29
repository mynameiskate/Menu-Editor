using System;

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

        public bool Vegeterian { get; set; }
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
