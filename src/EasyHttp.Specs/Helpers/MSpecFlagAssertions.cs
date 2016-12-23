using System;
using Machine.Specifications;
using Machine.Specifications.Utility.Internal;

namespace EasyHttp.Specs.Helpers
{
    public static class MSpecFlagAssertions
    {
        public static Enum ShouldHaveFlag(this Enum actual, Enum expected)
        {
            if (!actual.HasFlag(expected))
                throw new SpecificationException(PrettyPrintingExtensions.FormatErrorMessage((object)actual, (object)expected));
            return actual;
        }

        public static Enum ShouldNotHaveFlag(this Enum actual, Enum expected)
        {
            if (actual.HasFlag(expected))
                throw new SpecificationException(string.Format("Should not have {0} set but does: {1}", (object)expected, ((object)actual)));
            return actual;
        }
    }
}