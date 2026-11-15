using CrystalImageHelper;
using NUnit.Framework;

namespace CrystalImageHelper.Tests
{
    [TestFixture]
    public class SourceConfigLoaderTests
    {
        [Test]
        public void ParseRaw_ReturnsNull_WhenRawIsNull()
        {
            Assert.IsNull(SourceConfigLoader.ParseRaw(null));
        }

        [Test]
        public void ParseRaw_ReturnsNull_WhenRawIsEmpty()
        {
            Assert.IsNull(SourceConfigLoader.ParseRaw(string.Empty));
        }

        [Test]
        public void ParseRaw_ReturnsNull_WhenFewerThanTwoParts()
        {
            Assert.IsNull(SourceConfigLoader.ParseRaw("C:\\Images"));
        }

        [Test]
        public void ParseRaw_ParsesMinimalValidConfig()
        {
            SourceConfig config = SourceConfigLoader.ParseRaw(
                "C:\\ReportImages\\Products|https://account.blob.core.windows.net/products");

            Assert.IsNotNull(config);
            Assert.AreEqual("C:\\ReportImages\\Products", config.LocalFolder);
            Assert.AreEqual("https://account.blob.core.windows.net/products", config.AzureBaseUrl);
            Assert.AreEqual(string.Empty, config.AzureSasToken);
            Assert.IsTrue(config.NormalizeForCrystal);
        }

        [Test]
        public void ParseRaw_ParsesFullConfig_WithNormalizeFalse()
        {
            SourceConfig config = SourceConfigLoader.ParseRaw(
                "C:\\Logos|https://account.blob.core.windows.net/logos|sv=token|false");

            Assert.IsNotNull(config);
            Assert.AreEqual("C:\\Logos", config.LocalFolder);
            Assert.AreEqual("https://account.blob.core.windows.net/logos", config.AzureBaseUrl);
            Assert.AreEqual("sv=token", config.AzureSasToken);
            Assert.IsFalse(config.NormalizeForCrystal);
        }

        [Test]
        public void ParseRaw_DefaultsNormalizeToTrue_WhenFourthPartMissing()
        {
            SourceConfig config = SourceConfigLoader.ParseRaw(
                "C:\\Logos|https://account.blob.core.windows.net/logos|sv=token");

            Assert.IsNotNull(config);
            Assert.IsTrue(config.NormalizeForCrystal);
        }
    }
}
