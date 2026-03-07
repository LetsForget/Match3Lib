using System.Collections.Generic;

namespace Match3Lib.FieldFiller
{
    public interface IFieldMover
    {
        IEnumerable<FieldMove> GetFieldMoves(Field field);
    }
}