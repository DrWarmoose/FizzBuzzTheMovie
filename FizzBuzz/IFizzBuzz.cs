namespace CM
{
    using System.Collections.Generic;

    public interface IFizzBuzz : IEnumerable<string>
    {
        long End { get; }
        bool IsAscending { get; }
        bool IsDescending { get; }
        IFizzBuzzSettings Settings { get; set; }
        long Start { get; }

        IEnumerable<string> Execute(IFizzBuzzSettings fizzBuzzSettings = null);
    }
}