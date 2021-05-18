namespace Atestat.Classes
{
    enum PasswordRequirements
    {
        None = 0b_0000_0000,        /// None
        Match = 0b_0000_0001,       /// The passwords match
        Length = 0b_0000_0010,      /// The password has at least 8 characters
        Digit = 0b_0000_0100,       /// The passwrod has at least one digit
        Capital = 0b_0000_1000,     /// The password has at least on capital letter
        All = 0b_0000_1111          /// All the requirements 
    }
}
