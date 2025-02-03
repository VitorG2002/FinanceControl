using FinanceControl.FinanceControl.Application.Common;

namespace FinanceControl.FinanceControl.Application.Extensions
{
    public static class AutoMapperExtensions
    {
        public static TDestination MapTo<TSource, TDestination>(this TSource source)
            where TSource : class
            where TDestination : class
            =>
            AutoMapperFactory.Mapper.Map<TSource, TDestination>(source);

        public static TDestination MapTo<TSource, TDestination>(this TSource source, TDestination destination)
            where TSource : class
            where TDestination : class
            =>
            AutoMapperFactory.Mapper.Map(source, destination);

    }

}
