using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Threading.Tasks;
using YTScrapper.Application.Contracts;
using YTScrapper.Domain.Models;

namespace YTScrapper.Infrastructure.Repository
{
    public class SearchItemRepository : ISearchItemRepository
    {
        private readonly string _connectionString;

        public SearchItemRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlLiteDefault");
        }

        public async Task<int> Add(YouTubeSearchItem searchItem)
        {
            var sql = @"
insert into searchitem 
(""ImagePreviewUrl"", ""SearchItemUrl"", ""Title"", ""Description"", ""Author"", ""Duration"")
values(@preview, @url, @title, @description, @author, @duration) RETURNING Id;";

            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var output = await connection.QueryAsync<int>(sql, new {
                url = searchItem.Url,
                title = searchItem.Title,
                description = searchItem.Description,
                author = searchItem.Author,
                duration = searchItem.Duration,
            });

            return output.FirstOrDefault();
        }

        public async Task Delete(YouTubeSearchItem searchItem)
        {
            var sql = @"
delete from searchitem
where id = @id;";

            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var output = await connection.QueryAsync<YouTubeSearchItem>(sql, new
            {
                id = searchItem.Id,
            });
        }

        public async Task<List<YouTubeSearchItem>> Get()
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var output = await connection.QueryAsync<YouTubeSearchItem>(@"select * from searchitem;");
            return output.AsList();
        }

        public async Task Update(YouTubeSearchItem searchItem)
        {
            var sql = @"
UPDATE searchitem
SET ImagePreviewUrl = @preview, SearchItemUrl= @url, Title=@title, Description=@description, Author=@author, Duration=@duration
WHERE Id = @id;";

            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var output = await connection.QueryAsync<YouTubeSearchItem>(sql, new
            {
                id = searchItem.Id,
                url = searchItem.Url,
                title = searchItem.Title,
                description = searchItem.Description,
                author = searchItem.Author,
                duration = searchItem.Duration,
            });
        }
    }
}
