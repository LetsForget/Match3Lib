namespace Match3Lib.FieldChecker
{
    public readonly struct FieldMatchesResult
    {
        public readonly FieldMatch[] Vertical;
        public readonly int VerticalCount;
        
        public readonly FieldMatch[] Horizontal;
        public readonly int HorizontalCount;

        public FieldMatchesResult(FieldMatch[] vertical, int verticalCount, FieldMatch[] horizontal, int horizontalCount)
        {
            Vertical = vertical;
            VerticalCount = verticalCount;
            Horizontal = horizontal;
            HorizontalCount = horizontalCount;
        }
    }
}