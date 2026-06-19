using System.Reflection;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

namespace Infrastructure.Serialization;

public class AllPropertiesContractResolver : DefaultContractResolver
{
    protected override IList<JsonProperty> CreateProperties(Type type, MemberSerialization memberSerialization)
    {
        var properties = type.GetProperties(
                BindingFlags.Public |
                BindingFlags.NonPublic |
                BindingFlags.Instance)
            .Select(p =>
            {
                var jp = CreateProperty(p, memberSerialization);
                jp.Readable = p.CanRead;
                jp.Writable = p.SetMethod != null;
                return jp;
            })
            .ToList();

        return properties;
    }

    protected override JsonObjectContract CreateObjectContract(Type objectType)
    {
        var contract = base.CreateObjectContract(objectType);

        if (contract.DefaultCreator == null && contract.OverrideCreator == null)
        {
            var ctor = objectType
                .GetConstructors(BindingFlags.NonPublic | BindingFlags.Instance)
                .OrderByDescending(c => c.GetParameters().Length)
                .FirstOrDefault();

            if (ctor != null)
            {
                contract.OverrideCreator = args => ctor.Invoke(args);
                foreach (var param in CreateConstructorParameters(ctor, contract.Properties))
                    contract.CreatorParameters.AddProperty(param);
            }
        }

        return contract;
    }
}
