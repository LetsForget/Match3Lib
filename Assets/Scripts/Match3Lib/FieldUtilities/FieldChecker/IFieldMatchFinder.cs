using System.Collections.Generic;

namespace Match3Lib.FieldChecker
{
    public interface IFieldMatchFinder
    {
        IEnumerable<Match> GetMatchedCells(Field field);
    }
}