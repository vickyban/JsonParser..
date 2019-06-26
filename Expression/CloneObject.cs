using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Linq;

namespace ExpressionTree
{
    public static class ObjectExt
    {
        public static T Clone<T>(this T obj) where T : new()
        {
            return ObjectExtCache<T>.Clone(obj);
        }

        static class ObjectExtCache<T> where T : new()
        {
            private static readonly Func<T, T> cloner;
            static ObjectExtCache()
            {
                var type = typeof(T);
                var param = Expression.Parameter(type, "o");
                var bindings = type.GetProperties()
                    .Where(prop => prop.CanRead && prop.CanWrite)
                    .Select(prop => Expression.Bind(prop, Expression.Property(param, prop)));
                var lambda = Expression.Lambda<Func<T, T>>(
                        Expression.MemberInit(
                            Expression.New(type),
                            bindings),
                        param
                    );
                cloner = lambda.Compile();
            }

            public static T Clone(T obj)
            {
                return cloner(obj);
            }
        }
    }

    public class Foo
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DateTime DoB { get; set; }
    }
}
