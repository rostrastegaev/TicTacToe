using System;
using System.Collections.Generic;

namespace TicTacToe.Entities
{
    public class GameEntity
    {
        public int Id { get; set; }
        public bool IsPlayerFirst { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime? EndedAt { get; set; }

        public ICollection<TurnEntity> Turns { get; set; }
    }
}
