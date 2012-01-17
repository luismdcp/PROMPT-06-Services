using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GitHubSoap.Server.Batching.Requests;
using GitHubSoap.Server.Batching.Responses;

namespace GitHubSoap.Server.Implementation
{
    public static class KnownTypeProvider
    {
        private static readonly List<Type> KnownTypes;

        static KnownTypeProvider()
        {
            KnownTypes = BuildListOfKnownTypes();
        }

        public static IEnumerable<Type> GetKnownTypes(ICustomAttributeProvider provider)
        {
            return KnownTypes;
        }

        private static List<Type> BuildListOfKnownTypes()
        {
            Assembly containingAssembly = typeof(KnownTypeProvider).Assembly;

            return containingAssembly.GetTypes()
                                    .Where(t => t.IsSubclassOf(typeof(Request)) || t.IsSubclassOf(typeof(Response)))
                                    .ToList();
        }
    }
}