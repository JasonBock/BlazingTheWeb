using System;
using System.Collections.Generic;
using WebAssembly.Runtime;

namespace BlazingTheWeb.UsingWasmInCSharp
{
	public static class Program
	{
		static void Main()
		{
			Program.RunCollatz();
			Program.RunCollatzWithCallback();
		}

		private static void RunCollatz()
		{
			var module = Compile.FromBinary<Collatz>(
				@"..\..\..\..\BlazingTheWeb.WebAssembly\wwwroot\collatz.wasm");
			using var instance = module(new ImportDictionary());
			var functions = instance.Exports;
			// 10, 5, 16, 8, 4, 2, 1 => 6 iterations
			Console.Out.WriteLine(functions.collatz(10));
		}

		private static void RunCollatzWithCallback()
		{
			var values = new List<int>();
			var module = Compile.FromBinary<dynamic>(
				@"..\..\..\..\BlazingTheWeb.WebAssembly\wwwroot\collatzWithCallback.wasm");
			var imports = new ImportDictionary
			{
				{ "imports", "collatzCallback", new FunctionImport(new Action<int>(value => values.Add(value))) }
			};
			using var instance = module(imports);
			var functions = instance.Exports;
			// 10, 5, 16, 8, 4, 2, 1 => 6 iterations
			functions.collatz(10);
			Console.Out.WriteLine(string.Join(", ", values));
		}
	}
}