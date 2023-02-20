using BakerBuddy.Exceptions;

namespace BakerBuddy.Recipes
{
    public class Ingredient
    {
        public string Name { get; }
        public int Gramms { get; }
        public IngredientType IngredientType { get; }
        public float? BakerPercentage { get; }

        public static Ingredient Make(
            string name,
            int gramms,
            IngredientType ingredientType
        )
        {
            return new Ingredient(
                name,
                gramms,
                ingredientType
            );
        }

        private Ingredient(string name, int gramms, IngredientType ingredientType, float? bakerPercentage = null)
        {
            Name = name;
            Gramms = gramms;
            IngredientType = ingredientType;
            BakerPercentage = bakerPercentage;
        }

        public Ingredient WithBakerPercentage(int flourGramms)
        {
            if (flourGramms <= 0)
                throw new InvalidIngredientProportionException("Flour ingredient is required.");

            return new Ingredient(
                Name,
                Gramms,
                IngredientType,
                CalculateBakersPercentage(flourGramms)
            );

            float CalculateBakersPercentage(int flourGramms) => 100 * (Gramms / flourGramms);
        }
    }
}
