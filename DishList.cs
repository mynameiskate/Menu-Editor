using System;
using System.Collections.ObjectModel;
using System.Xml.Serialization;

namespace DishesHierarchy
{
    [Serializable]
    [XmlRoot("dish_list")]
    [XmlInclude(typeof(Drinks)), XmlInclude(typeof(MainCourse)), XmlInclude(typeof(Snacks)),
     XmlInclude(typeof(Dessert))]
    public class DishList
    {
        [XmlElement("Dish", typeof(Dish))]
        public ObservableCollection<Dish> Menu { get; private set; }

        public DishList()
        { 
            Menu = new ObservableCollection<Dish>();
        }

        public void FillList(ObservableCollection<Dish> list = null)
        {
            if (list != null)
            {
                Menu = list;
            }
            else
            {
                var ingredients = new ObservableCollection<Ingredient>()
                {
                  new DairyProduct("Cow milk", 0.6f, DairyProduct.Group.Milk, 250),
                  new DairyProduct("Vanilla ice cream", 2.01f, DairyProduct.Group.IceCream, 100),
                  new Fruit("Banana", 0.89f, "Yellow", 0.12f, 150)
                };

                Menu.Add(new Drinks("Milkshake", new IngredientList(ingredients), Drinks.TemperatureType.Icy));

                ingredients = new ObservableCollection<Ingredient>()
                {
                  new DairyProduct("Cow milk", 0.6f, DairyProduct.Group.Milk, 100),
                  new DairyProduct("Cheese", 4.02f, DairyProduct.Group.Cheese, 40),
                  new Vegetable("Tomato", 0.89f, Vegetable.PungencyDegree.Mild, "Red", 40),
                  new Ingredient("Eggs", 2f, 80)
                };

                Menu.Add(new MainCourse("Omelette", new IngredientList(ingredients), true, MainCourse.MealType.Breakfast)); 
            }
        }

        public bool RemoveItem(object dish)
        {
            try
            {
                Menu.Remove(dish as Dish);
                return true;
            }
            catch
            {
                return false;
            }            
        }

        public void AddItem(string classType)
        {
            string formTypeFullName = string.Format("{0}.{1}", GetType().Namespace, classType);
            Type type = Type.GetType(formTypeFullName, true);
            Dish item = (Dish)Activator.CreateInstance(type);
            Menu.Add(item);
        }
    }
}
