﻿namespace MyRecipes.Application.Common.Interfaces;

public interface IDataAccess
{
    Task<List<T>> QueryData<T, U>(string sqlStatement, U parameters);
    Task<T?> QueryDataSingle<T, U>(string sqlStatement, U parameters);
    Task<int> SaveData<T>(string sqlStatement, T parameters);
    Task<int> ExecuteStatement<T>(string sqlStatement, T parameters);
}