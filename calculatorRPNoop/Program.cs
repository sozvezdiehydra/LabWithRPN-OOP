using System;
namespace labwithrpnoop
{
    class Program
    {
        public abstract class Token
        {
            // General properties and methods for all tokens
        }
        static double secondOperation(Operation operation, double number1, double number2)
        {
            if (operation.Symbol == '+') return number1 + number2;
            else if (operation.Symbol == '-') return number2 - number1;
            else if (operation.Symbol == '*') return number1 * number2;
            else return number2 / number1;
        }

        // Parenthensis class
        public class Parenthesis : Token
        {
            // 
            public char Symbol { get; }
            public bool isClosing { get; }
            public Parenthesis(char symbol)
            {
                Symbol = symbol;
                isClosing = symbol == ')';
            }
        }

        // Number class
        public class Number : Token
        {
            public double Value { get; }

            public Number(double value)
            {
                Value = value;
            }
        }

        // Operation class
        public class Operation : Token
        {
            public char Symbol { get; }
            public int Priority { get; }

            public Operation(char symbol)
            {
                Symbol = symbol;
                Priority = GetPriority(symbol);
            }
            private static int GetPriority(char symbol)
            {
                switch (symbol)
                {
                    case '(': return 0;
                    case ')': return 0;
                    case '+': return 1;
                    case '-': return 1;
                    case '*': return 2;
                    case '/': return 2;
                    default: return 3;
                }
            }

            //Main method
            class RPN
            {
                static void Main()
                {
                    Console.Write("Write your expression: ");
                    string input = Console.ReadLine();
                    input.Replace(" ", string.Empty);

                    var tokens = Tokenize(input);
                    var rpn = ConvertToRPN(tokens);
                    Console.WriteLine($"Result: {CalculateRPN(rpn)}");
                    Console.ReadLine();
                }
            }

            // Method convert string into tokens list
            static List<Token> Tokenize(string input)
            {
                List<Token> tokens = new List<Token>();
                string number = string.Empty;
                foreach (var c in input)
                {
                    if (char.IsDigit(c))
                    {
                        number += c;
                    }
                    else if (c == ',' || c == '.')
                    {
                        number += ",";
                    }

                    else if (c == '+' || c == '-' || c == '*' || c == '/')
                    {
                        if (number != string.Empty)
                        {
                            tokens.Add(new Number(double.Parse(number)));
                            number = string.Empty;
                        }
                        tokens.Add(new Operation(c));
                    }
                    else if (c == '(' || c == ')')
                    {
                        if (number != string.Empty)
                        {
                            tokens.Add(new Number(double.Parse(number)));
                            number = string.Empty;
                        }
                        tokens.Add(new Parenthesis(c));
                    }
                }

                if (number != string.Empty)
                {
                    tokens.Add(new Number(double.Parse(number)));
                }

                return tokens;
            }
        }

        // Method to convert tokens list into RRN
        static List<Token> ConvertToRPN(List<Token> tokens)
        {
            // Realization
            List<Token> rpnOutput = new List<Token>();
            Stack<Token> operators = new Stack<Token>();
            string number = string.Empty;

            foreach (Token token in tokens)
            {
                if (operators.Count == 0 && !(token is Number))
                {
                    operators.Push(token);
                    continue;
                }

                if (token is Operation)
                {
                    if (operators.Peek() is Parenthesis)
                    {
                        operators.Push(token);
                        continue;
                    }

                    Operation first = (Operation)token;
                    Operation second = (Operation)operators.Peek();

                    if (first.Priority > second.Priority)
                    {
                        operators.Push(token);
                    }
                    else if (first.Priority <= second.Priority)
                    {
                        while (operators.Count > 0 && !(token is Parenthesis))
                        {
                            rpnOutput.Add(operators.Pop());
                        }
                        operators.Push(token);
                    }
                }
                else if (token is Parenthesis)
                {
                    if (((Parenthesis)token).isClosing)
                    {
                        while (!(operators.Peek() is Parenthesis))
                        {
                            rpnOutput.Add(operators.Pop());
                        }

                        operators.Pop();
                    }
                    else
                    {
                        operators.Push(token);
                    }
                }
                else if (token is Number)
                {
                    rpnOutput.Add(token);
                }
            }

            while (operators.Count > 0)
            {
                rpnOutput.Add(operators.Pop());
            }
            return rpnOutput;
        }

        // Result in RPN
        static double CalculateRPN(List<Token> rpnTokens)
        {
            Stack<double> binCalculator = new Stack<double>();
            double result = 0;

            for (int i = 0; i < rpnTokens.Count; i++)
            {
                if (rpnTokens[i] is Number value)
                {
                    binCalculator.Push(value.Value);
                }
                else
                {
                    double number1 = binCalculator.Pop();
                    double number2 = binCalculator.Pop();

                    var oper = (Operation)rpnTokens[i];
                    result = secondOperation(oper, number1, number2);
                    binCalculator.Push(result);
                }
            }
            return binCalculator.Peek();
        }
    }
}
