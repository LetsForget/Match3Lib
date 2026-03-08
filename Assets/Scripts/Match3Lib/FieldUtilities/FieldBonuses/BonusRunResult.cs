using Match3Lib.Data;
using Match3Lib.FieldFiller;

namespace Match3Lib.FieldUtilities.FieldBonuses
{
    public readonly struct BonusRunResult
    {
        public readonly bool Success;
        public readonly CellType BonusType;
        
        public readonly int AffectedCellsCount;
        public readonly CellCoord[] AffectedCells;
        
        public readonly int AffectedBonusCellsCount;
        public readonly CellCoord[] AffectedBonusCells;

        public BonusRunResult(bool success, CellType bonusType, int affectedCellsCount, CellCoord[] affectedCells,
            int affectedBonusCellsCount, CellCoord[] affectedBonusCells)
        {
            Success = success;
            BonusType = bonusType;
            AffectedCellsCount = affectedCellsCount;
            AffectedCells = affectedCells;
            AffectedBonusCellsCount = affectedBonusCellsCount;
            AffectedBonusCells = affectedBonusCells;
        }
    }
}