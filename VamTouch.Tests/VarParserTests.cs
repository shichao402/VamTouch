using System.IO;
using System.IO.Compression;
using System.Text;
using System.Threading.Tasks;
using VamTouch.Core.Models;
using VamTouch.Core.Services;
using Xunit;

namespace VamTouch.Tests
{
    public class VarParserTests
    {
        [Fact]
        public async Task DetermineCategory_ShouldDetectSceneCategory()
        {
            // Arrange
            var parser = new VarParser();
            var meta = new VamMeta
            {
                ContentList = new System.Collections.Generic.List<string>
                {
                    "Saves/scene/test_scene.json",
                    "Custom/Atom/Person/Clothing/test.vap"
                }
            };

            // Act
            var category = parser.DetermineCategory(meta);

            // Assert
            Assert.Equal(CategoryType.Scenes, category);
        }

        [Fact]
        public async Task DetermineCategory_ShouldDetectAppearanceCategory()
        {
            // Arrange
            var parser = new VarParser();
            var meta = new VamMeta
            {
                ContentList = new System.Collections.Generic.List<string>
                {
                    "Custom/Atom/Person/Appearance/appearance.vap",
                    "Custom/Atom/Person/Clothing/test.vap"
                }
            };

            // Act
            var category = parser.DetermineCategory(meta);

            // Assert
            Assert.Equal(CategoryType.Appearance, category);
        }

        [Fact]
        public async Task DetermineCategory_ShouldDetectPluginCategory()
        {
            // Arrange
            var parser = new VarParser();
            var meta = new VamMeta
            {
                ContentList = new System.Collections.Generic.List<string>
                {
                    "Custom/Scripts/test.cs",
                    "Custom/Atom/Person/Clothing/test.vap"
                }
            };

            // Act
            var category = parser.DetermineCategory(meta);

            // Assert
            Assert.Equal(CategoryType.Plugin, category);
        }

        [Fact]
        public async Task DetermineCategory_ShouldReturnNoneForEmptyContent()
        {
            // Arrange
            var parser = new VarParser();
            var meta = new VamMeta
            {
                ContentList = new System.Collections.Generic.List<string>()
            };

            // Act
            var category = parser.DetermineCategory(meta);

            // Assert
            Assert.Equal(CategoryType.None, category);
        }

        [Fact]
        public async Task DetermineCategory_ShouldReturnNoneForNull()
        {
            // Arrange
            var parser = new VarParser();
            VamMeta meta = null;

            // Act
            var category = parser.DetermineCategory(meta);

            // Assert
            Assert.Equal(CategoryType.None, category);
        }
    }
} 