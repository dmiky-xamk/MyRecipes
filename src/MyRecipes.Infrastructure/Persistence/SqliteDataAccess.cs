using Dapper;
using Microsoft.Extensions.Configuration;
using MyRecipes.Application.Common.Interfaces;
using System.Data;
using System.Data.SQLite;

namespace MyRecipes.Infrastructure.Persistence;

/// <summary>
/// Handles the direct contact with the Sqlite database.
/// </summary>
internal class SqliteDataAccess : IDataAccess
{
    private readonly IConfiguration _config;
    private readonly string _connectionString;

    public SqliteDataAccess(IConfiguration config)
    {
        _config = config;
        _connectionString = _config.GetConnectionString("Default");
    }

    public async Task<List<T>> QueryData<T, U>(string sqlStatement, U parameters)
    {
        // Luodaan yhteys connectionStringin osoittamaan SQL tietokantaan 'using' statementissa varmistaakseen yhteyden varmasti sulkeutuvan.
        // Haetaan (Query) tietokannasta dataa argumenttien perusteella.
        // Dapper palauttaa tiedot modeleina.
        using (IDbConnection connection = new SQLiteConnection(_connectionString))
        {
            // <T>: Model
            // sqlStatement: SELECT * Contacts...
            // paremeters: Limiters (id...)
            List<T> rows = (await connection.QueryAsync<T>(sqlStatement, parameters)).ToList();

            return rows;
        }
    }

    public async Task<T?> QueryDataSingle<T, U>(string sqlStatement, U parameters)
    {
        using (IDbConnection connection = new SQLiteConnection(_connectionString))
        {
            return await connection.QueryFirstOrDefaultAsync<T>(sqlStatement, parameters);
        }
    }

    // WRITE
    public async Task<int> SaveData<T>(string sqlStatement, T parameters)
    {
        // Dapper tallentaa (Execute) dataa tietokantaan.
        using (IDbConnection connection = new SQLiteConnection(_connectionString))
        {
            connection.Open();

            await connection.ExecuteAsync(sqlStatement, parameters);

            string sql = "SELECT last_insert_rowid();";

            long lastInsertId = (long)await connection.ExecuteScalarAsync(sql);

            connection.Close();

            return (int)lastInsertId;
        }
    }

    /// <summary>
    /// Execute a SQL statement.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="sqlStatement"></param>
    /// <param name="parameters"></param>
    /// <returns>The number of rows affected.</returns>
    public async Task<int> ExecuteStatement<T>(string sqlStatement, T parameters)
    {
        using (IDbConnection connection = new SQLiteConnection(_connectionString))
        {
            return await connection.ExecuteAsync(sqlStatement, parameters);
        }
    }
}