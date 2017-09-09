namespace CM
{
    public class FibonacciFizzBuzz : FizzBuzz
    {
        private long _last;

        public FibonacciFizzBuzz(IFizzBuzzSettings settings) : base(settings) { }
        public FibonacciFizzBuzz(IFizzBuzzSettings settings, ContinueWhile condition) : base(settings,condition) { }

        public override IFizzBuzzSettings SettingsChanged(IFizzBuzzSettings settings)
        {
            _last = settings.Start = 1L;
            return base.SettingsChanged(settings);
        }

        protected override long Increment(long lastIndex)
        {
            var res = _last;
            _last = lastIndex;
            return res;
        }
    }
}