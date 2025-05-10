using Crosscutting;

namespace Properties.Domain.Errors
{
    public static class PropertyError
    {
        public static Error NotFoundById => Error.NotFound(
        "Property.NotFound",
        "There is no Property with the given id");
    }
}
