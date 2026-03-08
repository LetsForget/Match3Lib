using Match3Lib.FieldFiller;

namespace Match3Lib.FieldUtilities.FieldSwapper
{
    public interface IFieldSwapper
    {
        FieldsMoveResult Swap(Field field, CellCoord from, CellCoord to);
    }
}