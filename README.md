# FreakyKit.Utils

A robust and lightweight collection of C# extension methods designed to simplify common programming patterns and utility operations in .NET projects. This library includes extensions for arrays, collections, commands, dates, enumerables, exceptions, lists, numbers, objects, dependency injection, streams, strings, and tasks.

## Features

- Array Extensions: Multi-dimensional array traversal and element-wise actions.
- Collection Extensions: Simplified range add/remove operations for any collection.
- Command Extensions: Safe WPF `ICommand` pattern execution.
- DateTime Extensions: Weekday/weekend checks and workday calculations.
- Enumerable Extensions: Functional utilities like `WithIndex`, `DistinctBy`, safe list methods, shuffling, and more.
- Exception Extensions: Deep exception tracing to system logs.
- List Extensions: High-performance removal, insertion, and binary search.
- Number Extensions: Generic range checks for all number types.
- Object Extensions: Cloning, safe type conversions, JSON/XML utilities, and structural comparison.
- ServiceProvider Extension: Strongly-typed DI service fetching.
- Stream Extensions: Stream conversions and Base64 encoding/decoding.
- String Extensions: Text manipulations, Base64, reverse, currency, and validation utilities.
- Task Extensions: WhenAll with aggregate exceptions, safe result retrieval, and task timeout support.

## Installation

Add the compiled DLL to your project, or copy the relevant extension files into your solution.

```
dotnet add package FreakyKit.Utils
```

## Usage

Add a using directive to the `FreakyKit.Utils` namespace:

```
using FreakyKit.Utils;

// Array: Traverse a 2D array
matrix.ForEach((array, position) => Console.WriteLine(array.GetValue(position)));

// List: Remove items matching a predicate
myList.RemoveAll(x => x.IsObsolete);

// String: Validate email
bool isValid = "test@example.com".IsValidEmail();

// Enumerable: Enumerate with index
foreach (var (item, idx) in items.WithIndex()) { ... }

// Number: Check range
bool inRange = age.IsBetween(18, 65);

// DateTime: Find next workday
DateTime nextWorkday = today.NextWorkday();

// Task: Await with timeout
await task.TimeoutAfter(TimeSpan.FromSeconds(10));
```


For full API details, see the XML comments in each extension method.

## Documentation

- All extension classes are in the `FreakyKit.Utils` namespace.
- Methods use strong typing, safe defaults, and minimal dependencies.
- Most methods contain XML documentation accessible via IntelliSense.

I am also planning to either add the documentation here or to the wiki soon. 

## Contributing

Contributions are welcome! Please submit issues and PRs for bug reports, new utilities, or improvements. All code must follow C# standard naming conventions and include XML documentation.

## License

This project is released under the MIT License.


