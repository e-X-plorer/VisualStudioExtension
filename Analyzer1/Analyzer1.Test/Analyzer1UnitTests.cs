using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;
using Analyzer1;

namespace Analyzer1.Test
{
    [TestClass]
    public class UnitTest : CodeFixVerifier
    {

        //No diagnostics expected to show up
        [TestMethod]
        public void ZeroInputTest()
        {
            var test = @"";

            VerifyCSharpDiagnostic(test);
        }

        //No Diagnostics expected to show up
        [TestMethod]
        public void EmptyClassTest()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
        }
    }";
            VerifyCSharpDiagnostic(test);
        }

        //No Diagnostics expected to show up
        [TestMethod]
        public void SmallClassTest()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {   
            public const int var1 = 1;
            public const int var2 = 2;
            private Command1(AsyncPackage package, OleMenuCommandService commandService)
            {
                this.package = package ?? throw new ArgumentNullException(nameof(package));
                commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.Execute, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }
    }";
            VerifyCSharpDiagnostic(test);
        }
        
        //Diagnostic triggered and checked for
        //NOT WORKING
        [TestMethod]
        public void BigClassTest()
        {
            var test = @"
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;
    using System.Diagnostics;

    namespace ConsoleApplication1
    {
        class TypeName
        {
            public const int var1 = 1;
            public const int var2 = 2;
            public const int var3 = 3;
            public const int var4 = 4;
            public const int var5 = 5;
            public const int var6 = 6;
            public const int var7 = 7;
            public const int var8 = 8;
            public const int var9 = 9;
            public const int var10 = 10;
            private Command1(AsyncPackage package, OleMenuCommandService commandService)
            {
                this.package = package ?? throw new ArgumentNullException(nameof(package));
                commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.Execute, menuCommandID);
                commandService.AddCommand(menuItem);
            }
        }
    }";
            var expected = new DiagnosticResult
            {
                Id = "ClassLength",
                Message = @"TypeName contains 7 lines or more than 11 children. Children: public const int var1 = 1;
public const int var2 = 2;
public const int var3 = 3;
public const int var4 = 4;
public const int var5 = 5;
public const int var6 = 6;
public const int var7 = 7;
public const int var8 = 8;
public const int var9 = 9;
public const int var10 = 10;
private Command1(AsyncPackage package, OleMenuCommandService commandService)
            {
                this.package = package ?? throw new ArgumentNullException(nameof(package));
                commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.Execute, menuCommandID);
                commandService.AddCommand(menuItem);
            }
",
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 11, 15)
                        }
            };
        
  //        VerifyCSharpDiagnostic(test, expected);

  //         var fixtest = @"
  // using System;
  // using System.Collections.Generic;
  // using System.Linq;
  // using System.Text;
  // using System.Threading.Tasks;
  // using System.Diagnostics;
  //
  // namespace ConsoleApplication1
  // {
  //     class TYPENAME
  //     {   
  //     }
  // }";
  //         VerifyCSharpFix(test, fixtest);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new Analyzer1CodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new Analyzer1Analyzer();
        }
    }
}
