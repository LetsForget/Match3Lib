using System;
using System.Collections.Generic;
using Match3Lib.Data;

namespace Match3Lib.FieldFiller
{
    public class PureRandomFieldMover : IFieldMover
    {
        private readonly Random random;

        public PureRandomFieldMover() : this(new Random()) { }

        public PureRandomFieldMover(Random random)
        {
            this.random = random ?? throw new ArgumentNullException(nameof(random));
        }

        public IEnumerable<FieldMove> GetFieldMoves(Field field)
        {
            for (var x = 0; x < field.Width; x++)
            {
                var writeY = field.Height - 1;

                for (var y = field.Height - 1; y >= 0; y--)
                {
                    var cell = field[x, y];
                    var elementNum = cell.ElementNum;

                    if (elementNum < 0 && cell.Type == CellType.Default)
                    {
                        continue;
                    }

                    if (y != writeY)
                    {
                        var from = new CellCoord(x, y);
                        var to = new CellCoord(x, writeY);

                        yield return new FieldMove(from, to, elementNum);
                    }

                    writeY--;
                }

                var missingCount = writeY + 1;

                for (var i = 0; i < missingCount; i++)
                {
                    var elementNum = random.Next(0, field.MaxElementCount);
                    var targetY = i;
                    var fromY = i - missingCount;

                    var from = new CellCoord(x, fromY);
                    var to = new CellCoord(x, targetY);

                    yield return new FieldMove(from, to, elementNum);
                }
            }
        }
    }
}
