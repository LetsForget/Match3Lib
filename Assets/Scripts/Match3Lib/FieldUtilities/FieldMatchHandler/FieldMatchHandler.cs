using System.Collections.Generic;
using Match3Lib.Data;
using Match3Lib.FieldChecker;
using Match3Lib.FieldFiller;

namespace Match3Lib.FieldMatchHandler
{
    public class FieldMatchHandler : IFieldMatchHandler
    {
        public void Handle(Field field, IEnumerable<FieldMove> lastMoves, IEnumerable<Match> matches)
        {
            foreach (var match in matches)
            {
                HandleMatch(field, lastMoves, match);
            }
        }

        private void HandleMatch(Field field, IEnumerable<FieldMove> moves, Match match)
        {
            switch (match.Length)
            {
                case 3:
                    ClearMatchCells(field, match);
                    return;
                case 4:
                {
                    var lineCell = TryGetLastMoveInMatch(moves, match, out var last) ? last.To : match.StartCell;
                
                    ClearMatchCells(field, match);
                    SetLineBonus(field, match, lineCell);
                
                    return;
                }
                case 5:
                {
                    var bombCell = TryGetLastMoveInMatch(moves, match, out var last) ? last.To : match.StartCell;
             
                    ClearMatchCells(field, match);
                    SetBombBonus(field, match, bombCell);

                    return;
                }
            }
        }
        
        private bool TryGetLastMoveInMatch(IEnumerable<FieldMove> moves, Match match, out FieldMove last)
        {
            last = default;
            var found = false;
            
            foreach (var move in moves)
            {
                if (!IsCoordInMatch(move.To))
                {
                    continue;
                }
                
                last = move;
                found = true;
            }

            return found;
            
            bool IsCoordInMatch(CellCoord coord)
            {
                return match.Type == MatchType.Horizontal
                    ? coord.Y == match.StartCell.Y && coord.X >= match.StartCell.X && coord.X <= match.EndCell.X
                    : coord.X == match.StartCell.X && coord.Y >= match.StartCell.Y && coord.Y <= match.EndCell.Y;
            }
        }
        
        private void ClearMatchCells(Field field, Match match)
        {
            if (match.Type == MatchType.Horizontal)
            {
                for (var x = match.StartCell.X; x <= match.EndCell.X; x++)
                {
                    var y = match.StartCell.Y;
                    var cell = field[x, y];

                    cell.ElementNum = -1;

                    field[x, y] = cell;
                }
            }
            else
            {
                for (var y = match.StartCell.Y; y <= match.EndCell.Y; y++)
                {
                    var x = match.StartCell.X;
                    var cell = field[x, y];

                    cell.ElementNum = -1;

                    field[x, y] = cell;
                }
            }
        }

        private void SetLineBonus(Field field, Match match, CellCoord lastMoveTo)
        {
            var bonusCoord = GetBonusCoordinate(match, lastMoveTo);
            var cell = field[bonusCoord.X, bonusCoord.Y];

            if (match.Type == MatchType.Horizontal)
            {
                cell.Type = CellType.LineHorizontal;
            }
            else
            {
                cell.Type = CellType.LineVertical;
            }

            field[bonusCoord.X, bonusCoord.Y] = cell;
        }

        private void SetBombBonus(Field field, Match match, CellCoord lastMoveTo)
        {
            var bonusCoord = GetBonusCoordinate(match, lastMoveTo);
            var cell = field[bonusCoord.X, bonusCoord.Y];

            cell.Type = CellType.Bomb;

            field[bonusCoord.X, bonusCoord.Y] = cell;
        }

        private CellCoord GetBonusCoordinate(Match match, CellCoord moveTo)
        {
            var startX = match.StartCell.X;
            var startY = match.StartCell.Y;
            var endX = match.EndCell.X;
            var endY = match.EndCell.Y;

            if (match.Type == MatchType.Horizontal)
            {
                if (moveTo.Y == startY && moveTo.X >= startX && moveTo.X <= endX)
                {
                    return moveTo;
                }

                var middleX = startX + (endX - startX) / 2;
                var coord = new CellCoord(middleX, startY);
                return coord;
            }
            else
            {
                if (moveTo.X == startX && moveTo.Y >= startY && moveTo.Y <= endY)
                {
                    return moveTo;
                }

                var middleY = startY + (endY - startY) / 2;
                var coord = new CellCoord(startX, middleY);
                return coord;
            }
        }
    }
}
