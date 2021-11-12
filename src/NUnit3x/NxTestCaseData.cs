using NUnit.Framework;
using NUnit.Framework.Interfaces;
using NUnit.Framework.Internal.Builders;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace NUnit3x
{
    public class NxTestCaseData : TestCaseData
    {
        #region Construction

        public NxTestCaseData(params object[] args)
            : base(args)
        {
            try
            {
                this.SetName(this.HandleTestName($"{{m}}{ RenameParameters(args) }"));
            }
            catch
            {
                this.SetName(this.HandleTestName("{m}"));
            }
        }

        #endregion

        #region Fields

        private const string MOCK_NAMESPACE = "Castle.Proxies";
        private const int MAX_LENGTH = 449;

        private static int _testId = 0;

        #endregion

        #region Methods

        public static IEnumerable<NxTestCaseData> Combinatorial(params IEnumerable[] args)
        {
            if (args != null)
            {
                CombinatorialStrategy strategy = new CombinatorialStrategy();
                foreach (ITestCaseData data in strategy.GetTestCases(args))
                {
                    yield return new NxTestCaseData(data.Arguments);
                }
            }
        }

        public new NxTestCaseData Returns(object obj) => (NxTestCaseData)base.Returns(obj);

        private string RenameParameters(params object[] args)
        {
            StringBuilder result = new StringBuilder();
            result.Append("(");

            if (args == null)
                result.Append("null");

            StringBuilder parameters = new StringBuilder();
            foreach (object obj in args)
            {
                parameters.Append(RenameParameter(obj));

                // Is this the last parameter?
                if (Array.IndexOf(args, obj) < (args.Length - 1))
                {
                    parameters.Append(", ");
                }
            }

            // The NUnitTestAdapter cannot handle inner parenthesis - just use curly braces instead
            parameters = parameters.Replace("(", "{").Replace(")", "}");

            result.Append(parameters);
            result.Append(")");

            return result.ToString();
        }

        private string RenameParameter(object obj)
        {
            string result;

            if (obj == null)
            {
                result = "null";
            }
            else if (obj is string)
            {
                if (string.IsNullOrWhiteSpace(obj.ToString()))
                    result = "string.Empty";
                else
                    result = $"\"{ obj }\"";
            }
            else
            {
                result = obj.ToString();

                if (IsMock(obj))
                {
                    if (result.Contains("Mock<"))
                    {
                        result = result.Replace("Mock<", string.Empty);

                        if (result.Contains(":"))
                        {
                            result = result.Substring(0, result.IndexOf(':'));
                        }
                    }
                }
                else if (obj as IEnumerable != null){
                    result = HandleEnumerableParameter(result, obj as IEnumerable);
                }
            }

            return result;
        }

        private string HandleEnumerableParameter(string root, IEnumerable value)
        {
            string result = root;

            if (result.Contains('['))
            {
                result = result.Substring(result.Substring(0, result.IndexOf('[') + 1).LastIndexOf('.') + 1);

                int start = result.IndexOf('[') + 1;
                int end = result.LastIndexOf(']');

                result = result.Remove(start, end - start);
            }
            else
            {
                result = result.Substring(result.LastIndexOf('.') + 1);
            }

            while (Regex.Matches(result, @"\[\]").Count > 1)
            {
                result = result.Remove(result.IndexOf("[]"), 2);
            }

            result = result.Insert(result.LastIndexOf("[") + 1, string.Join(", ", value.Cast<object>().Select(i => RenameParameter(i))));

            return result;
        }

        private bool IsMock(object obj)
        {
            return obj.GetType().Namespace.Equals(MOCK_NAMESPACE);
        }

        private string HandleTestName(string name)
        {
            string testId = $"[{ Interlocked.Increment(ref _testId) }]";

            if ((name.Length + testId.Length) > MAX_LENGTH)
            {
                string truncated = $"{ name.Substring(0, (MAX_LENGTH - testId.Length - 4))}";

                return $"{ truncated }...){ testId }";
            }

            return $"{ name }{ testId }";
        }

        #endregion
    }
}
