FizzBuzz usage:

**** See the unit tests for example usage. ****

FizzBuzz class is an enumeration of strings so the simplest way to invoke it is:

foreach( var item in new FizzBuzz() ) {...}

or use the Execute method:

foreach( var item in new FizzBuzz().Execute() ){...}

Originally, the FizzBuzz class required a FizzBuzzSettings configuration or it would spit out usage warning.
That didn't seem very friendly especially to those who don't read instructions.  (I'm  sure opinions differ)

So without any additional clues, FizzBuzz will now run the traditional 1..100 3=Fizz, 5=Buzz configuration.

To alter the Start, End or to alter or add additional pairs use the FizzBuzzSettings class which
provides properties to adjust any of these values.

if Start > End,  then FizzBuzz will automatically detect and decrement by the default amount of 1.
If the property Pairs is empty or null, then the range of numbers (as strings) from Start...End is returned.

Pairs will be evaluated in its provided order.   For example if 3=Fizz, 5=Buzz and if ordering is 3 first then 5 
in the pairs list, then 15 should equal FizzBuzz.   By reversing the order in the Pairs array, 15 = BuzzFizz.

If you are not satisfied with the increment value, then alter the Increment property of FizzBuzzSettings.

If you are still not satisfied with the increment value, then override the FizzBuzz Increment method which accepts
as an argument the previous value,  has access to the Settings property and can thus return any increment value.
NOTE: do not return the next value, rather the difference between the previous value and the desired value.

In some cases, altering the Increment method may be undesirable so the Enumerated Range method is also virtual
permitting a descendant class to modify its understanding of Range based on whatever criteria the derived class
wishes to use outside that which is provided by FizzBuzz itself.

This should satisfy the plain reading instruction of the Statement of Work.   And as a professional I would
stop there after satisfying those demands.

Yet as a hobbyist, I wasn't through.

While playing with this project, I noticed a bit of a deficiency, so to test the flexibility of the architecture
I created FizzBuzzWhile  which keeps the Start..End paradigm yet permits replacing the End property with a
conditional delegate.   For as long as the conditional (lastIndex, lastValue) returns true, the march through the Range
will continue.

To test the utility of the FizzBuzzWhile  I wanted to add 7=Foo and 11=Bar.  Instead of just calculating what index
all I would see FizzBuzzFooBar,  I implemented  the conditional until lastValue was satisfied.

Then to test expandibility,  I created the FibonacciFizzBuzz.   (Good thing the base class uses 64 bit longs)

For your amusment I have retained the version 2.0 FizzBuzzWhile and its sidekick FibonacciFizzBuzz
so that future developers can have a documented example of what work is required to expand FizzBuzz

I'm satisfied that the FizzBuzz class is maintainable.
