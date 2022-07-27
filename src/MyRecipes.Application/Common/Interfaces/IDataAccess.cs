using MyRecipes.Domain.Entities;

namespace MyRecipes.Application.Common.Interfaces;

public interface IDataAccess
{
    Task<IEnumerable<RecipeEntity>> QueryRecipes<T>(string sqlStatement, T parameters);
    Task<List<T>> QueryData<T, U>(string sqlStatement, U parameters);
    Task<T?> QueryDataSingle<T, U>(string sqlStatement, U parameters);
    Task<int> ExecuteStatement<T>(string sqlStatement, T parameters);
}