using System.Collections.Generic;
using System.Threading.Tasks;
using Match3Lib.FieldFiller;

namespace Match3Lib.Visual
{
    public interface IFieldVisualizer
    {
        void Initialize(Field field);
        
        Task DrawFieldMoves(IEnumerable<FieldMove> move);
    }
}