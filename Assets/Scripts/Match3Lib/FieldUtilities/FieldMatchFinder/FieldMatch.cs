using Match3Lib.FieldFiller;

namespace Match3Lib.FieldChecker
{
    public readonly struct FieldMatch
    {
        public readonly CellCoord StartCell;
        public readonly CellCoord EndCell;
        public readonly MatchType Type;
        public readonly int Length;
        public readonly int ElementNum;
        
        public FieldMatch(CellCoord startCell, CellCoord endCell, MatchType type, int length, int elementNum)
        {
            StartCell = startCell;
            EndCell = endCell;
            Type = type;
            Length = length;
            ElementNum = elementNum;
        }
    }

    public enum MatchType
    {
        Horizontal,
        Vertical,
    }
}