using Match3Lib.FieldFiller;

namespace Match3Lib.FieldUtilities.FieldBonuses
{
    public interface IFieldBonusApplier
    {
        BonusRunResult TryRunBonus(Field field, CellCoord coord, CellCoord[] affectedCells, CellCoord[] affectedBonusCells);
    }
}