namespace BrewBuddy
{
    public class UserValidationException : Exception
    {
        public UserValidationException(string messsage): base(messsage) { }
    }
}
