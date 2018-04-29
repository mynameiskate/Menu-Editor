using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace DishesHierarchy
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Menu = new DishList();
            dishGrid.DataContext = Menu;
            ingredientGrid.DataContext = Ingredients;

            var ingredients = new ObservableCollection<Ingredient>()
            {
              new DairyProduct("Cow milk", 0.6f, DairyProduct.Group.Milk, 250),
              new DairyProduct("Vanilla ice cream", 2.01f, DairyProduct.Group.IceCream, 100),
              new Fruit("Banana", 0.89f, "Yellow", 0.12f, 150),
              new Vegetable("Tomato", 0.3f, Vegetable.PungencyDegree.None, "Red", 150),
              new Meat("Chiken", 1f, Meat.MeatType.Chicken, 100),
              new Meat("Beef", 1f, Meat.MeatType.Beef, 100)
            };

            FullList = new IngredientList(ingredients);
            fullListGrid.ItemsSource = FullList.Ingredients;
        }

        public DishList Menu { get; private set; }
        public IngredientList Ingredients { get; set; }
        public IngredientList FullList { get; set; }
        private object _objSelected;

        private void dishGrid_MouseClick(object sender, SelectionChangedEventArgs e)
        {
           // DataGridRow row = ItemsControl.ContainerFromElement((DataGrid)sender, e.OriginalSource as DependencyObject) as DataGridRow;
            try
            {
                if (dishGrid.SelectedIndex != -1)
                {
                    Dish dish = (Dish)dishGrid.SelectedItem;
                    if (dish == null) return;
                    _objSelected = dish;
                    Ingredients = dish.Ingredients;
                    RefreshIngredientGrid(ingredientGrid, Ingredients);
                }
            }
            catch
            {
                dishGrid.UnselectAllCells();
                _objSelected = null;
                RefreshIngredientGrid(ingredientGrid, null);
            }
            finally
            {
                e.Handled = true;
            }        
        }

        public void RefreshIngredientGrid (DataGrid grid, IngredientList list)
        {
            grid.DataContext = typeof(IngredientList);
            grid.DataContext = list;
        }

        public void RefreshMenuGrid(DataGrid grid, DishList list)
        {
            grid.DataContext = typeof(DishList);
            grid.DataContext = list;
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            string msg = string.Empty;
            if (_objSelected == null)
            {
                msg = "Cannot delete the blank entry";
            }
            else
            {
                var result = MessageBox.Show(
                    "Delete selected row?\n",
                    "Delete",
                    MessageBoxButton.YesNo,
                    MessageBoxImage.Question,
                    MessageBoxResult.No);
                if (result == MessageBoxResult.Yes)
                {
                    msg = (Menu.RemoveItem(_objSelected)) ? "Record is successfully removed" : "Error!";
                }
                else msg = "Canceled";
            }
            
            RefreshMenuGrid(dishGrid, Menu);
            MessageBox.Show(msg);
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (dishComboBox.SelectedItem == null)
            {
                MessageBox.Show("Choose dish type", "Add item");
                return;
            }
            var type = dishComboBox.Text;
            Menu.AddItem(type);
            MessageBox.Show("New item is added");
        }

        private void btnSerialize_Click(object sender, RoutedEventArgs e)
        {
            string msg = (Serializator.XMLSerialize(Menu) && Serializator.BinarySerialize(Menu)) ? "Success" : "Error";
            MessageBox.Show(msg, "Serialization");
        }

        private void btnDeserialize_Click(object sender, RoutedEventArgs e)
        {
            if (comboBox.SelectedItem == null)
            {
                MessageBox.Show("Choose deserialization type", "Deserialization");
                return;
            }

            var type = comboBox.Text;
            DishList list;
            switch (type)
            {
                case "XML":
                    list = Serializator.BinaryDeserialize();
                    break;
                case "Binary":
                    list = Serializator.XMLDeserialize();
                    break;
                default: return;
            }

            if (list != null)
            {
                Menu = list;
                RefreshMenuGrid(dishGrid, Menu);
                MessageBox.Show("Successfully deserialized", "Deserialization");
            }
            else MessageBox.Show("Deserialization error", "Deserialization");
        }

        private void btnAddIngredient_Click(object sender, RoutedEventArgs e)
        {
            if (dishGrid.SelectedIndex != -1 && fullListGrid.SelectedIndex != -1)
            {
                Ingredient ingredient = (Ingredient)fullListGrid.SelectedItem;
                if (ingredient == null) return;
                Dish dish = (Dish)dishGrid.SelectedItem;
                if (dish == null) return;
                _objSelected = dish;
                dish.Ingredients.AddItem(ingredient);
                Ingredients = dish.Ingredients;
            }
            else MessageBox.Show("Pick an ingredient and a dish", "Adding ingredient");
        }

        private void btnRemoveIng_Click(object sender, RoutedEventArgs e)
        {
            if (dishGrid.SelectedIndex != -1 && ingredientGrid.SelectedIndex != -1)
            {
                Ingredient ingredient = (Ingredient)ingredientGrid.SelectedItem;
                if (ingredient == null) return;
                Dish dish = (Dish)dishGrid.SelectedItem;
                if (dish == null) return;
                _objSelected = dish;
                dish.Ingredients.RemoveItem(ingredient, ingredientGrid.SelectedIndex);
                Ingredients = dish.Ingredients;
            }
            else MessageBox.Show("Pick an ingredient and a dish", "Removing ingredient");
        }
    }
}
