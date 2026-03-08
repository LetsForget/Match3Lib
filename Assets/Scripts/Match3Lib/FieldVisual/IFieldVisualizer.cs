using System.Threading.Tasks;
using Match3Lib.FieldFiller;
using Match3Lib.FieldUtilities.FieldBonuses;

namespace Match3Lib.Visual
{
    public interface IFieldVisualizer
    {
        void Initialize(Field field);

        Task DrawFieldMoves(FieldsMoveResult move);

        void DrawBonus(BonusRunResult bonus);
    }
}
