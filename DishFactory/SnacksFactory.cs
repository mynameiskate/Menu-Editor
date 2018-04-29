namespace DishesHierarchy.DishFactory
{
    class SnacksFactory : IFactory
    {
        public Dish GetItem()
        {
            return new Snacks();
        }
    }
}
