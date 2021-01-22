# Blazing the Web - Building Web Applications in C#
This repository contains the demo code used in the presentation. Here's a rundown of each project and what they do. Note that all of the projects are targeting .NET 5.0.

## BlazingTheWeb.WebAssembly
This is an ASP.NET app that is a vehicle for HTML pages that call a .wasm component. To build the code in `collatz.c`, I used [WebAssembly Explorer](https://mbebenita.github.io/WasmExplorer/).

## BlazingTheWeb.Core
This is a class library that contains an implementation of the [Collatz Conjecture](https://en.wikipedia.org/wiki/Collatz_conjecture). This project is used in other projects within the solution to perform calculations and create data.

## BlazingTheWeb.Core.Tests
Contains tests for `BlazingTheWeb.Core`. All of the test projects use [NUnit](https://nunit.org/).

## BlazingTheWeb.ServiceHost
This is a console app that hosts a [gRPC](https://grpc.io/) service to run the Collatz Conjecture.

## BlazingTheWeb.WebComponents
This is a class library that houses Razor components that are used in Blazor Server-Side and WebAssembly projects. If you want to run the `SequenceGrpc` component from the host applications, make sure the `BlazingTheWeb.ServiceHost` project is up and running.

## BlazingTheWeb.WebComponents.Tests
This contains tests for `BlazingTheWeb.WebComponents`. [bunit](https://bunit.egilhansen.com/) is used to drive component testing. I used to use [Microsoft.AspNetCore.Components.Testing](https://github.com/SteveSandersonMS/BlazorUnitTestingPrototype), but that hasn't been updated in a while, and it's unclear if it will ever become production-ready.

## BlazingTheWeb.ServerSideHost
This is a server-side-based Blazor application that uses the components in `BlazingTheWeb.WebComponents`.

## BlazingTheWeb.WebAssemblyHost
This is a WebAssembly-based Blazor application that uses the components in `BlazingTheWeb.WebComponents`.

## BlazingTheWeb.SeleniumTests
This is a console app that drives `BlazingTheWeb.WebAssemblyHost` from the browser. This requires the necessary Chrome driver from [Selenium](https://www.selenium.dev/) installed on your machine correctly for this to work.

## BlazingTheWeb.UsingWasmInCSharp
This is a console app that uses [WebAssembly for .NET](https://github.com/RyanLamansky/dotnet-webassembly) to translate the .wasm file from `BlazingTheWeb.WebAssembly` to a .NET assembly and invoke its' methods.