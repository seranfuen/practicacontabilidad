using FluentAssertions;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using NUnit.Framework;
using PracticaContabilidad.Helpers;

namespace PracticaContabilidadTests
{
    [TestFixture]
    public class ShortToLongAccountCodeTests
    {
        [TestCase("420", ExpectedResult = "420000000")]
        [TestCase("420.0", ExpectedResult = "420000000")]
        [TestCase("420.1", ExpectedResult = "420000001")]
        [TestCase("420.", ExpectedResult = "420000000")]
        [TestCase("420.500", ExpectedResult = "420000500")]
        [TestCase("420000010", ExpectedResult = "420000010")]
        public string Test_normal_cases(string input)
        {
            var entityUnderTest = new ShortToLongAccountCode(input);
            return entityUnderTest.LongCode;
        }

        [TestCase("4200000000000", Reason = "Longer than 9 characters")]
        [TestCase("420.1.2", Reason = "Two separators")]
        [TestCase(".1", Reason = "Starts with separator")]
        [TestCase("", Reason = "Is empty or null string")]
        [TestCase("420000A", Reason = "Contains non numerical characters")]

        public void Test_non_acceptable_tests(string input)
        {
            var entityUnderTest = new ShortToLongAccountCode(input);
            entityUnderTest.IsCorrectCode.Should().BeFalse();
            entityUnderTest.LongCode.Should().BeNull();
        }
    }
}
