namespace CM
{
    using System.Collections.Generic;

    public interface IFizzBuzzSettings
    {
        long End { get; set; }
        long? Increment { get; set; }
        KeyValuePair<long, string>[] Pairs { get; set; }
        long Start { get; set; }

        string ToString();
    }
}