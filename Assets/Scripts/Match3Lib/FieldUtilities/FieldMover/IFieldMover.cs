namespace Match3Lib.FieldFiller
{
    public interface IFieldMover
    {
        FieldsMoveResult GetFieldMoves(Field field, FieldMove[] moves);
    }
}