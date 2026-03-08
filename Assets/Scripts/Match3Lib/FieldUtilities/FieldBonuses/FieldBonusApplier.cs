using Match3Lib.Data;
using Match3Lib.FieldFiller;

namespace Match3Lib.FieldUtilities.FieldBonuses
{
    public class FieldBonusApplier : IFieldBonusApplier
    {
        public BonusRunResult TryRunBonus(Field field, CellCoord coord, CellCoord[] affectedCells, CellCoord[] affectedBonusCells)
        {
            var cell = field[coord.X, coord.Y];

            return cell.Type switch
            {
                CellType.Default => new BonusRunResult(false, CellType.Default, 0, affectedCells, 0, affectedBonusCells),
                CellType.LineVertical => CollectVertical(field, coord, affectedCells, affectedBonusCells),
                CellType.LineHorizontal => CollectHorizontal(field, coord, affectedCells, affectedBonusCells),
                CellType.Bomb => CollectBomb(field, coord, affectedCells, affectedBonusCells),
            };
        }

        private BonusRunResult CollectVertical(Field field, CellCoord coord, CellCoord[] affectedCells, CellCoord[] affectedBonusCells)
        {
            var affectedCount = 0;
            var affectedBonusCount = 0;

            for (var y = coord.Y - 1; y >= 0; y--)
            {
                AddCell(field, new CellCoord(coord.X, y), affectedCells, ref affectedCount, affectedBonusCells, ref affectedBonusCount);
            }

            for (var y = coord.Y + 1; y < field.Height; y++)
            {
                AddCell(field, new CellCoord(coord.X, y), affectedCells, ref affectedCount, affectedBonusCells, ref affectedBonusCount);
            }

            return new BonusRunResult(true, CellType.LineVertical, affectedCount, affectedCells, affectedBonusCount, affectedBonusCells);
        }

        private BonusRunResult CollectHorizontal(Field field, CellCoord coord, CellCoord[] affectedCells, CellCoord[] affectedBonusCells)
        {
            var affectedCount = 0;
            var affectedBonusCount = 0;

            for (var x = coord.X - 1; x >= 0; x--)
            {
                AddCell(field, new CellCoord(x, coord.Y), affectedCells, ref affectedCount, affectedBonusCells, ref affectedBonusCount);
            }

            for (var x = coord.X + 1; x < field.Width; x++)
            {
                AddCell(field, new CellCoord(x, coord.Y), affectedCells, ref affectedCount, affectedBonusCells, ref affectedBonusCount);
            }

            return new BonusRunResult(true, CellType.LineHorizontal, affectedCount, affectedCells, affectedBonusCount, affectedBonusCells);
        }

        private BonusRunResult CollectBomb(Field field, CellCoord coord, CellCoord[] affectedCells, CellCoord[] affectedBonusCells)
        {
            var affectedCount = 0;
            var affectedBonusCount = 0;

            var minX = coord.X - 1;
            var maxX = coord.X + 1;
            var minY = coord.Y - 1;
            var maxY = coord.Y + 1;

            for (var y = minY; y <= maxY; y++)
            {
                if (y < 0 || y >= field.Height)
                {
                    continue;
                }

                for (var x = minX; x <= maxX; x++)
                {
                    if (x < 0 || x >= field.Width)
                    {
                        continue;
                    }

                    if (x == coord.X && y == coord.Y)
                    {
                        continue;
                    }

                    AddCell(field, new CellCoord(x, y), affectedCells, ref affectedCount, affectedBonusCells, ref affectedBonusCount);
                }
            }

            return new BonusRunResult(true, CellType.Bomb, affectedCount, affectedCells, affectedBonusCount, affectedBonusCells);
        }

        private void AddCell(Field field, CellCoord coord, CellCoord[] affectedCells, ref int affectedCount, CellCoord[] affectedBonusCells, ref int affectedBonusCount)
        {
            var cell = field[coord.X, coord.Y];

            if (cell.Type == CellType.Default)
            {
                affectedCells[affectedCount++] = coord;
            }
            else
            {
                affectedBonusCells[affectedBonusCount++] = coord;
            }
        }
    }
}
