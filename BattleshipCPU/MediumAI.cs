//might make this one more complex later, might be too easy


namespace BattleshipCPU;

class MediumAI : BattleshipAI
{
    public override (int row, int col) GetNextShot()
    {
        if (HasActiveHits())
            return GetChaserShot();

        return GetRandomUnknownCell();
    }

    private (int row, int col) GetChaserShot() //same chaser logic as in hard
    {
        const int ChaserBonus = 100;

        int[,] heatmap = new int[GridSize, GridSize];

        int[] dr = { -1, 1, 0, 0 };
        int[] dc = { 0, 0, -1, 1 };

        for (int r = 0; r < GridSize; r++)
        {
            for (int c = 0; c < GridSize; c++)
            {
                if (board[r, c] != CellState.Hit) continue;

                bool hasHorizontalNeighbour = IsHit(r, c - 1) || IsHit(r, c + 1);
                bool hasVerticalNeighbour = IsHit(r - 1, c) || IsHit(r + 1, c);

                for (int d = 0; d < 4; d++)
                {
                    int nr = r + dr[d];
                    int nc = c + dc[d];

                    if (!InBounds(nr, nc)) continue;
                    if (board[nr, nc] != CellState.Unknown) continue;

                    bool movingHorizontally = (dr[d] == 0);
                    if (hasHorizontalNeighbour && !movingHorizontally) continue;
                    if (hasVerticalNeighbour && movingHorizontally) continue;

                    heatmap[nr, nc] += ChaserBonus;
                }
            }
        }

        // pick the highest scoring cell
        int bestRow = -1, bestCol = -1, bestScore = -1;
        for (int r = 0; r < GridSize; r++)
            for (int c = 0; c < GridSize; c++)
                if (board[r, c] == CellState.Unknown && heatmap[r, c] > bestScore)
                {
                    bestScore = heatmap[r, c];
                    bestRow = r;
                    bestCol = c;
                }

        if (bestRow == -1)
            return GetRandomUnknownCell();

        return (bestRow, bestCol);
    }
}