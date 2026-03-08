namespace Match3Lib.FieldUtilities.FieldSwapper
{
    public readonly struct FieldSwapResult
    {
        public readonly Cell From;
        public readonly Cell To;

        public FieldSwapResult(Cell from, Cell to)
        {
            From = from;
            To = to;
        }
    }
}