using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TicTacToe.DataAccess;
using TicTacToe.Entities;

namespace TicTacToe
{
    public static class DataAccessQueries
    {
        public static IQueryable<TurnEntity> GetOrderedTurnsForGame(this IQueryable<TurnEntity> turns, int gameId) =>
            turns.Where(t => t.GameId == gameId).OrderBy(t => t.Id);

        public static async Task<FetchResult<T>> Fetch<T>(this IQueryable<T> entities, Fetch fetch)
        {
            if (fetch.PageSize <= 0)
            {
                fetch.PageSize = DataAccessConstants.FETCH_PAGE_SIZE;
            }

            int count = await entities.CountAsync();
            int pagesCount = (int)Math.Ceiling((double)count / fetch.PageSize);
            if (pagesCount > 0 && fetch.Page >= pagesCount)
            {
                fetch.Page = pagesCount - 1;
            }

            List<T> fetchedEntities =
                await entities.Skip(fetch.Page * fetch.PageSize).Take(fetch.PageSize).ToListAsync();
            return new FetchResult<T>()
            {
                Items = fetchedEntities,
                Page = fetch.Page,
                PagesCount = pagesCount,
                TotalCount = count
            };
        }
    }
}
