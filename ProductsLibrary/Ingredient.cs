using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DishesHierarchy
{
    [Serializable]
    public class Ingredient : Food
    {
        public float CaloriesPerGram
        { 
            get
            {
                return Calories;
            }

            set
            {
                Calories = value;
                OnPropertyChanged("CaloriesPerGram");
            } 
        }

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

        public Ingredient(string name, float calories, float weight) : base (weight)
        {
            CaloriesPerGram = calories;
            Name = name;
        }

        public Ingredient()
        {

        }
    }
}
