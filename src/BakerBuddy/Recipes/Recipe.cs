using BakerBuddy.Exceptions;

namespace BakerBuddy.Recipes
{
    public class Recipe
    {
        public ulong UserId { get; }
        public string Name { get; }
        public string Description { get; }

        public IReadOnlyCollection<Ingredient> Ingredients { get; }

        public float? HydrationLevel { get; set; }

        public static Recipe Make(
            ulong userId,
            string name,
            string description,
            IReadOnlyCollection<Ingredient> ingredients)
        {

            return new Recipe(
                userId,
                name,
                description,
                ingredients);
        }

        private Recipe(ulong userId,
                             string name,
                             string description,
                             IReadOnlyCollection<Ingredient> ingredients,
                             float? hydrationLevel = null)
        {
            UserId = userId;
            Name = name;
            Description = description;
            Ingredients = ingredients;
            HydrationLevel = hydrationLevel;
        }

        public Recipe WithHydrationLevel()
        {
            int flourWeight = GetIngredientsWeightByType(IngredientType.Flour);
            var waterWeight = GetIngredientsWeightByType(IngredientType.Water);

            ValidateRecipe();

            return new Recipe(
                UserId,
                Name,
                Description,
                Ingredients,
                CalculateHydrationLevel());

            float CalculateHydrationLevel()
            {
                return 100 * (waterWeight / flourWeight);
            }

            void ValidateRecipe()
            {
                if (flourWeight <= 0)
                    throw new InvalidIngredientProportionException("Flour ingredient should be present in the recipe.");

                if (waterWeight <= 0)
                    throw new InvalidIngredientProportionException("Water ingredient should be present in the recipe.");
            }
        }

        public Recipe WithBakerPercentage()
        {
            int flourWeight = GetIngredientsWeightByType(IngredientType.Flour);

            ValidateRecipe();

            return new Recipe(
                UserId,
                Name,
                Description,
                Ingredients.Select(i => i.WithBakerPercentage(flourWeight)).ToArray(),
                HydrationLevel
            );

            void ValidateRecipe()
            {
                if (flourWeight <= 0)
                    throw new InvalidIngredientProportionException("Flour ingredient should be present in the recipe.");
            }
        }

        private int GetIngredientsWeightByType(IngredientType type)
        {
            return Ingredients.Where(i => i.IngredientType == type).Sum(i => i.Gramms);
        }
    }
}
