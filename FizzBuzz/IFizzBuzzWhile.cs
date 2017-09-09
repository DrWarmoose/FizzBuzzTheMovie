namespace CM
{
    using System.Collections.Generic;

    public delegate bool ContinueWhile(long lastIndex, string lastValue);

    public interface IFizzBuzzWhile : IFizzBuzz
    {
        IEnumerable<string> ExecuteWhile(ContinueWhile condition, IFizzBuzzSettings fizzBuzzSettings = null);
    }
}