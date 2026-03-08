using Match3Lib.Data;

namespace Match3Lib
{
    public struct Cell 
    {
        public int ElementNum;
        public CellType Type;

        public Cell(int elementNum, CellType type)
        {
            ElementNum = elementNum;
            Type = type;
        }
    }
}