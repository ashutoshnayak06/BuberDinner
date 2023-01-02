using ErrorOr;

namespace BuberDinner.Domain.Common.Errors;

public static partial class Errors
{
    public static class User
    {
        public static Error DuplivateEmail=>Error.Conflict(code:"User.DeplicateEmail",description:"Email already in use.");
    }
}