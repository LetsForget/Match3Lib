using System.Collections.Generic;
using Match3Lib.Data;
using Match3Lib.FieldFiller;

namespace Match3Lib.FieldChecker
{
    public class FieldMatchFinder : IFieldMatchFinder
    {
        public IEnumerable<Match> GetMatchedCells(Field field)
        {
            for (var y = 0; y < field.Height; y++)
            {
                var x = 0;

                while (x < field.Width)
                {
                    var startX = x;
                    var cell = field[x, y];

                    if (cell.Type != CellType.Default)
                    {
                        x++;
                        continue;
                    }

                    var elementNum = cell.ElementNum;
                    var length = 1;

                    for (var i = x + 1; i < field.Width; i++)
                    {
                        if (field[i, y].ElementNum == elementNum)
                        {
                            length++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (length >= 3)
                    {
                        var start = new CellCoord(startX, y);
                        var end = new CellCoord(startX + length - 1, y);

                        yield return new Match(start, end, MatchType.Horizontal, length);
                    }

                    x += length > 1 ? length : 1;
                }
            }

            for (var x = 0; x < field.Width; x++)
            {
                var y = 0;

                while (y < field.Height)
                {
                    var startY = y;
                    
                    var cell = field[x, y];
                    if (cell.Type != CellType.Default)
                    {
                        x++;
                        continue;
                    }

                    var elementNum = cell.ElementNum;

                    var length = 1;

                    for (var i = y + 1; i < field.Height; i++)
                    {
                        if (field[x, i].ElementNum == elementNum)
                        {
                            length++;
                        }
                        else
                        {
                            break;
                        }
                    }

                    if (length >= 3)
                    {
                        var start = new CellCoord(x, startY);
                        var end = new CellCoord(x, startY + length - 1);

                        yield return new Match(start, end, MatchType.Vertical, length);
                    }

                    y += length > 1 ? length : 1;
                }
            }
        }
    }
}
