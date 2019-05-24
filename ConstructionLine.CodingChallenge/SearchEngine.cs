using System.Collections.Generic;
using System.Linq;

namespace ConstructionLine.CodingChallenge
{
    public class SearchEngine
    {
        private readonly List<Shirt> _shirts;

        public SearchEngine(List<Shirt> shirts)
        {
            _shirts = shirts;
        }


        public SearchResults Search(SearchOptions options)
        {
            var queryResult = (from shirt in _shirts
                where (options.Colors.Contains(shirt.Color) || !options.Colors.Any())
                      && (options.Sizes.Contains(shirt.Size) || !options.Sizes.Any())
                        select shirt).ToList();

            var colorCount = new List<ColorCount>();
            foreach (var color in Color.All)
                colorCount.Add(new ColorCount {Color = color, Count = queryResult.Count(s => s.Color == color)});

            var sizeCount = new List<SizeCount>();
            foreach (var size in Size.All)
                sizeCount.Add(new SizeCount { Size = size, Count = queryResult.Count(s => s.Size == size) });

            return new SearchResults
            {
                Shirts = queryResult,
                SizeCounts = sizeCount,
                ColorCounts = colorCount
            };
        }
    }
}