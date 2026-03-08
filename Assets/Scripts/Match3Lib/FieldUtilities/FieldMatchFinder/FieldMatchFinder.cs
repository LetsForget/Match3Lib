using Match3Lib.Data;
using Match3Lib.FieldFiller;

namespace Match3Lib.FieldChecker
{
    public class FieldMatchFinder : IFieldMatchFinder
    {
        public FieldMatchesResult GetMatchedCells(Field field, FieldMatch[] horizontal, FieldMatch[] vertical)
        {
            var width = field.Width;
            var height = field.Height;

            var horIndex = 0;

            // HORIZONTAL
            for (var y = 0; y < height; y++)
            {
                var x = 0;

                while (x < width)
                {
                    var cell = field[x, y];

                    if (cell.Type != CellType.Default)
                    {
                        x++;
                        continue;
                    }

                    var element = cell.ElementNum;
                    var startX = x;
                    var length = 1;

                    while (x + length < width && field[x + length, y].ElementNum == element)
                    {
                        length++;
                    }

                    if (length >= 3)
                    {
                        var startCoord = new CellCoord(startX, y);
                        var endCoord = new CellCoord(startX + length - 1, y);
                        horizontal[horIndex++] = new FieldMatch(startCoord, endCoord, MatchType.Horizontal, length, cell.ElementNum);
                    }

                    x += length;
                }
            }

            var vertIndex = 0;

            // VERTICAL
            for (var x = 0; x < width; x++)
            {
                var y = 0;

                while (y < height)
                {
                    var cell = field[x, y];

                    if (cell.Type != CellType.Default)
                    {
                        y++;
                        continue;
                    }

                    var element = cell.ElementNum;
                    var startY = y;
                    var length = 1;

                    while (y + length < height && field[x, y + length].ElementNum == element)
                    {
                        length++;
                    }

                    if (length >= 3)
                    {
                        var startCoord = new CellCoord(x, startY);
                        var endCoord = new CellCoord(x, startY + length - 1);
                        vertical[vertIndex++] = new FieldMatch(startCoord, endCoord, MatchType.Vertical, length, cell.ElementNum);
                    }

                    y += length;
                }
            }

            return new FieldMatchesResult(vertical, vertIndex, horizontal, horIndex);
        }
    }
}