
using System.Collections.ObjectModel;

namespace DishesHierarchy.DishFactory
{
    class MainCourseFactory : IFactory
    {
        public Dish GetItem(string name, IngredientList ingredients, bool vegetarian, MainCourse.MealType meal)
        {
            return new MainCourse(name, ingredients, vegetarian, meal);
        }

        public Dish GetItem()
        {
            return new MainCourse();
        }
    }
}
