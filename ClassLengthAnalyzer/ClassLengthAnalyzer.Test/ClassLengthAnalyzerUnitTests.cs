using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.CodeFixes;
using Microsoft.CodeAnalysis.Diagnostics;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using TestHelper;

namespace ClassLengthAnalyzer.Test
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

        //Diagnostic triggered on too many members
        [TestMethod]
        public void ManyMembersTest()
        {
            var test = @"
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
                Id = "ClassTooLong",
                Message = String.Format("Class {0} contains {1}. Maximum allowed is {2}. Class can be divided.", "TypeName", "11 members", "5"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 4, 15)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);
        }

        //Diagnostic triggered on too many lines
        [TestMethod]
        public void ManyLinesTest()
        {
            var test = @"
    namespace ConsoleApplication1
    {
        class TypeName
        {
            public const int var1 = 1;
            public const int var2 = 2;
            public const int var3 = 3;
            private Command1(AsyncPackage package, OleMenuCommandService commandService)
            {
                this.package = package ?? throw new ArgumentNullException(nameof(package));
                commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.Execute, menuCommandID);
                commandService.AddCommand(menuItem);

                this.package = package ?? throw new ArgumentNullException(nameof(package));
                commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.Execute, menuCommandID);
                commandService.AddCommand(menuItem);

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
                Id = "ClassTooLong",
                Message = String.Format("Class {0} contains {1}. Maximum allowed is {2}. Class can be divided.", "TypeName", "25 lines", "15"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 4, 15)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

        }

        //Diagnostic triggered on too many members
        [TestMethod]
        public void ManyMembersManyLinesTest()
        {
            var test = @"
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

                this.package = package ?? throw new ArgumentNullException(nameof(package));
                commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));
                var menuCommandID = new CommandID(CommandSet, CommandId);
                var menuItem = new MenuCommand(this.Execute, menuCommandID);
                commandService.AddCommand(menuItem);

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
                Id = "ClassTooLong",
                Message = String.Format("Class {0} contains {1}. Maximum allowed is {2}. Class can be divided.", "TypeName", "11 members", "5"),
                Severity = DiagnosticSeverity.Warning,
                Locations =
                    new[] {
                            new DiagnosticResultLocation("Test0.cs", 4, 15)
                        }
            };

            VerifyCSharpDiagnostic(test, expected);

        }

        //Diagnostic triggered on too many members, fix applied, updated and created files checked
        [TestMethod]
        public void ManyMembersFixTest()
        {
            var test = @"
    namespace ConsoleApplication1
    {
        class TypeName
        {
            public int x1;
            public int x2;
            public int x3;
            public int f1()
            {










            }
            public int f2()
            {
            }
        }
    }";
            var testUpdated = @"
    namespace ConsoleApplication1
    {
    partial class TypeName
        {
            public int x1;
            public int x2;
            public int x3;
        }
    }";
            var testCreated = @"namespace ConsoleApplication1
{
    partial class TypeName
        {
            public int f1()
            {










            }
            public int f2()
            {
            }
        }
}";
            VerifyCSharpCreatedFix(test, testUpdated, testCreated);
        }

        protected override CodeFixProvider GetCSharpCodeFixProvider()
        {
            return new MakePartialCodeFixProvider();
        }

        protected override DiagnosticAnalyzer GetCSharpDiagnosticAnalyzer()
        {
            return new ClassLengthAnalyzer();
        }
    }
}
