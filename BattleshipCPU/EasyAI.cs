using System;
using System.Collections.Generic;
using System.Text;

namespace BattleshipCPU
{
    internal class EasyAI
    {
        public override (int row, int col) GetNextShot()
        {
            return GetRandomUnknownCell();
        }
    }
}
