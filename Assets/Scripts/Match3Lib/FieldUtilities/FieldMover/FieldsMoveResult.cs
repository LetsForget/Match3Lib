namespace Match3Lib.FieldFiller
{
    public readonly struct FieldsMoveResult
    {
        public readonly int MovesCount;
        public readonly FieldMove[] Moves;

        public FieldsMoveResult(int movesCount, FieldMove[] moves)
        {
            MovesCount = movesCount;
            Moves = moves;
        }
    }
}