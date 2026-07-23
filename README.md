<div align="center">

# FreakyKit.Utils

**A lightweight, dependency-free collection of C# extension methods for .NET**

<a href="https://www.nuget.org/packages/FreakyKit.Utils"><img src="https://img.shields.io/nuget/v/FreakyKit.Utils?color=blue&logo=nuget&style=for-the-badge" alt="FreakyKit.Utils NuGet version"></a>
<a href="https://www.nuget.org/packages/FreakyKit.Utils"><img src="https://img.shields.io/nuget/dt/FreakyKit.Utils?style=for-the-badge" alt="FreakyKit.Utils download count"></a>
<a href="./LICENSE"><img src="https://img.shields.io/github/license/freakyali/FreakyKit.Utils?style=for-the-badge" alt="FreakyKit.Utils license"></a>
<a href="https://www.codefactor.io/repository/github/freakyali/FreakyKit.Utils"><img src="https://img.shields.io/codefactor/grade/github/freakyali/FreakyKit.Utils?style=for-the-badge" alt="FreakyKit.Utils Codefactor grade"></a>

<br/>

| .NET | Namespace | Dependencies |
| :---: | :---: | :---: |
| 8.0+ | `FreakyKit.Utils` | none |

<br/>

</div>

---

## Highlights

> Extension methods only — drop the package in, add a `using`, every helper below becomes available on the receiver type.

<div align="center">

| Area | What you get |
| :--- | :--- |
| **Arrays** | Multi-dimensional traversal with element-wise callbacks. |
| **Collections** | `AddRange` / `RemoveRange` for any `ICollection<T>`. |
| **Commands** | `CanExecute`-safe `ICommand` invocation. |
| **DateTime** | Weekday / weekend predicates and `NextWorkday`. |
| **Enumerables** | `WithIndex`, `DistinctBy`, null-safe defaults, `Shuffle`, `ToObservable`. |
| **Exceptions** | Full `InnerException` chain → `Trace`. |
| **Lists** | `RemoveAll`, `InsertWhere`, key-projected `BinarySearch`. |
| **Numbers** | Generic `IsBetween` for any `INumber<T>`. |
| **Objects** | Cloning, type-test sugar, JSON / XML helpers, structural compare. |
| **DI** | Strongly-typed `IServiceProvider.GetService<T>()`. |
| **Streams** | `Stream` → `MemoryStream` / Base64. |
| **Strings** | Base64, regex strip, alphanumeric / email validation, currency, reverse. |
| **Tasks** | Aggregate-exception `WhenAll`, fire-and-forget, `TimeoutAfter`. |

</div>

---

## Installation

```bash
dotnet add package FreakyKit.Utils
```

Or via Package Manager Console:

```powershell
Install-Package FreakyKit.Utils -Version xx.xx.xx
```

### Initialization

Add a single `using` and every extension below becomes available:

```csharp
using FreakyKit.Utils;

// Array: traverse a multi-dimensional array
matrix.ForEach((array, position) => Console.WriteLine(array.GetValue(position)));

// Enumerable: iterate with index
foreach (var (item, idx) in items.WithIndex()) { /* ... */ }

// String: validate email
bool ok = "test@example.com".IsValidEmail();

// Number: range check
bool inRange = age.IsBetween(18, 65);

// Task: bound with a timeout
await task.TimeoutAfter(TimeSpan.FromSeconds(10));
```

---

## Documentation

Full API docs for every extension class live in the [`docs/`](./docs/) folder.

---

### Like what you saw? Want to keep this repo alive?

<div align="center">

[![Buy Me A Coffee](https://miro.medium.com/max/600/0*wrBJU05A3BULKcWA.gif)](https://www.buymeacoffee.com/FreakyAli)

</div>

---

## License

[MIT](https://github.com/FreakyAli/FreakyKit.Utils/blob/master/LICENSE)

[![FOSSA Status](https://app.fossa.com/api/projects/git%2Bgithub.com%2FFreakyAli%2FFreakyKit.Utils.svg?type=large)](https://app.fossa.com/projects/git%2Bgithub.com%2FFreakyAli%2FFreakyKit.Utils?ref=badge_large)

---

## Activity

<div align="center">

[![Star History Chart](https://api.star-history.com/svg?repos=FreakyAli/FreakyKit.Utils&type=Date)](https://star-history.com/#FreakyAli/FreakyKit.Utils&type=Date)

![Alt](https://repobeats.axiom.co/api/embed/c1f79493ade6fb1939b12493d25aa4c5f5362005.svg "Repobeats analytics image")

</div>
