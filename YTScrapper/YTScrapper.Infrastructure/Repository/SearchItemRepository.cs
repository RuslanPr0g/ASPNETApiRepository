using Dapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Data.SQLite;
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

        public async Task Add(SearchItem searchItem)
        {
            var sql = @"
insert into searchitem 
(""ImagePreviewUrl"", ""SearchItemUrl"", ""Title"", ""Description"", ""Author"", ""Duration"")
values(@preview, @url, @title, @description, @author, @duration);";

            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var output = await connection.QueryAsync<SearchItem>(sql, new {
                preview = searchItem.ImagePreviewUrl,
                url = searchItem.SearchItemUrl,
                title = searchItem.Title,
                description = searchItem.Description,
                author = searchItem.Author,
                duration = searchItem.Duration,
            });
        }

        public async Task Delete(SearchItem searchItem)
        {
            var sql = @"
delete from searchitem
where id = @id;";

            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var output = await connection.QueryAsync<SearchItem>(sql, new
            {
                id = searchItem.Id,
            });
        }

        public async Task<List<SearchItem>> Get()
        {
            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var output = await connection.QueryAsync<SearchItem>(@"select * from searchitem;");
            return output.AsList();
        }

        public async Task Update(SearchItem searchItem)
        {
            var sql = @"
UPDATE searchitem
SET ImagePreviewUrl = @preview, SearchItemUrl= @url, Title=@title, Description=@description, Author=@author, Duration=@duration
WHERE Id = @id;";

            using IDbConnection connection = new SQLiteConnection(_connectionString);
            connection.Open();
            var output = await connection.QueryAsync<SearchItem>(sql, new
            {
                id = searchItem.Id,
                preview = searchItem.ImagePreviewUrl,
                url = searchItem.SearchItemUrl,
                title = searchItem.Title,
                description = searchItem.Description,
                author = searchItem.Author,
                duration = searchItem.Duration,
            });
        }
    }
}
