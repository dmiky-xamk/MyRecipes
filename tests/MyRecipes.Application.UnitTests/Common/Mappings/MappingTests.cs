using AutoMapper;
using MyRecipes.Application.Common.Mappings;
using MyRecipes.Application.Recipes.Queries.GetRecipes;
using MyRecipes.Domain.Entities;
using System.Runtime.Serialization;

namespace MyRecipes.Application.UnitTests.Common.Mappings;

public class MappingTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingTests()
    {
        _configuration = new MapperConfiguration(config
            => config.AddProfile<MappingProfiles>());

        _mapper = _configuration.CreateMapper();
    }

    [Fact]
    public void AutoMapper_Configuration_IsValid()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Theory]
    [InlineData(typeof(IngredientEntity), typeof(RecipeDto))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);   
    }

    private object GetInstanceOf(Type type)
    {
        if (type.GetConstructor(Type.EmptyTypes) != null)
            return Activator.CreateInstance(type)!;

        // Type without parameterless constructor
        return FormatterServices.GetUninitializedObject(type);
    }
}