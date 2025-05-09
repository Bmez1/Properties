using Crosscutting;

namespace Properties.Domain.Errors
{
    public static class OwnerError
    {
        public static Error NotFoundById => Error.NotFound(
        "Owner.NotFound",
        "There is no Owner with the given id");
    }
}
