using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Infrastructure.Persistence;

public interface IDataAccess
{
    Task<IEnumerable<RecipeEntity>> QueryRecipes<T>(string sqlStatement, T parameters);
    Task<List<T>> QueryData<T, U>(string sqlStatement, U parameters);
    Task<T?> QueryDataSingle<T, U>(string sqlStatement, U parameters);
    Task<int> ExecuteStatement<T>(string sqlStatement, T parameters);
    Task<bool> ExecuteScalar<T>(string sqlStatement, T parameters);
}