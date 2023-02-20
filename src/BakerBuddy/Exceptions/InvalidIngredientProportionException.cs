namespace BakerBuddy.Exceptions;

public class InvalidIngredientProportionException: Exception
{
    public InvalidIngredientProportionException(string message)
        : base(message)
    {
    }
}
