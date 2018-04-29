namespace DishesHierarchy.DishFactory
{
    class DrinksFactory : IFactory
    {
        public Dish GetItem()
        {
            return new Drinks();
        }
    }
}
