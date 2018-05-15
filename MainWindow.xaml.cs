using Microsoft.Win32;
using System;
using System.Collections.ObjectModel;
using PluginInterfaces;
using System.Reflection;
using System.Windows;
using System.Windows.Controls;
using System.Collections.Generic;
using System.IO;

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
            Menu.FillList();
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
            pluginCheckBox.DataContext = this;
            fullListGrid.ItemsSource = FullList.Ingredients;
            FillPluginList();
        }

        public bool UsePlugins { get; set; }
     
        public IPlugin ChosenPlugin { get; private set; }
        public List<IPlugin> PluginList { get; private set; }
        public DishList Menu { get; private set; }
        public IngredientList Ingredients { get; set; }
        public IngredientList FullList { get; set; }
        private object _objSelected;
        private const string _pluginDir = "Plugins";

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

        public void FillPluginList()
        {
            if (!Directory.Exists(_pluginDir))
                return;
            /*OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Title = "Open plugin directory";
            fileDlg.ShowDialog();
            if (string.IsNullOrEmpty(fileDlg.FileName)) return;
            string path = Path.GetDirectoryName(fileDlg.FileName); */
            string[] dllFileNames = null;
            dllFileNames = Directory.GetFiles(_pluginDir, "*.dll");
            ICollection<Assembly> assemblies = new List<Assembly>(dllFileNames.Length);
            foreach (string dllFile in dllFileNames)
            {
                AssemblyName an = AssemblyName.GetAssemblyName(dllFile);
                Assembly assembly = Assembly.Load(an);
                assemblies.Add(assembly);
            }

            ICollection<Type> pluginTypes = new List<Type>();
            foreach (Assembly assembly in assemblies)
            {
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.IsInterface || type.IsAbstract)
                        {
                            continue;
                        }
                        else
                        {
                            if (type.GetInterface("IPlugin") != null)
                            {
                                pluginTypes.Add(type);
                            }
                        }
                    }
                }
            }

            PluginList = new List<IPlugin>(pluginTypes.Count);
            foreach (Type type in pluginTypes)
            {
                IPlugin plugin = (IPlugin)Activator.CreateInstance(type);
                PluginList.Add(plugin);
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
            if (comboBox.SelectedItem == null)
            {
                MessageBox.Show("Choose serialization type", "Serialization");
                return;
            }
            var type = comboBox.Text;
            string path, msg = string.Empty;
            switch (type)
            {
                case "XML":
                    path = Serializator.XMLSerialize(Menu);
                    break;
                case "Binary":
                    path = Serializator.BinarySerialize(Menu);
                    break;
                default: return;
            }
            msg = "Successfully serialized.";
            if (path == null)
            {
                msg = "Serialization error";
            }
            else if (ChosenPlugin != null && UsePlugins)
            {
                try
                {
                    ChosenPlugin.Compress(path);
                    msg = "Successfully serialized and compressed";
                }
                catch (Exception ex)
                { 
                    msg = "Compression error: "+ ex.Message;
                }
            }
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
                    list = Serializator.XMLDeserialize();
                    break;
                case "Binary":
                    list = Serializator.BinaryDeserialize();
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

        private void btnPickPlugin_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "Dll files|*.dll";
            if (fileDlg.ShowDialog() == true)
            {
                if (fileDlg.FileName == null) return;
                AssemblyName name = AssemblyName.GetAssemblyName(fileDlg.FileName);
                Assembly assembly = Assembly.Load(name);
                if (assembly != null)
                {
                    Type[] types = assembly.GetTypes();
                    foreach (Type type in types)
                    {
                        if (type.GetInterface("IPlugin") != null)
                        {
                            ChosenPlugin = Activator.CreateInstance(type) as IPlugin;
                            pluginLbl.Content = ChosenPlugin.ArchiveType;
                        }
                    }
                }
            }
        }

        private void btnOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fileDlg = new OpenFileDialog();
            fileDlg.Filter = "Archive files|*.zip;*.gz";
            fileDlg.ShowDialog();
            string path = fileDlg.FileName;
            if (string.IsNullOrEmpty(path)) return;
            if (PluginList.Count == 0)
            {
                var result = MessageBox.Show("Plugins cannot be found. Please choose plugin directory.", "Error", MessageBoxButton.YesNo);
                if (result != MessageBoxResult.Yes) return;
                FillPluginList();
            }
            else
            {
                string msg = string.Empty;
                foreach(IPlugin plugin in PluginList)
                {
                    if (plugin.ArchiveType == Path.GetExtension(path))
                    {
                        try
                        {
                            string resPath = plugin.Decompress(path);
                            path = Path.GetFullPath(resPath);
                            var ext = Path.GetExtension(path);
                            DishList list = null;
                            switch (ext)
                            {
                                case ".dat":
                                    list = Serializator.BinaryDeserialize();
                                    break;
                                case ".xml":
                                    list = Serializator.XMLDeserialize();
                                    break;
                            }

                            if (list != null)
                            {
                                Menu = list;
                                RefreshMenuGrid(dishGrid, Menu);
                                msg = "Successfully decompressed.";
                            }
                            else
                            {
                                msg = "Deserialization error";
                            }

                        }
                        catch(Exception ex)
                        {
                            msg = "File cannot be decompressed: " + ex.Message;
                        }
                    }
                }
                MessageBox.Show(msg, "Decompression");
            }
        }

        private void PluginCheckBox_Checked(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
