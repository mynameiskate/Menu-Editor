using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace DishesHierarchy
{
    [Serializable]
    public class Dish : Food, IDataErrorInfo
    {
        public IngredientList Ingredients { get; set; }
        private string _name;

        public string Name
        {
            get
            {
                return _name;
            }

            set
            {
                _name = value;
                OnPropertyChanged("Name");
            }
        }
        public float CaloriesPerServing
        {
            get
            {
                return Calories;
            }
            set
            {
                Calories = value;
                OnPropertyChanged("CaloriesPerServing");
            }
        }

        public float PortionSize
        {
            get
            {
                return Weight;
            }

            set
            {
                Weight = value;
                OnPropertyChanged("PortionSize");
            }
        }

        public void UpdateInfo()
        {
            float sum = 0;
            float calories = 0;          
            foreach (var ingredient in Ingredients.Ingredients)
            {
                sum += ingredient.Weight;
                calories += ingredient.CaloriesPerGram * ingredient.Weight;
            }
            PortionSize = sum;
            CaloriesPerServing = calories;
        }

        public Dish (string name, IngredientList ingredients)
        {
            Name = name;
            Ingredients = ingredients;
            ingredients.PropertyChanged += Ingredients_PropertyChanged;
            UpdateInfo();
        }

        private void Ingredients_PropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            UpdateInfo();
        }

        public Dish (string name)
        {
            Name = name;
        }

        public Dish()
        {
            Ingredients = new IngredientList();
            Ingredients.PropertyChanged += Ingredients_PropertyChanged;
            Name = "New dish";
        }

        public string this[string columnName]
        {
            get
            {
                string error = String.Empty;
                try
                {
                    switch (columnName)
                    {
                        case "Name":
                            if (!Regex.IsMatch(Name, @"^[a-zA-Z]+[-']?$"))
                            {
                                error = "Invalid dish name.";
                            }
                            break;
                        case "PortionSize":
                            if ((PortionSize < 0) || (PortionSize > 5000))
                            {
                                error = "Portion size cannot be less than 0 or greater than 5000.";
                            }
                            break;
                        case "CaloriesPerServing":
                            if ((CaloriesPerServing < 0) || (CaloriesPerServing > 15000))
                            {
                                error = "Calories per serving cannot be less than 0 or more than 15000.";
                            }
                            break;
                    }
                    return error;
                }
                catch
                {
                    return error;
                }
            }
        }
        public string Error
        {
            get { return null; }
        }
    }
}
