namespace CM
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;

    public class FizzBuzz : IFizzBuzz
    {
        public const string UsageWarning = "usage: fizzBuzzSettings.Start and fizzBuzzSettings.End must be provided.";
        private IFizzBuzzSettings _settings;
        private readonly ContinueWhile _condition = null;

        public FizzBuzz(IFizzBuzzSettings settings = null)
        {
            Settings = settings;
        }
        public FizzBuzz(ContinueWhile condition)
        {
            _condition = condition;
        }
        public FizzBuzz(IFizzBuzzSettings settings, ContinueWhile condition)
        {
            Settings = settings;
            _condition = condition;
        }

        public long Start => _settings?.Start ?? 0L;
        public long End => _settings?.End ?? 0L;
        public bool IsDescending { get; private set; }
        public bool IsAscending => !IsDescending;

        public IFizzBuzzSettings Settings
        {
            get => _settings ?? (_settings = FizzBuzzSettings.Default);
            set
            {
                if (value != null)
                {
                    _settings = SettingsChanged(value);
                    IsDescending = Start > End;
                }
            }
        }

        public IEnumerator<string> GetEnumerator()
        {
            return (_condition == null ? Execute() : ExecuteWhile(_condition)).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return GetEnumerator();
        }

        public IEnumerable<string> Execute( IFizzBuzzSettings fizzBuzzSettings = null )
        {
            Settings = fizzBuzzSettings;
            return Shortcut() ?? IterativeExecute();
        }

        public IEnumerable<string> ExecuteWhile(ContinueWhile condition, IFizzBuzzSettings fizzBuzzSettings = null  )
        {
            Settings = fizzBuzzSettings ;
            return Shortcut(condition) ?? ConditionalExecute(condition);
        }

        public virtual IFizzBuzzSettings SettingsChanged(IFizzBuzzSettings settings)
        {
            return settings;
        }

        /// <summary>
        /// Contains validation and ability to recognize that a simple solution can be made
        /// rather than invoking the bigger methods
        /// </summary>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        private IEnumerable<string> Shortcut( ContinueWhile condition = null)
        {
            if (Settings == null) // TODO:  could remove this if Settings remains coded to default to FBS.Default
            {
                return new[] { UsageWarning };
            }
            if (Start == End )
            {
                return new[] { Start.ToString() };
            }
            if (Settings.Pairs == null || Settings.Pairs.Length == 0)
            {
                return (condition != null) ? EmptyConditionalExecute(condition) :
                        Range().Select(s => s.ToString());
            }
            return null;
        }

        /// <summary>
        /// One-line example of functional programming.
        /// Uses our Range command since Enumerable.Range only performs ascending
        /// </summary>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        private IEnumerable<string> IterativeExecute()
        {
            return from index in Range()
                let pairs = Settings.Pairs.Where(x => (index % x.Key) == 0).Select(kvp => kvp.Value)
                select pairs.Any() ? $"{string.Join("", pairs)}" : $"{index}";
        }

        /// <summary>
        /// Instead of running through a fore-known range, this method passes the last index
        /// and the last value generated so that it will continue to iterate until some formula
        /// that may use the last value and/or the last fizz/buzz string generated.
        /// </summary>
        /// <param name="condition">The condition delegate that accepts last index and last value and
        /// returns true if the function should continue, false if it should end now.</param>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
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

        /// <summary>
        /// Increments the specified last index.
        /// This method can be override to increment based on last value or some other criteria.
        /// </summary>
        /// <param name="lastIndex">The last index.</param>
        /// <returns>System.Int32.</returns>
        protected virtual long Increment( long lastIndex )
        {
            var absIncrement = Math.Abs(Settings.Increment.GetValueOrDefault(1));
            return IsDescending ? -absIncrement : absIncrement ;
        }

        /// <summary>
        /// If not using the condition method, then the Range method is called
        /// and uses the Increment method to move the returned index
        /// </summary>
        /// <returns>IEnumerable&lt;System.Int32&gt;.</returns>
        protected virtual IEnumerable<long> Range()
        {
            var index = Start;
            if ( IsDescending )
            {
                while (index >= End)
                {
                    yield return index;
                    index += Increment(index);
                }
            }
            else
            {
                while (index <= Settings.End)
                {
                    yield return index ;
                    index += Increment(index);
                }
            }
        }

    }
}