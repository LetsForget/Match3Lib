using System.Threading.Tasks;
using Match3Lib.Data;
using Match3Lib.FieldChecker;
using Match3Lib.FieldFiller;
using Match3Lib.FieldMatchHandler;
using Match3Lib.FieldUtilities.FieldSwapper;
using Match3Lib.Visual;

namespace Match3Lib
{
    public class GameController
    {
        private readonly IFieldMover fieldMover;
        private readonly IFieldVisualizer fieldVisualizer;
        private readonly IFieldMatchFinder fieldMatchFinder;
        private readonly IFieldMatchHandler fieldMatchHandler;
        private readonly IFieldSwapper fieldSwapper;

        private FieldMove[] fieldMoves;
        private FieldMatch[] horizontalMatches;
        private FieldMatch[] verticalMatches;

        private Field field;

        public GameController(
            IFieldMover fieldMover,
            IFieldVisualizer fieldVisualizer,
            IFieldMatchFinder fieldMatchFinder,
            IFieldMatchHandler fieldMatchHandler,
            IFieldSwapper fieldSwapper)
        {
            this.fieldMover = fieldMover;
            this.fieldVisualizer = fieldVisualizer;
            this.fieldMatchFinder = fieldMatchFinder;
            this.fieldMatchHandler = fieldMatchHandler;
            this.fieldSwapper = fieldSwapper;
        }

        public async void Launch(FieldConfig fieldConfig)
        {
            field = new Field(fieldConfig);

            var movesCount = field.Width * field.Height;
            fieldMoves = new FieldMove[movesCount];
            horizontalMatches = new FieldMatch[movesCount / 2];
            verticalMatches = new FieldMatch[movesCount / 2];

            fieldVisualizer.Initialize(field);

            var moveResult = fieldMover.GetFieldMoves(field, fieldMoves);
            ApplyMoveResult(moveResult);

            await fieldVisualizer.DrawFieldMoves(moveResult);
            await ResolveMatchesAndMoves(moveResult);
        }

        public async Task Swap(CellCoord from, CellCoord to)
        {
            var swapResult = fieldSwapper.Swap(field, from, to);
            await fieldVisualizer.DrawFieldMoves(swapResult);

            var result = await ResolveMatchesAndMoves(swapResult);

            if (!result.HasMatches && !result.HasMoves)
            {
                var revertSwap = fieldSwapper.Swap(field, to, from);
                await fieldVisualizer.DrawFieldMoves(revertSwap);
            }
        }

        private async Task<MatchStepResult> ResolveMatchesAndMoves(FieldsMoveResult moveResult)
        {
            var hasMatches = false;
            var hasMoves = false;

            while (true)
            {
                var matchResult = fieldMatchFinder.GetMatchedCells(field, horizontalMatches, verticalMatches);

                if (matchResult.HorizontalCount == 0 && matchResult.VerticalCount == 0)
                {
                    return new MatchStepResult(hasMatches, hasMoves);
                }

                hasMatches = true;
                fieldMatchHandler.Handle(field, moveResult, matchResult);
                moveResult = fieldMover.GetFieldMoves(field, fieldMoves);

                if (moveResult.MovesCount == 0)
                {
                    return new MatchStepResult(hasMatches, hasMoves);
                }

                hasMoves = true;
                ApplyMoveResult(moveResult);
                await fieldVisualizer.DrawFieldMoves(moveResult);
            }
        }
        
        private void ApplyMoveResult(FieldsMoveResult moveResult)
        {
            for (var i = 0; i < moveResult.MovesCount; i++)
            {
                var move = moveResult.Moves[i];

                if (move.From.Y >= 0)
                {
                    var fromCell = field[move.From.X, move.From.Y];
                    field[move.From.X, move.From.Y] = new Cell
                    {
                        ElementNum = -1,
                        Type = CellType.Default
                    };

                    field[move.To.X, move.To.Y] = fromCell;
                    continue;
                }

                field[move.To.X, move.To.Y] = new Cell
                {
                    ElementNum = move.ElementNum,
                    Type = CellType.Default
                };
            }
        }
    }
}