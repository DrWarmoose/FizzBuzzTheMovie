namespace CM
{
    using System.Collections.Generic;

    public class FizzBuzzSettings : IFizzBuzzSettings
    {
        public long Start { get; set; }
        public long End { get; set; }
        public long? Increment { get; set; }
        public KeyValuePair<long,string> [] Pairs { get; set; }

        public override string ToString()
        {
            var direction = Start < End ? "incrementing" : "decrementing";
            return $"{Start} to {End} {direction} by {Increment.GetValueOrDefault(1)}";
        }

        public static FizzBuzzSettings Default = new FizzBuzzSettings
        {
            Start = 1, End = 100, Increment = 1, 
            Pairs = new[]
            {
                new KeyValuePair<long,string>(3,"Fizz"),
                new KeyValuePair<long,string>(5,"Buzz")
            }
        };
    }
}
