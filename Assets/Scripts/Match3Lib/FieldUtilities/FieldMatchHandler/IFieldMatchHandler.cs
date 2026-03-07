using System.Collections.Generic;
using Match3Lib.FieldChecker;
using Match3Lib.FieldFiller;

namespace Match3Lib.FieldMatchHandler
{
    public interface IFieldMatchHandler
    {
        void Handle(Field field, IEnumerable<FieldMove> lastMoves, IEnumerable<Match> matches);
    }
}