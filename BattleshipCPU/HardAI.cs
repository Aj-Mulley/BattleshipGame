using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipCPU
{
    internal class HardAI : BattleshipAI
    {
        // builds the heatmap and returns the highest scoring untouched cell as the next shot
        public override (int row, int col) GetNextShot()
        {
            int[,] heatmap = BuildHeatmap();

            int bestRow = 0;
            int bestCol = 0;
            int bestScore = -1;


            for (int c = 0; c < GridSize; c++)
                if (board[r, c] == CellState.Unknown && heatmap[r, c] > bestScore)
                {
                    bestScore = heatmap[r, c];
                    bestRow = r;
                    bestCol = c;
                }

            return (bestRow, bestCol);
        }

        // builds a gridsize by gridsize grid where each cell value represents how many valid ship placements cover it
        private int[,] BuildHeatmap()
        {
            int[,] heatmap = new int[GridSize, GridSize];

            foreach (Ship ship in remainingShips)
                AddShipProbabilities(heatmap, ship);

            if (HasActiveHits())
                ApplyChaserBoost(heatmap);

            return heatmap;
        }

        // for a given ship, tries every row/col/orientation combo, each valid placement increments the cells it would be on
        private void AddShipProbabilities(int[,] heatmap, Ship ship)
        {
            for (int r = 0; r < GridSize; r++)
                for (int c = 0; c < GridSize; c++)
                {
                    if (CanPlaceShip(ship, r, c, horizontal: true))
                        AddPlacementToHeatmap(heatmap, ship, r, c, horizontal: true);
                    if (CanPlaceShip(ship, r, c, horizontal: false))
                        AddPlacementToHeatmap(heatmap, ship, r, c, horizontal: false);
                }
        }

        // returns true if the ship fits on the board starting at (startRow, startCol) without going out of bounds or overlapping a known miss
        private bool CanPlaceShip(Ship ship, int startRow, int startCol, bool horizontal)
        {
            for (int i = 0; i < ship.Size; i++)
            {
                // step along the row (horizontal) or column (vertical)
                int r;
                if (horizontal)
                    r = startRow;
                else
                    r = startRow + i;

                int c;
                if (horizontal)
                    c = startCol + i;
                else
                    c = startCol;

                if (!InBounds(r, c)) return false; // would go off the board
                if (board[r, c] == CellState.Miss) return false; // known miss at this cell
            }
            return true;
        }

        // increments every cell this placement would cover
        private void AddPlacementToHeatmap(int[,] heatmap, Ship ship, int startRow, int startCol, bool horizontal)
        {
            for (int i = 0; i < ship.Size; i++)
            {
                int r;
                if (horizontal)
                    r = startRow;
                else
                    r = startRow + i;

                int c;
                if (horizontal)
                    c = startCol + i;
                else
                    c = startCol;

                heatmap[r, c]++;
            }
        }

        // adds a large bonus to untouched cells adjacent to known hits
        private void ApplyChaserBoost(int[,] heatmap)
        {

            const int ChaserBonus = 100;

            // cardinal directions: up, down, left, right
            int[] dr = { -1, 1, 0, 0 };
            int[] dc = { 0, 0, -1, 1 };

            for (int r = 0; r < GridSize; r++)
            {
                for (int c = 0; c < GridSize; c++)
                {
                    if (board[r, c] != CellState.Hit) continue;

                    // Check whether this hit already has a hit neighbour on either axix, if so, the ship's orientation is known 
                    bool hasHorizontalNeighbour = IsHit(r, c - 1) || IsHit(r, c + 1);
                    bool hasVerticalNeighbour = IsHit(r - 1, c) || IsHit(r + 1, c);

                    for (int d = 0; d < 4; d++) // nr = neighbour row, nc = neighbour col
                    {
                        int nr = r + dr[d];
                        int nc = c + dc[d];

                        if (!InBounds(nr, nc)) continue;
                        if (board[nr, nc] != CellState.Unknown) continue;

                        // if orientation is established skip directions off that axis
                        bool movingHorizontally = (dr[d] == 0);
                        if (hasHorizontalNeighbour && !movingHorizontally) continue;
                        if (hasVerticalNeighbour && movingHorizontally) continue;

                        heatmap[nr, nc] += ChaserBonus;
                    }
                }
            }
        }
    }
}