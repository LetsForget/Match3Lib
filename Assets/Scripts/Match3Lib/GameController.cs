using System.Threading.Tasks;
using Match3Lib.Data;
using Match3Lib.FieldChecker;
using Match3Lib.FieldFiller;
using Match3Lib.FieldMatchHandler;
using Match3Lib.FieldUtilities.FieldBonuses;
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
        private readonly IFieldBonusApplier fieldBonusApplier;

        private FieldMove[] fieldMoves;
        private FieldMatch[] horizontalMatches;
        private FieldMatch[] verticalMatches;

        private CellCoord[] affected;
        private CellCoord[] affectedBonuses;

        private Field field;

        public GameController(IFieldMover fieldMover, IFieldVisualizer fieldVisualizer, IFieldMatchFinder fieldMatchFinder,
            IFieldMatchHandler fieldMatchHandler, IFieldSwapper fieldSwapper, IFieldBonusApplier fieldBonusApplier)
        {
            this.fieldMover = fieldMover;
            this.fieldVisualizer = fieldVisualizer;
            this.fieldMatchFinder = fieldMatchFinder;
            this.fieldMatchHandler = fieldMatchHandler;
            this.fieldSwapper = fieldSwapper;
            this.fieldBonusApplier = fieldBonusApplier;
        }

        public async void Launch(FieldConfig fieldConfig)
        {
            field = new Field(fieldConfig);

            var cellsCount = field.Width * field.Height;

            fieldMoves = new FieldMove[cellsCount];
            horizontalMatches = new FieldMatch[cellsCount / 2];
            verticalMatches = new FieldMatch[cellsCount / 2];

            affected = new CellCoord[cellsCount];
            affectedBonuses = new CellCoord[cellsCount];

            fieldVisualizer.Initialize(field);

            var moveResult = fieldMover.GetFieldMoves(field, fieldMoves);
            ApplyMoveResult(moveResult);

            await fieldVisualizer.DrawFieldMoves(moveResult);
            await ResolveMatchesAndMoves(moveResult);
        }

        public async Task Swap(CellCoord from, CellCoord to)
        {
            fieldSwapper.Swap(field, from, to);

            var swapResult = CreateSwapMoveResult(from, to);
            await fieldVisualizer.DrawFieldMoves(swapResult);

            var result = await ResolveMatchesAndMoves(swapResult);

            if (!result.HasMatches && !result.HasMoves)
            {
                fieldSwapper.Swap(field, to, from);
                await fieldVisualizer.DrawFieldMoves(CreateSwapMoveResult(to, from));
            }
        }

        public async Task Click(CellCoord clickCoord)
        {
            var result = fieldBonusApplier.TryRunBonus(field, clickCoord, affected, affectedBonuses);

            if (!result.Success)
            {
                return;
            }
            
            ApplyBonus(clickCoord, result);
            
            await ResolveMovesAsync();
        }

        private void ApplyBonus(CellCoord bonusCoord, BonusRunResult result)
        {
            ClearCell(bonusCoord);
            
            for (var i = 0; i < result.AffectedCellsCount; i++)
            {
                var coord = affected[i];
                field[coord.X, coord.Y] = new Cell(-1, CellType.Default);
            }

            fieldVisualizer.DrawBonus(result);
            
            for (var j = 0; j < result.AffectedBonusCellsCount; j++)
            {
                var affectedBonusResult = fieldBonusApplier.TryRunBonus(field, affectedBonuses[j], affected, affectedBonuses);
                ApplyBonus(affected[j], affectedBonusResult);
            }
        }
        
        private async Task ResolveMovesAsync()
        {
            while (true)
            {
                var moveResult = fieldMover.GetFieldMoves(field, fieldMoves);

                if (moveResult.MovesCount == 0)
                {
                    return;
                }

                ApplyMoveResult(moveResult);
                await fieldVisualizer.DrawFieldMoves(moveResult);
                await ResolveMatchesAndMoves(moveResult);
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

        private FieldsMoveResult CreateSwapMoveResult(CellCoord from, CellCoord to)
        {
            var swapMoves = new FieldMove[2];
            swapMoves[0] = new FieldMove(from, to, field[to.X, to.Y].ElementNum);
            swapMoves[1] = new FieldMove(to, from, field[from.X, from.Y].ElementNum);

            return new FieldsMoveResult(2, swapMoves);
        }

        private void ClearCell(CellCoord coord)
        {
            field[coord.X, coord.Y] = new Cell(-1, CellType.Default);
        }
        
        private void ApplyMoveResult(FieldsMoveResult moveResult)
        {
            for (var i = 0; i < moveResult.MovesCount; i++)
            {
                var move = moveResult.Moves[i];

                if (move.From.Y >= 0)
                {
                    var fromCell = field[move.From.X, move.From.Y];
                    field[move.From.X, move.From.Y] = new Cell(-1, CellType.Default);
                    field[move.To.X, move.To.Y] = fromCell;
                    continue;
                }

                field[move.To.X, move.To.Y] = new Cell(move.ElementNum, CellType.Default);
            }
        }
    }
}