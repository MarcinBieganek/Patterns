using System;
using System.Collections.Generic;

namespace Zadanie_2
{
    public class Context
    {
        private Dictionary<string, bool> values;

        public Context()
        {
            values = new Dictionary<string, bool>();
        }
        public bool GetValue( string VariableName )
        {
            if (values.ContainsKey(VariableName))
            {
                return values[VariableName];
            }
            throw new ArgumentException();
        }
        public bool SetValue( string VariableName, bool Value )
        {
            if (values.ContainsKey(VariableName))
            {
                values[VariableName] = Value;
            } else
            {
                values.Add(VariableName, Value);
            }
            return Value;
        }
    }

    public abstract class AbstractExpression
    {
        public abstract bool Interpret( Context context );
    }

    public class ConstExpression : AbstractExpression
    {
        private bool value;

        public ConstExpression(bool val)
        {
            value = val;
        }

        public override bool Interpret(Context context)
        {
            return value;
        }
    }

    public class VariableExpression : AbstractExpression
    {
        private string name;

        public VariableExpression(string Name)
        {
            name = Name;
        }

        public override bool Interpret(Context context)
        {
            return context.GetValue(name);
        }
    }

    public class UnaryExpression : AbstractExpression
    {
        AbstractExpression expression;

        public UnaryExpression(AbstractExpression expr)
        {
            expression = expr;
        }

        public override bool Interpret(Context context)
        {
            return !(expression.Interpret(context));
        }
    }

    public enum BinaryType { Conjunction, Alternative};

    public class BinaryExpression : AbstractExpression
    {
        BinaryType type;
        AbstractExpression left;
        AbstractExpression right;

        public BinaryExpression(BinaryType Type, AbstractExpression Left, AbstractExpression Right)
        {
            type = Type;
            left = Left;
            right = Right;
        }

        public override bool Interpret(Context context)
        {
            switch (type)
            {
                case BinaryType.Conjunction:
                    return left.Interpret(context) && right.Interpret(context);
                case BinaryType.Alternative:
                    return left.Interpret(context) || right.Interpret(context);
                default:
                    throw new NotImplementedException();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Context ctx = new Context();
            ctx.SetValue("x", false);
            ctx.SetValue("y", true);

            AbstractExpression expr = 
                new BinaryExpression(
                    BinaryType.Alternative,
                    new BinaryExpression(
                        BinaryType.Conjunction,
                        new UnaryExpression(new ConstExpression(false)),
                        new VariableExpression("y")
                    ),
                    new BinaryExpression(
                        BinaryType.Conjunction,
                        new VariableExpression("x"),
                        new ConstExpression(true)
                    )
                );
            
            bool expr_val = expr.Interpret(ctx);

            Console.WriteLine("Wartość to: {0}", expr_val);
        }
    }
}
