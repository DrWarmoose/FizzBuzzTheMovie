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

        public FizzBuzz(IFizzBuzzSettings settings = null)
        {
            Settings = settings;
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

        public virtual IEnumerator<string> GetEnumerator()
        {
            return Execute().GetEnumerator();
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

        public virtual IFizzBuzzSettings SettingsChanged(IFizzBuzzSettings settings)
        {
            return settings;
        }

        /// <summary>
        /// Contains validation and ability to recognize that a simple solution can be made
        /// rather than invoking the bigger methods
        /// </summary>
        /// <returns>IEnumerable&lt;System.String&gt;.</returns>
        protected IEnumerable<string> Shortcut()
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
                return Range().Select(s => s.ToString());
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