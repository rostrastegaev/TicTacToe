using System;
using System.Collections.Generic;

namespace TicTacToe.Web
{
    public class GameViewModel
    {
        public int Id { get; set; }
        public IEnumerable<TurnInfoViewModel> History { get; set; }
        public bool IsPlayerFirst { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }
    }
}
