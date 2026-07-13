using System.Reflection;
using Reviews.Application.Contracts;

namespace Reviews.Infrastructure.Configuration;

public class Assemblies
{
    public static readonly Assembly Application = typeof(IReviewsModule).Assembly;
}
