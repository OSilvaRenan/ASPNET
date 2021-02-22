using Dapper;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace WebApp.Identity
{
    public class MyUserStore : IUserStore<MyUser>
    {
        public async Task<IdentityResult> CreateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync("insert into Users([Id], " +
                    "[UserName], " +
                    "[NormalizedUserName], " +
                    "[PassowordHash]) " +
                    "Values (@id, @userName, @normalizedUserName, @passowordHash)",
                    new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passowordHash = user.PassowordHash
                    });
            }
            return IdentityResult.Success;
        }

        public async Task<IdentityResult> DeleteAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync("delete * from Users where id = @id",
                    new
                    {
                        id = user.Id
                    });
            }
            return IdentityResult.Success;
        }

        public void Dispose()
        {
           
        }


        //Cria conexão com o banco de Dados
        public static DbConnection GetOpenConnection()
        {
            var connection = new SqlConnection("Integrated Security=SSPI;" +
                "Persist Security Info=False;" +
                "Initial Catalog=IdentityCurso;" +
                @"Data Source=DESKTOP-QDNK1OC\SQLEXPRESS");

            connection.Open();
            return connection;
        }

        public async Task<MyUser> FindByIdAsync(string userId, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<MyUser>(
                    "select * from Users where Id = @id",
                     new { id = userId });
            }
        }

        public async Task<MyUser> FindByNameAsync(string normalizedUserName, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                return await connection.QueryFirstOrDefaultAsync<MyUser>(
                    "select * from Users where normalizedUserName = @nome",
                     new { nome = normalizedUserName });
            }
        }

        public Task<string> GetNormalizedUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.NormalizedUserName);
        }

        public Task<string> GetUserIdAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.Id);
        }

        public Task<string> GetUserNameAsync(MyUser user, CancellationToken cancellationToken)
        {
            return Task.FromResult(user.UserName);
        }

        public Task SetNormalizedUserNameAsync(MyUser user, string normalizedName, CancellationToken cancellationToken)
        {
            user.NormalizedUserName = normalizedName;
            return Task.CompletedTask;
        }

        public Task SetUserNameAsync(MyUser user, string userName, CancellationToken cancellationToken)
        {
            user.UserName = userName;
            return Task.CompletedTask;
        }

        public async Task<IdentityResult> UpdateAsync(MyUser user, CancellationToken cancellationToken)
        {
            using (var connection = GetOpenConnection())
            {
                await connection.ExecuteAsync("update Users " +
                    "set [Id] = @id," +
                    " [UserName] = @userName," +
                    " [NormalizedUserName] = @normalizedUserName," +
                    " [PassowordHash)] = @passowordHash" +
                    " where [Id] = @id ",
                    new
                    {
                        id = user.Id,
                        userName = user.UserName,
                        normalizedUserName = user.NormalizedUserName,
                        passowordHash = user.PassowordHash
                    });
            }

            return IdentityResult.Success;
        }
    }
}
