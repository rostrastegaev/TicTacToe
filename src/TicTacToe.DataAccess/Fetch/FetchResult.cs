using System.Collections.Generic;

namespace TicTacToe.DataAccess
{
    public class FetchResult<T>
    {
        public int Page { get; set; }
        public int PagesCount { get; set; }
        public int TotalCount { get; set; }
        public IEnumerable<T> Items { get; set; }
    }
}
