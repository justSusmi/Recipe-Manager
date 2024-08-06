using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MySql.Data.MySqlClient;
using Newtonsoft.Json;

namespace Recipte_Management.HomePage
{
    public class Ingredient
    {
        public string Name { get; set; }
        public string Amount { get; set; }
    }
    public class Recipe_Model
    {
        public int RecipeId { get; set; } 
        public string Name { get; set; }
        public List<Ingredient> Ingredients { get; set; }
        public string Instructions { get; set; }
        public string Description { get; set; }
    }

    public class Database_Helper
    {
        private readonly string connectionString = "Server=localhost;Port=3306;Database=recipes;Uid=root;Pwd=Passw0rt;";

        public MySqlConnection GetConnection()
        {
            MySqlConnection connection = new MySqlConnection(connectionString);
            return connection;
        }
        public List<Recipe_Model> GetRecipes()
        {
            var recipes = new List<Recipe_Model>();

            using (var connection = GetConnection())
            {
                connection.Open();
                string query = "SELECT recipe_id, name, ingredients, instructions, description FROM recipes;";
                using (var cmd = new MySqlCommand(query, connection))
                {
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string ingredientsJson = reader.GetString("ingredients");
                            List<Ingredient> ingredients = new List<Ingredient>();

                            try
                            {
                                ingredients = JsonConvert.DeserializeObject<List<Ingredient>>(ingredientsJson);
                            }
                            catch (JsonException ex)
                            {
                                // Log or handle JSON parsing errors
                                Console.WriteLine($"JSON Parsing Error: {ex.Message}");
                            }

                            var recipe = new Recipe_Model
                            {
                                RecipeId = reader.GetInt32("recipe_id"),
                                Name = reader.GetString("name"),
                                Ingredients = ingredients,
                                Instructions = reader.GetString("instructions"),
                                Description = reader.IsDBNull(reader.GetOrdinal("description")) ? null : reader.GetString("description")
                            };
                            recipes.Add(recipe);
                        }
                    }
                }
            }

            return recipes;
        }

    }
}
