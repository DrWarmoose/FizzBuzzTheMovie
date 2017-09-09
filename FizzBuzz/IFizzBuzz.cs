namespace CM
{
    using System;
    using System.Collections.Generic;

    public delegate bool ContinueWhile(long lastIndex, string lastValue);

    public interface IFizzBuzz : IEnumerable<string>
    {
        long End { get; }
        bool IsAscending { get; }
        bool IsDescending { get; }
        IFizzBuzzSettings Settings { get; set; }
        long Start { get; }

        IEnumerable<string> Execute(IFizzBuzzSettings fizzBuzzSettings = null);
        IEnumerable<string> ExecuteWhile(ContinueWhile condition, IFizzBuzzSettings fizzBuzzSettings = null);
    }
}