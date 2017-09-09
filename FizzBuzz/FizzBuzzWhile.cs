
namespace CM
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class FizzBuzzWhile  : FizzBuzz, IFizzBuzzWhile
    {
        private readonly ContinueWhile _condition = null;

        public FizzBuzzWhile(IFizzBuzzSettings settings = null) : base(settings) { }
        public FizzBuzzWhile(ContinueWhile condition) : this(null,condition) { }
        public FizzBuzzWhile(IFizzBuzzSettings settings, ContinueWhile condition) : base(settings)
        {
            _condition = condition;
        }

        public override IEnumerator<string> GetEnumerator()
        {
            return (_condition == null ? Execute() : ExecuteWhile(_condition)).GetEnumerator();
        }

        public IEnumerable<string> ExecuteWhile(ContinueWhile condition, IFizzBuzzSettings fizzBuzzSettings = null)
        {
            Settings = fizzBuzzSettings;
            return Shortcut() ?? Shortcut(condition) ?? ConditionalExecute(condition);
        }

        protected IEnumerable<string> Shortcut(ContinueWhile condition)
        {
            if (Settings.Pairs == null || Settings.Pairs.Length == 0)
            {
                return (condition != null) ? EmptyConditionalExecute(condition) :
                    Range().Select(s => s.ToString());
            }
            return null;
        }

        /// <summary>
        /// Instead of running through a fore-known range, this method passes the last index
        /// and the last value generated so that it will continue to iterate until some formula
        /// that may use the last value and/or the last fizz/buzz string generated.
        /// </summary>
        /// <param name="condition">The condition delegate that accepts last index and last value and
        /// returns true if the function should continue, false if it should end now.</param>
        /// <returns>IEnumerable System.String.</returns>
        private IEnumerable<string> ConditionalExecute(ContinueWhile condition)
        {
            var index = Settings.Start;
            var lastValue = string.Empty;
            while (condition(index, lastValue))
            {
                var pairs = Settings.Pairs.Where(x => (index % x.Key) == 0).Select(kvp => kvp.Value).ToArray();
                lastValue = pairs.Any() ? $"{string.Join("", pairs)}" : $"{index}";
                index += Increment(index);
                yield return lastValue;
            }
        }

        /// <summary>
        /// Exercises the condition method.
        /// </summary>
        /// <param name="condition">The condition.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        private IEnumerable<string> EmptyConditionalExecute(ContinueWhile condition)
        {
            for (var index = Start; condition(index, index.ToString());)
                yield return index.ToString();
        }
    }
}
