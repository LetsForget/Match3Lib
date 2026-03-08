using Match3Lib.FieldFiller;

namespace Match3Lib.FieldUtilities.FieldSwapper
{
    public class FieldSwapper : IFieldSwapper
    {
        public FieldsMoveResult Swap(Field field, CellCoord from, CellCoord to)
        {
            var fromCell = field[from.X, from.Y];
            var toCell = field[to.X, to.Y];

            field[from.X, from.Y] = toCell;
            field[to.X, to.Y] = fromCell;

            var swapMoves = new FieldMove[2];
            swapMoves[0] = new FieldMove(from, to, field[to.X, to.Y].ElementNum);
            swapMoves[1] = new FieldMove(to, from, field[from.X, from.Y].ElementNum);

            return new FieldsMoveResult(2, swapMoves);
        }
    }
}
