using System.Linq.Expressions;


// x => x + 5

ParameterExpression param = Expression.Parameter(typeof(int));
ConstantExpression constant = Expression.Constant(5, typeof(int));
BinaryExpression add = Expression.Add(param, constant);
Expression<Func<int, int>> lambda = Expression.Lambda<Func<int, int>>(add, param);
Func<int, int> func = lambda.Compile();

Console.WriteLine(func(10)); // 15