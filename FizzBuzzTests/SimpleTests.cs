namespace FizzBuzzTests
{
    using System.Collections.Generic;
    using System.Linq;
    using CM;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class SimpleUnitTests
    {
        private static void TestFizzBuzzSequence( IReadOnlyList<string> actual)
        {
            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Count, 100, "Should default to 1..100 fizz=3, buzz=5 configuration.");
            Assert.AreEqual(actual[0], "1", "First value should be '1'.");
            Assert.AreEqual(actual[1], "2", "Second value should be '2'.");
            Assert.AreEqual(actual[2], "Fizz", "Third value should be 'Fizz'.");
            Assert.AreEqual(actual[3], "4", "Fourth value should be '4'.");
            Assert.AreEqual(actual[4], "Buzz", "Fifth value should be 'Buzz'.");
            Assert.AreEqual(actual[5], "Fizz", "Sixth value should be 'Fizz'.");
            Assert.AreEqual(actual[9], "Buzz", "Ninth value should be 'Buzz'.");
            Assert.AreEqual(actual[14], "FizzBuzz", "Fifteenth value should be 'FizzBuzz'.");
        }
        private static void TestFizzBuzzFooBarSequence(IReadOnlyList<string> actual, IFizzBuzzSettings settings)
        {
            Assert.IsNotNull(actual);
            long max = settings.Pairs.Aggregate<KeyValuePair<long, string>, long>(1, (current, kvp) => current * kvp.Key);
            Assert.AreEqual(actual.Count,max, $"For test should have a count of {max}");
            for (var index = 0; index < max; index++)
            {
                var comp = index + settings.Start;
                var pairs = settings.Pairs.Where(x => (comp % x.Key) == 0).Select(kvp => kvp.Value).ToArray();
                var expected = pairs.Any() ? $"{string.Join("", pairs)}" : $"{comp}";
                Assert.AreEqual( actual[(int)index], expected,
                    $"Expected: '{expected}', actual: '{actual[index]}'");
            }
        }


        [TestMethod]
        public void TestNullSettings()
        {
            var uut = new FizzBuzz();
            var actual = uut.Execute().ToArray();

            TestFizzBuzzSequence(actual);
        }

        [TestMethod]
        public void TestConstructor()
        {
            var settings = FizzBuzzSettings.Default;
            var actual = new FizzBuzz(settings).ToArray();

            TestFizzBuzzSequence(actual);
        }

        [TestMethod]
        public void TestMethodInvocation()
        {
            var settings = FizzBuzzSettings.Default;
            var actual = new FizzBuzz().Execute(settings).ToArray();

            TestFizzBuzzSequence(actual);
        }

        [TestMethod]
        public void TestInvertedRange()
        {
            var settings = FizzBuzzSettings.Default;
            settings.Start = 100;
            settings.End = 1;
            var actual = new FizzBuzz(settings).ToArray();

            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Length, 100, "Should be 100..1 fizz=3, buzz=5 configuration.");
            Assert.AreEqual(actual[99], "1", "First value should be '1'.");
            Assert.AreEqual(actual[99-1], "2", "Second value should be '2'.");
            Assert.AreEqual(actual[99-2], "Fizz", "Third value should be 'Fizz'.");
            Assert.AreEqual(actual[99-3], "4", "Fourth value should be '4'.");
            Assert.AreEqual(actual[99-4], "Buzz", "Fifth value should be 'Buzz'.");
            Assert.AreEqual(actual[99-5], "Fizz", "Sixth value should be 'Fizz'.");
            Assert.AreEqual(actual[99-9], "Buzz", "Ninth value should be 'Buzz'.");
            Assert.AreEqual(actual[99-14], "FizzBuzz", "Fifteenth value should be 'FizzBuzz'.");
        }

        [TestMethod]
        public void TestRangeWithoutKvp()
        {
            var settings = new FizzBuzzSettings { Start = 1, End = 100 };
            var actual = new FizzBuzz(settings).ToArray();

            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Length, 100);
            for( var i=0; i<actual.Length; )
            {
                Assert.AreEqual( actual[i], (++i).ToString(), $"Index {i-1} is '{actual[i-1]} should be {i}");
            }
        }
        [TestMethod]
        public void TestRangeWithManyKvp()
        {
            var kvp = new[]
            {
                new KeyValuePair<long, string>(3, "fizz"),
                new KeyValuePair<long, string>(5, "buzz"),
                new KeyValuePair<long, string>(7, "foo"),
                new KeyValuePair<long, string>(11, "bar")
            };

            var settings = new FizzBuzzSettings { Start = 1, End = 3*5*7*11, Pairs = kvp };
            var actual = new FizzBuzz(settings).ToArray();

            TestFizzBuzzFooBarSequence(actual,settings);
        }

        [TestMethod]
        public void TestConditionalIndex()
        {
            var settings = FizzBuzzSettings.Default;
            settings.End = 200;
            var actual = new FizzBuzzWhile(settings,(lastIndex,lastValue) => lastIndex <= 100).ToArray();

            TestFizzBuzzSequence(actual);
        }


        [TestMethod]
        public void TestConditionalValueWithManyKvp()
        {
            var kvp = new[]
            {
                new KeyValuePair<long, string>(3, "fizz"),
                new KeyValuePair<long, string>(5, "buzz"),
                new KeyValuePair<long, string>(7, "foo"),
                new KeyValuePair<long, string>(11, "bar")
            };

            // note, doubling 'End' to test condition
            var settings = new FizzBuzzSettings { Start = 1, End = 3 * 5 * 7 * 11 * 2, Pairs = kvp };
            var actual = new FizzBuzzWhile(settings, (lastIndex, lastValue) => lastValue != "fizzbuzzfoobar").ToArray();

            TestFizzBuzzFooBarSequence(actual, settings);
        }

        [TestMethod]
        public void TestFibonacci()
        {
            var settings = FizzBuzzSettings.Default;
            var actual = new FibonacciFizzBuzz(settings).ToArray();

            Assert.IsNotNull(actual);
            Assert.AreEqual(actual.Length,10);
            Assert.AreEqual(actual[0],"1");
            Assert.AreEqual(actual[1], "2");
            Assert.AreEqual(actual[2], "Fizz");
            Assert.AreEqual(actual[3], "Buzz");
            Assert.AreEqual(actual[4], "8");
            Assert.AreEqual(actual[5], "13");
            Assert.AreEqual(actual[7], "34");
            Assert.AreEqual(actual[9], "89");
        }
    }
}
