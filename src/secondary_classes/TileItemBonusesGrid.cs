using SaYSpin.src.inventory_items.tile_items;

namespace SaYSpin.src.secondary_classes
{
    internal class TileItemBonusesGrid
    {
        private List<TileItemIncomeBonus>[,] _bonuses;

        public TileItemBonusesGrid(int rows, int columns)
        {
            _bonuses = new List<TileItemIncomeBonus>[rows, columns];
            InitializeEmpty();
        }

        private void InitializeEmpty()
        {
            for (int i = 0; i < _bonuses.GetLength(0); i++)
            {
                for (int j = 0; j < _bonuses.GetLength(1); j++)
                {
                    _bonuses[i, j] = new List<TileItemIncomeBonus>();
                }
            }
        }

        public List<TileItemIncomeBonus> GetBonusesFor(int row, int column)
        {
            if (row >= 0 && row < _bonuses.GetLength(0) && column >= 0 && column < _bonuses.GetLength(1))
            {
                return _bonuses[row, column];
            }
            throw new ArgumentOutOfRangeException("Row or column is out of the bonuses grid bounds.");
        }

        public void AddBonus(int row, int column, TileItemIncomeBonus bonus)
        {
            if (row < 0 || row >= _bonuses.GetLength(0) || column < 0 || column >= _bonuses.GetLength(1))
                throw new ArgumentOutOfRangeException("Row or column is out of the bonuses grid bounds.");

            _bonuses[row, column].Add(bonus);
        }

        public void AddBonuses(int row, int column, IEnumerable<TileItemIncomeBonus> bonuses)
        {
            if (row < 0 || row >= _bonuses.GetLength(0) || column < 0 || column >= _bonuses.GetLength(1))
                throw new ArgumentOutOfRangeException("Row or column is out of the bonuses grid bounds.");

            _bonuses[row, column].AddRange(bonuses);
        }
    }

}
