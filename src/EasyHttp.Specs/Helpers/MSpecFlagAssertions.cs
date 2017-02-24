using System;
using NUnit.Framework;

namespace EasyHttp.Specs.Helpers
{
    public static class MSpecFlagAssertions
    {
        public static Enum ShouldHaveFlag(this Enum actual, Enum expected)
        {
            if (!actual.HasFlag(expected))
                throw new AssertionException(string.Format("Should have had {0} set but did not: {1}", expected, actual));
            return actual;
        }

        public static Enum ShouldNotHaveFlag(this Enum actual, Enum expected)
        {
            if (actual.HasFlag(expected))
                throw new AssertionException(string.Format("Should not have {0} set but does: {1}", expected, actual));
            return actual;
        }
    }
}