using System;
using WebAssembly.Runtime;

namespace BlazingTheWeb.UsingWasmInCSharp
{
	public static class Program
	{
		static void Main()
		{
			var module = Compile.FromBinary<Collatz>(
				@"..\..\..\..\BlazingTheWeb.WebAssembly\wwwroot\collatz.wasm");
			using var instance = module(new ImportDictionary());
			var functions = instance.Exports;
			// 10, 5, 16, 8, 4, 2, 1 => 6 iterations
			Console.Out.WriteLine(functions.collatz(10));
		}
	}
}