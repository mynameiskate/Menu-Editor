using System;

namespace DishesHierarchy
{
    [Serializable]
    public class Grains : Ingredient
    {
        public string QualityGrade { get; set; }
        public GrainGroup Group { get; set; }

        public enum GrainGroup
        {
            Breads,
            Cereals,
            Grains,
            Pasta,
            Noodles
        }

        public Grains() {}
    }
}
