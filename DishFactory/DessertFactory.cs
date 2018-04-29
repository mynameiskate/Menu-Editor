namespace DishesHierarchy.DishFactory
{
    class DessertFactory : IFactory
    {
        public Dish GetItem()
        {
            return new Dessert();
        }
    }
}
