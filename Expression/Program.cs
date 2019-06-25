using System;
using System.Collections.Generic;
using System.Linq.Expressions;
namespace ExpressionTree
{
    class Program
    {
        static void Main(string[] args)
        {
            var l = ToUpper();
            //Console.WriteLine( l.Compile().Invoke("poyo"));
            //Example5();
            var content = @"{Name:Poyo,Address:{City:Toronto},Phone:[{one:1},{two:2}]}";
            JsonParser parse = new JsonParser(content);
            var result = parse.Parse();
            Console.WriteLine(result.ToString());
        }

        public void Example1()
        {
            // filter 
            int[] numbers = { 1, 2, 3, 10, 18, 25, 43, 67, 85, 93, 102, 110, 256 };

        }

        //Expression<Func<int,int,bool>> Compare(int left, int right, ExpressionType type)
        //{

        //}

        //Expression<Func<string,string>> Append(string name)
        //{
        //    var param = Expression.Parameter(typeof(string), "myName");
        //    var constant = Expression.Constant("Hello, ");
        //    var toUpper = typeof(string).GetMethod("ToUpper",Type.EmptyTypes);
        //    var method = Expression.Call(param,toUpper);

        //}

        Expression<Func<int, bool>> GetComparison(int rhs, ExpressionType op)
        {
            var lhsParam = Expression.Parameter(typeof(int), "x");
            var rhsParam = Expression.Constant(rhs);
            var binaryExpr = Expression.MakeBinary(op, lhsParam, rhsParam);

            // Making lambda
            var theLambda = Expression.Lambda<Func<int, bool>>(binaryExpr, lhsParam);
            return theLambda;
        }

        // str => str.ToUpper()
        // Expression.Call if static method => pass Type
        static Expression<Func<string, string>> ToUpper()
        {
            ParameterExpression param = Expression.Parameter(typeof(string));
            var toUpper = typeof(string).GetMethod("ToUpper", Type.EmptyTypes);
            MethodCallExpression body = Expression.Call(param, toUpper);
            var lambda = Expression.Lambda<Func<string, string>>(body, param);
            return lambda;
        }

        // OrderBy(c=> c.Name)  to    Orderby<T>("Name")
        static void OrderByPropertyOrField<T>(string propertiesOrFieldName, bool ascending)
        {
            Type elementType = typeof(T);
            var param = Expression.Parameter(elementType);
            var prop = Expression.PropertyOrField(param, propertiesOrFieldName);
            var selector = Expression.Lambda(prop, param);
        }

        // call OrderBy or OrderByDescending
        static void OrderBy(bool ascending)
        {
            //var orderByMethodName = ascending ? "OrderBy" : "OrderByDescending";
            //var body = Expression.Call(
            //        typeof(System.Linq.IQueryable),
            //        orderByMethodName,
            //        new[] {},

            //    );
        }

        // (x,y) => x ? y
        static void ArithmeticFunc(int left, int right, ExpressionType op)
        {
            var x = Expression.Parameter(typeof(int));
            var y = Expression.Parameter(typeof(int));
            var body = Expression.MakeBinary(op, x, y);
            var lambda = Expression.Lambda<Func<int, int, int>>(body, x, y);
        }

        //company => (company.ToLower() == "coho winery" || company.Length > 16)
        static void Example2()
        {
            // param company
            var param = Expression.Parameter(typeof(string), "company");

            // company.ToLower() == "coho winery"
            var const1 = Expression.Constant("coho winery");
            var toLower = Expression.Call(param, typeof(string).GetMethod("ToLower", Type.EmptyTypes));
            var left = Expression.Equal(toLower, const1);

            //company.Length > 16
            var prop = Expression.Property(param, "Length");
            var const2 = Expression.Constant(16);
            var right = Expression.GreaterThan(prop, const2);

            // body
            var body = Expression.Or(left, right);

            // lambda
            var lambda = Expression.Lambda<Func<string, bool>>(body, param);
        }

        static void Example3()
        {
            // param  and local variable
            var param = Expression.Parameter(typeof(int), "n");
            var res = Expression.Variable(typeof(int), "res");

            //
        }

        // string.Concat(x,y)
        static void Example4()
        {
            var x = Expression.Parameter(typeof(string));
            var y = Expression.Parameter(typeof(string));
            var concat = Expression.Call(typeof(string).GetMethod("Concat", new[] { typeof(string), typeof(string) }), x, y);
            var lambda = Expression.Lambda<Func<string, string, string>>(concat, x, y);

            Console.WriteLine(lambda.Compile().Invoke("hello ", "world"));
        }

        // x => new Person() {Name = x.Name, fiel}   to select(str,str)
        static void Example5()
        {
            var x = Expression.Parameter(typeof(Person));
            var xProp = Expression.Property(x, "Name");

            var createdType = typeof(Person);
            var ctor = Expression.New(createdType);
            var Name = createdType.GetProperty("Name");
            var assigment = Expression.Bind(Name, xProp);

            var memberInit = Expression.MemberInit(ctor, assigment);

            var lambda = Expression.Lambda<Func<Person, Person>>(memberInit, x);

            Console.WriteLine(lambda.ToString());

        }

        //{"Name": "poyo", "Age": 1 }
        static void Example6()
        {
            string json = @"""Name"": ""poyo"", ""Age"": 1";
            string[] keyValues = json.Split(",");

            // create 
            var ctor = Expression.New(typeof(Person));
            var list = new List<MemberAssignment>();
            foreach(var pair in keyValues)
            {
                string[] p = pair.Split(":");
                var prop = typeof(Person).GetProperty(p[0].Replace("\"", ""));
                var value = Expression.Constant(p[1]);
                var assignment = Expression.Bind(prop, value);
                list.Add(assignment);
            }
        }

        /*
       {
       "Name": "poyo", 
       "Age": 1 ,
       "Address": {

        }
       }

        }
         */
        //public void Parse(string json, int index)
        //{
        //    if (json[index] == '}') return;
        //    if(json[index++] == '{')
        //    {
        //        Dictionary<string, object> dic = new Dictionary<string, object>();
        //        json.
        //    }
        //}

        void Scan(string content)
        {
            int openTokenIndex = content.IndexOf('{');
            int closeTokenIndex = content.LastIndexOf('}');
        }

        //void parenthesis(string exp)
        //{
        //    Dictionary<string, object> dic = new Dictionary<string, object>();
        //    Stack<char> stack = new Stack<char>();

        //    foreach(char c in exp)
        //    {
        //        if (c == '{') {
        //            stack.Push('}');
        //            // Push the key
        //            // create new object
   
        //        }                
        //        else if (c == '}')
        //        {
        //            stack.Pop();
        //            // add the object as value to the key
        //            // pop the key
        //        }
        //        else // process as normal 
        //        {
             

        //        }
                   
        //    }
        //}
    }



    class Person
    {
        public string Name { get; set; }
        public int Age { get; set; }
    }
}
