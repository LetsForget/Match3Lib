using Match3Lib.Data;

namespace Match3Lib
{
    public readonly struct Field
    {
        private readonly Cell[] cells;
        
        public readonly int Width;
        public readonly int Height;

        public readonly int MaxElementCount;
        
        public Field(FieldConfig config)
        {
            Width = config.width;
            Height = config.height;
            MaxElementCount = config.maxElementNum;
            
            cells = new Cell[Width * Height];
        }
        
        public Cell this[int x, int y]
        {
            get
            {
                var index = GetIndex(x, y);
                return cells[index];
            }
            set
            {
                var index = GetIndex(x, y);
                cells[index] = value;
            }
        }
        
        private int GetIndex(int x, int y)
        {
            return y * Width + x;
        }
    }
}