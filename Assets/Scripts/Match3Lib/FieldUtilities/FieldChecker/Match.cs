using Match3Lib.FieldFiller;

namespace Match3Lib.FieldChecker
{
    public readonly struct Match
    {
        public readonly CellCoord StartCell;
        public readonly CellCoord EndCell;
        public readonly MatchType Type;
        public readonly int Length;
        
        public Match(CellCoord startCell, CellCoord endCell, MatchType type, int length)
        {
            StartCell = startCell;
            EndCell = endCell;
            Type = type;
            Length = length;
        }
    }

    public enum MatchType
    {
        Horizontal,
        Vertical,
    }
}