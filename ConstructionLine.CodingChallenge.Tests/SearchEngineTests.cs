using System;
using System.Collections;
using System.Collections.Generic;
using ConstructionLine.CodingChallenge.Tests.SampleData;
using NUnit.Framework;

namespace ConstructionLine.CodingChallenge.Tests
{
    [TestFixture]
    public class SearchEngineTests : SearchEngineTestsBase
    {
        [Test]
        public void Test()
        {
            // Arrange
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> {Color.Red}
            };

            // Act
            var results = searchEngine.Search(searchOptions);


            // Assert
            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [TestCase(0)]
        [TestCase(2)]
        [TestCase(5)]
        public void WhenDataContainsMultipleColoredItems_ThenSearchingForThatColor_ShouldReturnTheExpectedResults(int numberOfItems)
        {
            // Arrange
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Black - Medium", Size.Medium, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            for(var index = 0; index < numberOfItems; index++ )
                shirts.Add(new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red));

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Colors = new List<Color> {Color.Red}
            };

            // Act
            var results = searchEngine.Search(searchOptions);

            // Assert
            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [TestCase(0)]
        [TestCase(1)]
        [TestCase(2)]
        [TestCase(5)]
        public void WhenDataContainsSizedItems_ThenSearchingForThatSize_ShouldReturnTheExpectedResults(int numberOfSizedItems)
        {
            // Arrange
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Black - Small", Size.Small, Color.Black),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
            };

            for (var index = 0; index < numberOfSizedItems; index++)
                shirts.Add(new Shirt(Guid.NewGuid(), "Red - Medium", Size.Medium, Color.Red));

            var searchEngine = new SearchEngine(shirts);

            var searchOptions = new SearchOptions
            {
                Sizes = new List<Size> { Size.Medium }
            };

            // Act
            var results = searchEngine.Search(searchOptions);

            // Assert
            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        private static readonly object[] _colorsList =
        {
            new List<Color> { Color.Red, Color.Black },
            new List<Color> { Color.Red, Color.Black, Color.Yellow },
            new List<Color> { Color.White, Color.Yellow },
            new List<Color> { Color.Red, Color.White, Color.Yellow }
        };

        [Test, TestCaseSource(nameof(_colorsList))]
        public void SearchingForMultipleColors_ShouldReturnTheExpectedResults(List<Color> colors)
        {
            // Arrange
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Black - Small", Size.Small, Color.Black),
                new Shirt(Guid.NewGuid(), "White - Medium", Size.Medium, Color.White),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Yellow - Large", Size.Large, Color.Yellow),
            };
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions();
            searchOptions.Colors.AddRange(colors);

            // Act
            var results = searchEngine.Search(searchOptions);

            // Assert
            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        private static readonly object[] _sizeList =
        {
            new List<Size> {Size.Small, Size.Large},
            new List<Size> {Size.Medium, Size.Large},
            new List<Size> {Size.Small, Size.Medium},
            new List<Size> {Size.Small, Size.Medium, Size.Large}
        };

        [Test, TestCaseSource(nameof(_sizeList))]
        public void SearchingForMultipleSizes_ShouldReturnExpectedResults(List<Size> sizes)
        {
            // Arrange
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Yellow - Small", Size.Small, Color.Yellow),
                new Shirt(Guid.NewGuid(), "Blue - Medium", Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Yellow - Large", Size.Large, Color.Yellow),
                new Shirt(Guid.NewGuid(), "White - Large", Size.Large, Color.White),
                new Shirt(Guid.NewGuid(), "Red - Large", Size.Large, Color.Red),
            };
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions();
            searchOptions.Sizes.AddRange(sizes);

            // Act
            var results = searchEngine.Search(searchOptions);

            // Assert
            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }

        [Test]
        public void SearchingForMultipleSizesAndColors_ShouldReturnExpectedResults([ValueSource(nameof(_colorsList))]List<Color> colors,[ValueSource(nameof(_sizeList))]List<Size> sizes)
        {
            // Arrange
            var shirts = new List<Shirt>
            {
                new Shirt(Guid.NewGuid(), "Red - Small", Size.Small, Color.Red),
                new Shirt(Guid.NewGuid(), "Yellow - Small", Size.Small, Color.Yellow),
                new Shirt(Guid.NewGuid(), "Blue - Medium", Size.Medium, Color.Blue),
                new Shirt(Guid.NewGuid(), "Blue - Large", Size.Large, Color.Blue),
                new Shirt(Guid.NewGuid(), "Yellow - Large", Size.Large, Color.Yellow),
                new Shirt(Guid.NewGuid(), "White - Large", Size.Large, Color.White),
                new Shirt(Guid.NewGuid(), "Red - Large", Size.Large, Color.Red),
            };
            var searchEngine = new SearchEngine(shirts);
            var searchOptions = new SearchOptions();
            searchOptions.Colors.AddRange(colors);
            searchOptions.Sizes.AddRange(sizes);

            // Act
            var results = searchEngine.Search(searchOptions);

            // Assert
            AssertResults(results.Shirts, searchOptions);
            AssertSizeCounts(shirts, searchOptions, results.SizeCounts);
            AssertColorCounts(shirts, searchOptions, results.ColorCounts);
        }
    }
}