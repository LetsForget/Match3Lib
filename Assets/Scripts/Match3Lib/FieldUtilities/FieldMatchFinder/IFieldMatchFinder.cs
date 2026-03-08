namespace Match3Lib.FieldChecker
{
    public interface IFieldMatchFinder
    {
        FieldMatchesResult GetMatchedCells(Field field, FieldMatch[] horizontal, FieldMatch[] vertical);
    }
}