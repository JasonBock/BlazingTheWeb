using System;
using WebAssembly.Runtime;

namespace BlazingTheWeb.UsingWasmInCSharp
{
	public static class Program
	{
		static void Main()
		{
			var module = Compile.FromBinary<dynamic>(
				@"..\..\..\..\BlazingTheWeb.WebAssembly\wwwroot\collatz.wasm");
			var functions = module(new ImportDictionary()).Exports;
			// 10, 5, 16, 8, 4, 2, 1 => 6 iterations
			Console.Out.WriteLine(functions.collatz(10));
		}
	}
}