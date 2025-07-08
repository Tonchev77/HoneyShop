namespace HoneyShop.Web.Infrastructure.Extensions
{
    using Microsoft.Extensions.DependencyInjection;
    using System.Reflection;
    public static class RepositoryCollectionExtensions
    {
        private static readonly string RepositoryInterfacePrefix = "I";
        private static readonly string RepositoryTypeSuffix = "Repository";

        public static IServiceCollection AddUserDefinedRepositories(this IServiceCollection repositoryCollection, Assembly repositoryAssembly)
        {
            Type[] repositoryClasses = repositoryAssembly
                .GetTypes()
                .Where(t => !t.IsInterface &&
                                 t.Name.EndsWith(RepositoryTypeSuffix))
                .ToArray();
            foreach (Type repositoryClass in repositoryClasses)
            {
                Type[] repositoryClassInterfaces = repositoryClass
                    .GetInterfaces();
                if (repositoryClassInterfaces.Length == 1 &&
                    repositoryClassInterfaces.First().Name.StartsWith(RepositoryInterfacePrefix) &&
                    repositoryClassInterfaces.First().Name.EndsWith(RepositoryTypeSuffix))
                {
                    Type repositoryClassInterface = repositoryClassInterfaces.First();

                    repositoryCollection.AddScoped(repositoryClassInterface, repositoryClass);
                }
            }

            return repositoryCollection;
        }
    }
}
