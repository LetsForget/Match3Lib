namespace Match3Lib.FieldFiller
{
    public readonly struct FieldMove 
    {
        public readonly CellCoord From;
        public readonly CellCoord To;
        public readonly int ElementNum;

        public FieldMove(CellCoord from, CellCoord to, int elementNum)
        {
            From = from;
            To = to;
            ElementNum = elementNum;
        }
    }
}