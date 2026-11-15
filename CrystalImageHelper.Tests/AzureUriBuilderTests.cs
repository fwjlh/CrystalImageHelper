using System;
using CrystalImageHelper;
using NUnit.Framework;

namespace CrystalImageHelper.Tests
{
    [TestFixture]
    public class AzureUriBuilderTests
    {
        [Test]
        public void Build_Throws_WhenConfigIsNull()
        {
            Assert.Throws<ArgumentNullException>(() => AzureUriBuilder.Build(null, "file.png"));
        }

        [Test]
        public void Build_Throws_WhenFileNameIsNull()
        {
            SourceConfig config = new SourceConfig("C:\\x", "https://example.com", string.Empty, true);
            Assert.Throws<ArgumentException>(() => AzureUriBuilder.Build(config, null));
        }

        [Test]
        public void Build_ProducesCorrectUri_WithoutSas()
        {
            SourceConfig config = new SourceConfig(
                "C:\\Images",
                "https://account.blob.core.windows.net/products",
                string.Empty,
                true);

            Uri uri = AzureUriBuilder.Build(config, "logo.png");

            Assert.AreEqual("https://account.blob.core.windows.net/products/logo.png", uri.AbsoluteUri);
        }

        [Test]
        public void Build_AppendsSasToken_WhenPresent()
        {
            SourceConfig config = new SourceConfig(
                "C:\\Images",
                "https://account.blob.core.windows.net/products",
                "sv=2024-11-04&sig=abc",
                true);

            Uri uri = AzureUriBuilder.Build(config, "logo.png");

            Assert.AreEqual(
                "https://account.blob.core.windows.net/products/logo.png?sv=2024-11-04&sig=abc",
                uri.AbsoluteUri);
        }

        [Test]
        public void Build_EncodesFileName()
        {
            SourceConfig config = new SourceConfig(
                "C:\\Images",
                "https://account.blob.core.windows.net/products",
                string.Empty,
                true);

            Uri uri = AzureUriBuilder.Build(config, "my logo.png");

            Assert.IsTrue(uri.AbsoluteUri.Contains("my%20logo.png"));
        }
    }
}
