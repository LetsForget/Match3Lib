using System.Linq;
using Match3Lib.Data;
using Match3Lib.FieldChecker;
using Match3Lib.FieldFiller;
using Match3Lib.Visual;

namespace Match3Lib
{
    public class GameController
    {
        private readonly IFieldMover fieldMover;
        private readonly IFieldVisualizer fieldVisualizer;
        private readonly IFieldMatchFinder fieldMatchFinder;
        
        private Field field;

        public GameController(IFieldMover fieldMover, IFieldVisualizer fieldVisualizer, IFieldMatchFinder fieldMatchFinder)
        {
            this.fieldMover = fieldMover;
            this.fieldVisualizer = fieldVisualizer;
            this.fieldMatchFinder = fieldMatchFinder;
        }

        public async void Launch(FieldConfig fieldConfig)
        {
            field = new Field(fieldConfig);

            var moves = fieldMover.GetFieldMoves(field).ToArray();
            await fieldVisualizer.DrawFieldMoves(moves);
        }
    }
}