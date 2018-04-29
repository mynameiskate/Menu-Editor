using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;

namespace DishesHierarchy
{
    [Serializable]
    public class Food : INotifyPropertyChanged
    {
        private float _weight;
        public float Weight
        {
            get
            {
                return _weight;
            }
            set
            {
                _weight = value;
                OnPropertyChanged("PortionSize");
            }
        }

        public struct Nutrients
        {
            float Protein { get; set; }
            float Fat { get; set; }
        }

        public float Calories { get; set;}

        public Food (float weight)
        {
            Weight = weight;
        }

        public Food()
        {

        }

        [field: NonSerialized]
        public event PropertyChangedEventHandler PropertyChanged;
        public void OnPropertyChanged([CallerMemberName]string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }
    }
}
