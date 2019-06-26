using System;
using System.Collections.Generic;
using System.Text;
using System.Linq.Expressions;
using System.Linq;

namespace ExpressionTree
{
    class ObjectMapper
    {
        public void Map<T,E>(T source, E dest) 
        {
            Mapper<T, E>.Map(source, dest);
        }

        static class Mapper<T, E>
        {
            private static Action<T, E> mapper;
            static Mapper()
            {
                var source = Expression.Parameter(typeof(T), "source");
                var dest = Expression.Parameter(typeof(E), "dest");
                var assigmnents = typeof(T).GetProperties()
                    .Where(prop => prop.CanRead && prop.CanWrite)
                    .Select(prop => Expression.Assign(
                            Expression.Property(source, prop),
                            Expression.Property(dest, prop)
                     ));
                var body = Expression.Block(assigmnents);
                mapper = Expression.Lambda<Action<T, E>>(body, source, dest).Compile();

            }

            public static void Map(T source, E dest)
            {
                 mapper(source, dest);
            }
        }
    }
}
