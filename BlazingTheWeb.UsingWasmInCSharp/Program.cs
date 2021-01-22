using BlazingTheWeb.UsingWasmInCSharp;
using System;
using System.Collections.Generic;
using WebAssembly.Runtime;

RunCollatz();
RunCollatzWithCallback();

static void RunCollatz()
{
	var module = Compile.FromBinary<Collatz>(
		@"..\..\..\..\BlazingTheWeb.WebAssembly\wwwroot\collatz.wasm");
	using var instance = module(new ImportDictionary());
	var functions = instance.Exports;
	// 10, 5, 16, 8, 4, 2, 1 => 6 iterations
	Console.Out.WriteLine(functions.collatz(10));
}

static void RunCollatzWithCallback()
{
	var values = new List<int>();
	var module = Compile.FromBinary<CollatzWithCallback>(
		@"..\..\..\..\BlazingTheWeb.WebAssembly\wwwroot\collatzWithCallback.wasm");
	var imports = new ImportDictionary
	{
		{ "imports", "collatzCallback", new FunctionImport(new Action<int>(value => values.Add(value))) }
	};
	using var instance = module(imports);
	var functions = instance.Exports;
	// 10 => "5, 16, 8, 4, 2, 1"
	functions.collatz(10);
	Console.Out.WriteLine(string.Join(", ", values));
}