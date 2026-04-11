using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipCPU
{
    abstract class BattleshipAI
    {
        protected const int GridSize = 10;
        protected CellState[,] board = new CellState[GridSize, GridSize];
        protected List<Ship> remainingShips = new List<Ship>();
        protected Random random = new Random();

        public void RegisterShot(int row, int col, CellState result)
        {
            board[row, col] = result;
        }

        public void RegisterSunk(Ship ship)
        {
            remainingShips.Remove(ship);
        }

        public abstract (int row, int col) GetNextShot();

        protected bool InBounds(int r, int c) =>
            r >= 0 && r < GridSize && c >= 0 && c < GridSize;

        protected bool IsHit(int r, int c) =>
            InBounds(r, c) && board[r, c] == CellState.Hit;

        protected bool HasActiveHits() //a cell that has been struck but the ship it belongs to hasnt been fully sunk yet
        {
            for (int r = 0; r < GridSize; r++)
                for (int c = 0; c < GridSize; c++)
                    if (board[r, c] == CellState.Hit)
                        return true;
            return false;
        }

        protected (int row, int col) GetRandomUnknownCell()
        {
            int r, c;
            do
            {
                r = random.Next(GridSize);
                c = random.Next(GridSize);
            }
            while (board[r, c] != CellState.Unknown);
            return (r, c);
        }
    }
}