using System;
using CrystalImageHelper;
using NUnit.Framework;

namespace CrystalImageHelper.Tests
{
    [TestFixture]
    public class RouteParserTests
    {
        [Test]
        public void IsSafeFileName_ReturnsFalse_WhenNull()
        {
            Assert.IsFalse(RouteParser.IsSafeFileName(null));
        }

        [Test]
        public void IsSafeFileName_ReturnsFalse_WhenEmpty()
        {
            Assert.IsFalse(RouteParser.IsSafeFileName(string.Empty));
        }

        [Test]
        public void IsSafeFileName_ReturnsFalse_WhenWhitespace()
        {
            Assert.IsFalse(RouteParser.IsSafeFileName("   "));
        }

        [Test]
        public void IsSafeFileName_ReturnsFalse_WhenPathTraversalPresent()
        {
            Assert.IsFalse(RouteParser.IsSafeFileName("../secret.png"));
            Assert.IsFalse(RouteParser.IsSafeFileName("folder/file.png"));
        }

        [Test]
        public void IsSafeFileName_ReturnsTrue_ForSimpleFileName()
        {
            Assert.IsTrue(RouteParser.IsSafeFileName("logo.png"));
            Assert.IsTrue(RouteParser.IsSafeFileName("product-123.jpg"));
        }

        [Test]
        public void TryParse_ReturnsNull_WhenRequestIsNull()
        {
            Assert.IsNull(RouteParser.TryParse(null));
        }
    }
}
