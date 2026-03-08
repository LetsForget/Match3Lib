namespace Match3Lib
{
    public readonly struct MatchStepResult
    {
        public readonly bool HasMatches;
        public readonly bool HasMoves;

        public MatchStepResult(bool hasMatches, bool hasMoves)
        {
            HasMatches = hasMatches;
            HasMoves = hasMoves;
        }
    }
}