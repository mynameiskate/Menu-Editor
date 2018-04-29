using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Xml.Serialization;

namespace DishesHierarchy
{
    [Serializable]
    [XmlInclude(typeof(DairyProduct)), XmlInclude(typeof(Fruit)), XmlInclude(typeof(Vegetable)),
     XmlInclude(typeof(Meat)), XmlInclude(typeof(Grains))]

    public class IngredientList : INotifyPropertyChanged
    {
        public ObservableCollection<Ingredient> Ingredients { get; private set; }

        public IngredientList()
        {
            Ingredients = new ObservableCollection<Ingredient>();
        }

        public IngredientList(ObservableCollection<Ingredient> list)
        {
            Ingredients = list;
            foreach (Ingredient ingredient in list)
            {
                ingredient.PropertyChanged += OnPropertyChanged;
            }
        }

        public void AddItem(Ingredient ingredient)
        {
            Ingredients.Add(ingredient);
            ingredient.PropertyChanged += OnPropertyChanged;
            OnPropertyChanged(this, new PropertyChangedEventArgs("IngredientList"));
        }

        public void RemoveItem(Ingredient ingredient, int index)
        {
            ingredient.PropertyChanged -= OnPropertyChanged;
            Ingredients.RemoveAt(index);
            OnPropertyChanged(this, new PropertyChangedEventArgs("IngredientList"));
        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged(object o, PropertyChangedEventArgs e)
        {
           PropertyChanged?.Invoke(this, e);
        }
    }
}
