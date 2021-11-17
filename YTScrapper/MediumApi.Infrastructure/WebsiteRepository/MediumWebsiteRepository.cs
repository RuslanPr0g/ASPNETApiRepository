using Dapper;
using MediumApi.Application.Contract;
using MediumApi.Domain.Models;
using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace MediumApi.Infrastructure.WebsiteRepository
{
    public class MediumWebsiteRepository : IMediumWebsiteRepository
    {
        private readonly string _connectionString;

        public MediumWebsiteRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("SqlLiteDefault");
        }

        public async Task<int> AddPost(Post post)
        {
            var sql = @"
insert into post 
(""Link"", ""Title"", ""Author"", ""Description"", ""Content"", ""Thumbnail"", ""PubDate"")
values(@link, @title, @author, @description, @content, @thumbnail, @pubdate) RETURNING Id;";

            using IDbConnection connection = new SqliteConnection(_connectionString);
            connection.Open();
            var output = await connection.QueryAsync<int>(sql, new
            {
                link = post.Link,
                title = post.Title,
                author = post.Author,
                description = post.Description,
                content = post.Content,
                thumbnail = post.Thumbnail,
                pubdate = post.PubDate,
            });

            foreach (var category in post.Categories)
            {
                await AddCategory(category);
            }

            return output.FirstOrDefault();
        }

        public async Task DeletePost(Post post)
        {
            var sql = @"
delete from post
where id = @id;";

            using IDbConnection connection = new SqliteConnection(_connectionString);
            connection.Open();
            var output = await connection.QueryAsync<Post>(sql, new
            {
                id = post.Id,
            });
        }

        public async Task<List<Post>> GetPosts()
        {
            var sql = @"
SELECT p.*, c.*
FROM post p
LEFT JOIN category c ON c.PostId = p.Id; ";
            using IDbConnection connection = new SqliteConnection(_connectionString);
            connection.Open();
            var output = await connection.QueryAsync<Post>(sql);

            connection.Query<Post, Category, Post>(sql, (p, c) =>
            {
                p.Categories ??= new List<Category>();
                p.Categories.Add(c);
                return p;
            }, splitOn: "Id").AsQueryable();

            return output.AsList();
        }

        public async Task UpdatePost(Post post)
        {
            var sql = @"
UPDATE post
SET Link = @link, Title= @title, Author=@author, Description=@description, Author=@content, Thumbnail=@thumbnail, PubDate=@pubdate
WHERE Id = @id;";

            using IDbConnection connection = new SqliteConnection(_connectionString);
            connection.Open();
            var output = await connection.QueryAsync<Post>(sql, new
            {
                id = post.Id,
                link = post.Link,
                title = post.Title,
                author = post.Author,
                description = post.Description,
                content = post.Content,
                thumbnail = post.Thumbnail,
                pubdate = post.PubDate,
            });
        }

        public async Task<int> AddCategory(Category category)
        {
            var sql = @"
insert into category 
(""PostId"", ""Content"")
values(@postId, @content) RETURNING Id;";

            using IDbConnection connection = new SqliteConnection(_connectionString);
            connection.Open();
            var output = await connection.QueryAsync<int>(sql, new
            {
                postId = category.PostId,
                content = category.Content,
            });

            return output.FirstOrDefault();
        }
    }
}
