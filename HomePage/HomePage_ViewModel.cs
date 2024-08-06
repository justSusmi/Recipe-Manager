using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace Recipte_Management.HomePage
{
    public class HomePage_ViewModel : INotifyPropertyChanged
    {
        private Recipe_Model _selectedRecipe;

        public ObservableCollection<Recipe_Model> Recipes { get; set; }
        Database_Helper database_Helper = new Database_Helper();

        public HomePage_ViewModel()
        {
            Recipes = new ObservableCollection<Recipe_Model>(database_Helper.GetRecipes());

            foreach (var recipe in Recipes)
            {
                Console.Write(recipe.Name);
                foreach (var ingridient in recipe.Ingredients)
                {
                    Console.WriteLine("Name " + ingridient.Name);
                    Console.WriteLine("Amount " + ingridient.Amount);
                }
            }

        }

        public Recipe_Model SelectedRecipe
        {
            get => _selectedRecipe;
            set
            {
                if (_selectedRecipe != value)
                {
                    _selectedRecipe = value;
                    OnPropertyChanged();
                    UpdateVisibility();
                    UpdateFormattedInstructions();
                }
            }
        }

        public Visibility RecipeSelectedVisibility { get; private set; } = Visibility.Collapsed;
        public Visibility NoRecipeSelectedVisibility { get; private set; } = Visibility.Visible;

        private void UpdateVisibility()
        {
            RecipeSelectedVisibility = SelectedRecipe != null ? Visibility.Visible : Visibility.Collapsed;
            NoRecipeSelectedVisibility = SelectedRecipe == null ? Visibility.Visible : Visibility.Collapsed;

            OnPropertyChanged(nameof(RecipeSelectedVisibility));
            OnPropertyChanged(nameof(NoRecipeSelectedVisibility));
        }

        private ObservableCollection<string> _formattedInstructions;
        public ObservableCollection<string> FormattedInstructions
        {
            get => _formattedInstructions;
            set
            {
                if (_formattedInstructions != value)
                {
                    _formattedInstructions = value;
                    OnPropertyChanged();
                }
            }
        }

        private void UpdateFormattedInstructions()
        {
            if (SelectedRecipe != null)
            {
                FormattedInstructions = FormatInstructions(SelectedRecipe.Instructions);
            }
            else
            {
                FormattedInstructions = new ObservableCollection<string>();
            }
        }

        private ObservableCollection<string> FormatInstructions(string instructions)
        {
            var collection = new ObservableCollection<string>();

            if (string.IsNullOrEmpty(instructions))
                return collection;

            var formattedText = Regex.Replace(instructions, @"(\d+)\.", "\n$1");

            var lines = formattedText.Split('\n');

            foreach (var line in lines)
            {
                if (!string.IsNullOrWhiteSpace(line))
                {
                    string pattern = @"^.{2}";
                    var newLine = Regex.Replace(line, pattern, string.Empty);

                    collection.Add(newLine.Trim());
                }
            }

            return collection;
        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
