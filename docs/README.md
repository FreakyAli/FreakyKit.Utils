# FreakyKit.Utils — API Reference

Full API reference for every extension class in `FreakyKit.Utils`.

**Target Framework:** .NET 8.0  ·  **Namespace:** `FreakyKit.Utils`

---

## Contents

- [ArrayExtensions](#arrayextensions)
- [BytesExtensions](#bytesextensions)
- [CharExtensions](#charextensions)
- [CollectionExtensions](#collectionextensions)
- [CommandExtensions](#commandextensions)
- [DateTimeExtensions](#datetimeextensions)
- [DictionaryExtensions](#dictionaryextensions)
- [EnumExtensions](#enumextensions)
- [EnumerableExtensions](#enumerableextensions)
- [ExceptionExtensions](#exceptionextensions)
- [GuidExtensions](#guidextensions)
- [ListExtensions](#listextensions)
- [NumberExtensions](#numberextensions)
- [ObjectExtensions](#objectextensions)
- [RandomExtensions](#randomextensions)
- [ServiceProvider](#serviceprovider)
- [StreamExtensions](#streamextensions)
- [StringExtensions](#stringextensions)
- [TaskExt](#taskext)
- [TimeSpanExtensions](#timespanextensions)
- [TypeExtensions](#typeextensions)
- [UriExtensions](#uriextensions)

---

## ArrayExtensions

Element-wise traversal helpers for multi-dimensional `System.Array` instances.

### Usage

```csharp
using FreakyKit.Utils;

var matrix = new int[,]
{
    { 1, 2, 3 },
    { 4, 5, 6 },
};

matrix.ForEach((array, position) =>
{
    Console.WriteLine($"[{string.Join(",", position)}] = {array.GetValue(position)}");
});
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `ForEach(Action<Array,int[]> action)` | `void` | Walks every element of any-rank array and invokes `action(array, position)` per cell. No-op for empty arrays. |
| `Fill<T>(T value)` | `void` | Sets every element of a 1-D array to `value`. Wraps `Array.Fill`. |

---

## CollectionExtensions

Range add/remove helpers for any `ICollection<T>` (including `ObservableCollection<T>`, where the BCL omits these).

### Usage

```csharp
using FreakyKit.Utils;

var collection = new ObservableCollection<Animal>();
collection.AddRange(new Dog(), new Cat(), new Fish());

collection.RemoveRange(dog, cat);
```

Both methods accept a covariant subtype: `S : T`, so you can pass `Dog[]` into `ICollection<Animal>`.

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `AddRange<T, S>(params S[] values)` where `S : T` | `void` | Appends each item to the collection in order. |
| `RemoveRange<T, S>(params S[] values)` where `S : T` | `void` | Calls `Remove` for each item. Silently skips items not present. |
| `RemoveWhere<T>(Func<T, bool> predicate)` | `int` | Removes every element matching `predicate`. Returns the count removed. Delegates to `List<T>.RemoveAll` when possible. |
| `Replace<T>(T oldItem, T newItem)` | `bool` | Replaces the first occurrence; preserves position for `IList<T>`. Returns `false` when `oldItem` is not found. |

---

## CommandExtensions

Null- and `CanExecute`-safe invocation for any `System.Windows.Input.ICommand`.

### Usage

```csharp
using FreakyKit.Utils;

// Equivalent to:
//   if (saveCommand?.CanExecute(file) == true) saveCommand.Execute(file);
saveCommand.ExecuteWhenAvailable(file);

// Works with null commands — no NRE.
ICommand? notWired = null;
notWired.ExecuteWhenAvailable();
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `ExecuteWhenAvailable(object? parameter = null)` | `void` | Invokes `Execute(parameter)` only if `command` is non-null and `CanExecute(parameter)` returns `true`. |

---

## DateTimeExtensions

Weekday/weekend predicates and Mon–Fri workday navigation.

> Workday logic uses Saturday + Sunday as the weekend. Locale-specific weekends (e.g. Fri/Sat in parts of MENA) and bank holidays are not considered.

### Usage

```csharp
using FreakyKit.Utils;

var today = DateTime.Today;

if (today.IsWeekend())
    Console.WriteLine("Take it easy.");

DateTime nextOpenDay = today.NextWorkday();
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `IsWeekDay()` | `bool` | `true` for Monday through Friday. |
| `IsWeekend()` | `bool` | `true` for Saturday and Sunday. |
| `NextWorkday()` | `DateTime` | Returns the receiver if it's a weekday; otherwise advances day-by-day until the next weekday. |
| `PreviousWorkday()` | `DateTime` | Counterpart to `NextWorkday` — rewinds to the previous weekday. |
| `StartOfDay()` / `EndOfDay()` | `DateTime` | Midnight / last tick of the same calendar day. Preserves `Kind`. |
| `StartOfWeek(DayOfWeek firstDay = Monday)` / `EndOfWeek(...)` | `DateTime` | Boundaries of the containing week. Defaults to Monday-aligned weeks. |
| `StartOfMonth()` / `EndOfMonth()` | `DateTime` | First / last day of the same calendar month. |
| `IsSameDay(DateTime other)` | `bool` | Date equality ignoring time-of-day. |
| `Age(DateTime? referenceDate = null)` | `int` | Whole years between receiver and reference (default `DateTime.Today`). |
| `IsToday()` / `IsYesterday()` / `IsTomorrow()` | `bool` | Convenience day comparisons against `DateTime.Today`. |
| `ToUnixTimeSeconds()` | `long` | Seconds since the Unix epoch. |

---

## EnumerableExtensions

Functional helpers that complement LINQ: indexed iteration, null-safe variants, defaulting selectors, shuffling, and an `ObservableCollection` projection.

### Usage

```csharp
using FreakyKit.Utils;

var items = new[] { "alpha", "beta", "gamma" };

foreach (var (item, index) in items.WithIndex())
    Console.WriteLine($"{index}: {item}");

ObservableCollection<string> bound = items.ToObservable();

if (items.IsNullOrEmpty()) return;

var unique = people.DistinctBy(p => p.Email);

var safe = maybeNull.EmptyIfNull();

var first = items.FirstOrDefault(x => x.Length > 10, theDefault: "n/a");

var shuffled = items.Shuffle();
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `ToObservable<T>()` | `ObservableCollection<T>` | Materializes the sequence into an `ObservableCollection<T>`. |
| `WithIndex<T>()` | `IEnumerable<(T item, int index)>` | Pairs each item with its zero-based index. Returns empty enumerable for null input. |
| `IsNullOrEmpty<T>()` | `bool` | `true` when the sequence is `null` or contains no elements. Uses `ICollection<T>.Count` when available. |
| `DistinctBy<TSource, TKey>(Func<TSource, TKey> keySelector)` | `IEnumerable<TSource>` | Deduplicates elements by a projected key. Preserves source order. |
| `ForEach<T>(Action<T> action)` | `void` | Invokes `action` for each element. |
| `SingleOrDefault<T>(Func<T, bool> predicate, T theDefault)` | `T` | Null-safe overload that returns `theDefault` when the source is null. |
| `FirstOrDefault<T>(Func<T, bool> predicate, T theDefault)` | `T` | Null-safe overload that returns `theDefault` when the source is null. |
| `ElementAtOrDefault<T>(int index, T theDefault)` | `T` | Null-/range-safe overload that returns `theDefault` for null source or invalid index. |
| `EmptyIfNull<T>()` | `IEnumerable<T>` | Returns the source if non-null, otherwise an empty sequence. |
| `Shuffle<T>()` | `IEnumerable<T>` | Returns a uniformly-shuffled sequence using Fisher–Yates. Throws `ArgumentNullException` for null source. |
| `WhereNotNull<T>()` | `IEnumerable<T>` | Two overloads (class / struct) — drops `null` / `Nullable<T>` nulls from the sequence. |
| `JoinString<T>(string separator)` | `string` | Sugar for `string.Join(separator, source)`. |
| `IndexOf<T>(Func<T, bool> predicate)` | `int` | First matching index, or `-1`. |
| `None<T>(Func<T, bool> predicate)` | `bool` | Inverse of `Any(predicate)`. Throws `ArgumentNullException` if source or predicate is null. |
| `Partition<T>(Func<T, bool> predicate)` | `(IReadOnlyList<T> matched, IReadOnlyList<T> unmatched)` | Splits the source by predicate. |
| `TakeRandom<T>(int count)` | `IEnumerable<T>` | Uniform random sample via reservoir sampling (O(n) time, O(count) space). Capped to source size. |

---

## ExceptionExtensions

Recursive exception-chain dumping to `System.Diagnostics.Trace`.

### Usage

```csharp
using FreakyKit.Utils;

try
{
    DoWork();
}
catch (Exception ex)
{
    ex.TraceException(); // Writes Message + StackTrace for ex and every InnerException.
    throw;
}
```

Output is routed via `Trace.TraceError`, so it shows in any active `TraceListener` (debug output, file, ETW, etc.).

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `TraceException()` | `void` | Walks the `InnerException` chain and appends `Message` + `StackTrace` for each, then emits a single `Trace.TraceError` call. |
| `GetRootCause()` | `Exception` | Drills to the innermost `InnerException`. |
| `GetAllMessages(string separator = " -> ")` | `string` | Concatenates the message of every exception in the chain. |

---

## ListExtensions

In-place removal, ordered insertion, and key-projected binary search over `IList<T>`.

### Usage

```csharp
using FreakyKit.Utils;

IList<Item> items = LoadItems();

items.RemoveAll(x => x.IsObsolete);

// Insert 8 at the first position where the predicate fails:
var sorted = new List<int> { 1, 2, 3, 4, 5, 10, 12 };
sorted.InsertWhere(8, x => 8 > x); // { 1, 2, 3, 4, 5, 8, 10, 12 }

var match = sortedUsers.BinarySearch(u => u.Id, targetId);
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `RemoveAll<T>(Predicate<T> predicate)` | `void` | Removes every item matching the predicate. Delegates to `List<T>.RemoveAll` when possible; otherwise compacts in-place. Throws `NotSupportedException` for `T[]`. |
| `InsertWhere<T>(T obj, Func<T, bool> predicate)` | `void` | Inserts `obj` at the first index where `predicate(list[i])` is `false`; otherwise appends. |
| `BinarySearch<T, TKey>(Func<T, TKey> keySelector, TKey key)` | `T` | Standard binary search on a key projection; the list must be sorted by `keySelector`. Throws `InvalidOperationException` when not found. |
| `Swap<T>(int i, int j)` | `void` | Swaps the elements at the two indices in place. Bounds-checked. |
| `Move<T>(int fromIndex, int toIndex)` | `void` | Removes at `fromIndex` and re-inserts at `toIndex`. |

---

## NumberExtensions

Generic, inclusive range check for any `INumber<T>`.

### Usage

```csharp
using FreakyKit.Utils;

if (age.IsBetween(18, 65)) { /* ... */ }

decimal amount = 49.99m;
bool inTier = amount.IsBetween(0m, 99.99m);

if (((double)temp).IsBetween(-10.0, 35.0)) { /* ... */ }
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `IsBetween<T>(T min, T max)` where `T : INumber<T>` | `bool` | Inclusive range check: `number >= min && number <= max`. Works for all numeric primitives. |
| `IsEven<T>()` / `IsOdd<T>()` where `T : IBinaryInteger<T>` | `bool` | Parity check for any integer type. |
| `Clamp<T>(T min, T max)` where `T : INumber<T>` | `T` | Instance form of `T.Clamp(min, max)`. |
| `RoundTo(int decimals)` | `double` / `decimal` | Wraps `Math.Round` / `decimal.Round`. Two overloads. |
| `Map<T>(T fromMin, T fromMax, T toMin, T toMax)` where `T : INumber<T>` | `T` | Linear remap between two numeric ranges. For integral types, overflow throws `OverflowException`. |

---

## ObjectExtensions

Deep cloning, type-testing sugar, JSON/XML (de)serialization, and structural comparison — all via `System.Text.Json` and `System.Xml.Serialization`.

> `Clone` and `CompareAsJson` only see state visible to `System.Text.Json`: public, JSON-serializable members. Private fields, cycles, and non-serializable types are not preserved/compared.

### Usage

```csharp
using FreakyKit.Utils;

var copy = order.Clone();

if (payload.Is<Customer>())
{
    var c = payload.As<Customer>();
}

string json = order.ToJson();
Order? back = json.FromJson<Order>();

string xml = config.XmlSerialize();
Config? cfg = xml.XmlDeserialize<Config>();

bool same = orderA.CompareAsJson(orderB);
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `Clone<T>()` | `T?` | Deep-clone via JSON round-trip. Only JSON-serializable public state is copied. |
| `Is<T>()` where `T : class` | `bool` | Sugar for `obj is T`. |
| `IsNot<T>()` where `T : class` | `bool` | Negation of `Is<T>`. |
| `As<T>()` where `T : class` | `T?` | Sugar for `obj as T`. |
| `ToJson<T>(JsonSerializerOptions? options = null)` | `string` | Serializes via `JsonSerializer.Serialize`. |
| `FromJson<T>(JsonSerializerOptions? options = null)` | `T?` | Deserializes via `JsonSerializer.Deserialize`. |
| `XmlSerialize<T>()` where `T : class, new()` | `string` | Serializes via `XmlSerializer` to a string. |
| `XmlDeserialize<T>()` where `T : class, new()` | `T?` | Deserializes via `XmlSerializer`. Returns `null` on failure (does not throw). |
| `CompareAsJson(object anotherObj)` | `bool` | Structural equality via JSON: same runtime type and matching serialized output (ordinal, case-insensitive). |

---

## ServiceProvider

Strongly-typed `GetService<T>()` over `IServiceProvider`, with no dependency on `Microsoft.Extensions.DependencyInjection`.

### Usage

```csharp
using FreakyKit.Utils;

IServiceProvider provider = BuildProvider();

ILogger? logger = provider.GetService<ILogger>();
```

Throws `ArgumentNullException` if `provider` is null. Returns `null` if the service isn't registered.

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `GetService<T>()` | `T?` | Typed wrapper around `IServiceProvider.GetService(typeof(T))`. |
| `GetRequiredService<T>()` where `T : notnull` | `T` | Like `GetService<T>` but throws `InvalidOperationException` when not registered. |

---

## StreamExtensions

Stream-to-`MemoryStream` and stream-to-Base64 conversions.

### Usage

```csharp
using FreakyKit.Utils;

using var fileStream = File.OpenRead("photo.jpg");

MemoryStream buffered = fileStream.GetMemoryStream();   // Position reset to 0.

string? base64 = buffered.GetBase64();
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `GetMemoryStream()` | `MemoryStream` | Copies the source stream into a new `MemoryStream` and rewinds it to position 0. |
| `GetBase64()` | `string?` | Returns the stream contents (from current Position to end) as a Base64-encoded string. Zero-copy via `TryGetBuffer` on `MemoryStream`; returns empty string if Position ≥ Length. Returns `null` for a null receiver. |
| `ToByteArray()` | `byte[]` | Reads the stream from current Position to end into a new byte array. Fast-path for `MemoryStream` via `TryGetBuffer`. |
| `ReadAllBytesAsync(CancellationToken token = default)` | `Task<byte[]>` | Async variant of `ToByteArray`; respects current Position. |

---

## StringExtensions

Base64, regex-based sanitization, alphanumeric/email validation, culture-aware currency formatting, and string reversal.

### Usage

```csharp
using FreakyKit.Utils;

string encoded = "hello".ToBase64();      // "aGVsbG8="
string decoded = encoded.FromBase64();    // "hello"

string slug = "file name#1!.txt".RemoveSpecialCharacters();
// "filename1.txt"

string filtered = "AB12-cd".RemoveUnwantedCharacters("[^A-Z]");
// "AB"

bool ok   = "abc123".IsAlphaNumeric();    // true
bool mail = "a@b.com".IsValidEmail();     // true

string usd = 1234.5.ToCurrency("en-US");  // "$1,234.50"
string eur = 1234.5.ToCurrency("de-DE");  // "1.234,50 €"

string reversed = "racecar".Reverse();    // "racecar"
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `ToBase64()` | `string` | UTF-8 → Base64. |
| `FromBase64()` | `string` | Base64 → UTF-8. Auto-pads with `=` if length is not a multiple of 4. |
| `RemoveUnwantedCharacters(string allowedCharactersRegEx)` | `string` | Removes every character matching the supplied regex. |
| `RemoveSpecialCharacters()` | `string` | Strips anything outside `0-9 a-z A-Z - _ .`. |
| `IsAlphaNumeric()` | `bool` | `true` if the string matches `^[a-zA-Z0-9]*$`. |
| `ToCurrency(this double value, string cultureName)` | `string` | Formats `value` using the specified culture's currency format (`{0:C}`). |
| `Reverse()` | `string` | Returns the input with characters in reverse order. |
| `IsValidEmail()` | `bool` | `true` if `new MailAddress(value.Trim())` succeeds. Lenient — accepts anything `MailAddress` accepts. |
| `Truncate(int maxLength, string ellipsis = "…")` | `string` | Cuts at `maxLength`; appends `ellipsis` if truncated. |
| `Repeat(int count)` | `string` | Concatenates the receiver `count` times. |
| `Left(int count)` / `Right(int count)` | `string` | Bounds-safe substring from the start / end. |
| `RemoveWhitespace()` | `string` | Strips every `char.IsWhiteSpace` character. |
| `SplitLines()` | `string[]` | Splits on `\r\n`, `\n`, or `\r`. |
| `EnsurePrefix(string prefix)` / `EnsureSuffix(string suffix)` | `string` | Adds the prefix / suffix only when not already present. |
| `ContainsIgnoreCase` / `EqualsIgnoreCase` / `StartsWithIgnoreCase` / `EndsWithIgnoreCase` | `bool` | Ordinal case-insensitive variants of the BCL string methods. |
| `IsValidGuid()` | `bool` | `true` if the string parses as a `Guid`. |
| `IsValidUrl()` | `bool` | `true` if the string parses as an absolute http / https URL. |

---

## TaskExt

Task lifecycle helpers: fire-and-forget starts, aggregate exception propagation, and `WhenAny`-based timeouts.

### Usage

```csharp
using FreakyKit.Utils;

// Start a cold task without awaiting it.
var task = new Task(DoWork);
task.RunConcurrently();

// WhenAll that surfaces every faulted task, not just the first.
var results = await TaskExt.WhenAll(LoadA(), LoadB(), LoadC());

// Force the aggregate exception (instead of the unwrapped first inner) to bubble.
await someTask.WithAggregateException();

// Bound a task with a hard deadline.
try
{
    var value = await DownloadAsync().TimeoutAfter(TimeSpan.FromSeconds(10));
}
catch (TimeoutException)
{
    // ...
}
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `RunConcurrently(this Task task)` | `void` | Starts the task only if its status is `Created`. Throws `ArgumentNullException` for null. |
| `WhenAll<T>(params Task<T>[] tasks)` | `Task<IEnumerable<T>>` | Like `Task.WhenAll`, but throws the aggregate (with every inner exception) rather than just the first. |
| `WithAggregateException(this Task source)` | `Task` | Re-throws the aggregate exception on failure. Preserves `OperationCanceledException`. |
| `WithAggregateException<T>(this Task<T> source)` | `Task<T>` | Generic overload of the above. |
| `TimeoutAfter<TResult>(this Task<TResult> task, TimeSpan timeout)` | `Task<TResult>` | Awaits the task; throws `TimeoutException` if the timeout elapses first. |
| `TimeoutAfter(this Task task, TimeSpan timeout)` | `Task` | Non-generic overload. Throws `TimeoutException` if the timeout elapses first. |
| `FireAndForget(this Task task, Action<Exception>? onException = null)` | `void` | Starts the task (if `Created`) and routes failures to the optional handler. Cancellation is treated as failure. |
| `Retry(this Func<Task> action, int maxAttempts, TimeSpan delay, Func<Exception, bool>? shouldRetry = null)` | `Task` | Retries the action on failure; preserves the exception's original stack trace. Re-throws the last exception when all attempts fail. Generic `Func<Task<T>>` overload also provided. |
| `WithCancellation(this Task task, CancellationToken token)` | `Task` | Awaits the task with external cancellation. Generic `Task<T>` overload also provided. |

---

## DictionaryExtensions

Atomic get-or-add patterns, merging, inversion, and read-only snapshots over `IDictionary<TKey, TValue>`.

### Usage

```csharp
using FreakyKit.Utils;

var counts = new Dictionary<string, int>();

counts.GetOrAdd("hits", 0);
counts.AddOrUpdate("hits", addValue: 1, updateValueFactory: (_, old) => old + 1);

var partial = new Dictionary<string, int> { ["misses"] = 5 };
counts.Merge(partial);

var byValue = counts.Invert();
var snapshot = counts.ToReadOnlyDictionary();
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `GetOrAdd<TKey, TValue>(TKey key, TValue value)` | `TValue` | Returns the existing value or stores and returns `value`. |
| `GetOrAdd<TKey, TValue>(TKey key, Func<TKey, TValue> factory)` | `TValue` | Lazy factory variant. |
| `AddOrUpdate<TKey, TValue>(TKey key, TValue addValue, Func<TKey, TValue, TValue> updateFactory)` | `TValue` | Adds when absent or replaces using the supplied factory. |
| `Merge<TKey, TValue>(IDictionary<TKey, TValue> other)` | `void` | Copies entries from `other`, overwriting on collision. |
| `Invert<TKey, TValue>()` | `Dictionary<TValue, TKey>` | Swaps keys and values. Throws if values are not unique. |
| `ToReadOnlyDictionary<TKey, TValue>()` | `ReadOnlyDictionary<TKey, TValue>` | Wraps a snapshot copy in `ReadOnlyDictionary`. |

---

## TypeExtensions

Reflection helpers: assignability, friendly generic names, attribute presence, nullability, and instantiability checks.

### Usage

```csharp
using FreakyKit.Utils;

bool stringIsObject = typeof(string).IsAssignableTo<object>();   // true
string name = typeof(Dictionary<string, int>).GetGenericTypeName(); // "Dictionary<String, Int32>"

if (typeof(MyDto).HasAttribute<SerializableAttribute>()) { /* ... */ }

bool isOptional = typeof(int?).IsNullable();         // true
object? zero = typeof(int).GetDefaultValue();        // 0
bool canNew = typeof(MyDto).IsConcrete();
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `IsAssignableTo<T>()` | `bool` | Inverse of `Type.IsAssignableFrom` — can this type be assigned to a `T`? |
| `GetGenericTypeName()` | `string` | Friendly name like `List<Int32>` for generic types. |
| `HasAttribute<TAttribute>(bool inherit = true)` | `bool` | Whether the type is decorated with the attribute. |
| `GetAttribute<TAttribute>(bool inherit = true)` | `TAttribute?` | First attribute instance or `null`. |
| `IsNullable()` | `bool` | `true` for `Nullable<T>`. |
| `GetDefaultValue()` | `object?` | Language-level default — `null` for refs, zero/empty for value types. |
| `IsConcrete()` | `bool` | Not abstract, not interface, not open generic. |

---

## UriExtensions

Query-string and URL manipulation built only on the BCL (`UriBuilder` + `System.Net.WebUtility`).

### Usage

```csharp
using FreakyKit.Utils;

var baseUri = new Uri("https://api.example.com/v1/resource");

var withQuery = baseUri
    .AppendQueryParameter("page", "2")
    .AppendQueryParameter("size", "50");

IReadOnlyDictionary<string, string> parsed = withQuery.GetQueryParameters();
// parsed["page"] == "2"

Uri normalized = baseUri.EnsureTrailingSlash();   // .../resource/
Uri clean = withQuery.WithoutQuery();

bool local = new Uri("http://localhost:5000").IsLocalhost(); // true
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `AppendQueryParameter(string key, string value)` | `Uri` | URL-encodes and appends `key=value`. |
| `GetQueryParameters()` | `IReadOnlyDictionary<string, string>` | Parses the query string into a dictionary. |
| `WithoutQuery()` | `Uri` | Returns a clone with the query string stripped. |
| `EnsureTrailingSlash()` | `Uri` | Adds `/` to the path if missing. |
| `IsLocalhost()` | `bool` | `true` for `localhost`, `127.0.0.1`, and `::1`. |

---

## GuidExtensions

Short-form GUID encoding (URL-safe Base64, 22 chars) and emptiness checks.

### Usage

```csharp
using FreakyKit.Utils;

var id = Guid.NewGuid();
string shortId = id.ToShortString();   // 22 chars, URL-safe

Guid back = shortId.ParseShortGuid();
bool ok = shortId.TryParseShortGuid(out var parsed);

bool empty = Guid.Empty.IsEmpty();      // true
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `ToShortString()` | `string` | 22-character URL-safe Base64 encoding of the GUID's bytes. |
| `ParseShortGuid()` | `Guid` | Decodes a short-form GUID. Throws `FormatException` on invalid input. |
| `TryParseShortGuid(out Guid)` | `bool` | TryParse variant. Returns `false` and `Guid.Empty` on failure. |
| `IsEmpty()` | `bool` | `true` when the GUID equals `Guid.Empty`. |

---

## BytesExtensions

Hex / Base64 / string conversions for `byte[]`.

### Usage

```csharp
using FreakyKit.Utils;
using System.Text;

byte[] data = [0xDE, 0xAD, 0xBE, 0xEF];

string hex = data.ToHex();              // "DEADBEEF"
byte[] back = hex.FromHex();

string b64 = data.ToBase64();
string text = Encoding.UTF8.GetBytes("hi").AsString();   // "hi"
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `ToHex()` | `string` | Uppercase hex. Wraps `Convert.ToHexString`. |
| `FromHex(this string hex)` | `byte[]` | Wraps `Convert.FromHexString`. Throws on invalid input. |
| `ToBase64()` | `string` | Wraps `Convert.ToBase64String`. |
| `AsString(Encoding? encoding = null)` | `string` | Decodes bytes as a string. Defaults to UTF-8. |

---

## RandomExtensions

Convenience generators on top of `System.Random`.

### Usage

```csharp
using FreakyKit.Utils;

var rnd = new Random();

bool coin = rnd.NextBool();
DayOfWeek day = rnd.NextEnum<DayOfWeek>();
int pick = rnd.NextElement(new[] { 1, 2, 3, 4, 5 });
string id = rnd.NextString(10);                   // alphanumeric
string pin = rnd.NextString(6, "0123456789");     // digits only
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `NextBool()` | `bool` | 50/50 boolean. |
| `NextEnum<T>()` where `T : struct, Enum` | `T` | Uniformly random enum value. |
| `NextElement<T>(IReadOnlyList<T> source)` | `T` | Random element from a non-empty list. |
| `NextString(int length, string? alphabet = null)` | `string` | Random string drawn from `alphabet` (default A–Z, a–z, 0–9). |

---

## CharExtensions

Minor character predicates and a `Repeat` helper.

### Usage

```csharp
using FreakyKit.Utils;

bool vowel = 'a'.IsVowel();           // true
bool cons = 'b'.IsConsonant();        // true
string line = '-'.Repeat(40);         // 40 dashes
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `IsVowel()` | `bool` | English vowel (a, e, i, o, u), case-insensitive. |
| `IsConsonant()` | `bool` | Letter that is not a vowel. |
| `Repeat(int count)` | `string` | Returns the character repeated `count` times. |

---

## EnumExtensions

Description metadata, definition checks, and string → enum parsing.

### Usage

```csharp
using FreakyKit.Utils;
using System.ComponentModel;

enum Status
{
    [Description("On the way")] InTransit,
    Delivered
}

string label = Status.InTransit.GetDescription();   // "On the way"
bool defined = Status.Delivered.IsDefined();         // true

Status s = "delivered".ToEnum<Status>();             // case-insensitive
bool ok = "InTransit".TryToEnum<Status>(out var v);
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `GetDescription()` | `string` | Value of `[Description]` attribute or the member name. |
| `IsDefined()` | `bool` | Whether the value is a declared member of its enum type. |
| `ToEnum<TEnum>(this string value)` | `TEnum` | Case-insensitive parse; rejects undefined numeric values; throws on invalid input. |
| `TryToEnum<TEnum>(this string value, out TEnum result)` | `bool` | TryParse variant; rejects undefined numeric values. |

---

## TimeSpanExtensions

Min / Max selectors and a compact human-readable formatter.

### Usage

```csharp
using FreakyKit.Utils;

var timeout = TimeSpan.FromSeconds(30);
var configured = TimeSpan.FromSeconds(10);

var effective = timeout.Min(configured);   // 10s

string label = TimeSpan.FromMinutes(90).ToHumanString();   // "1h 30m"
string short_  = TimeSpan.FromSeconds(5).ToHumanString();  // "5s"
string neg    = TimeSpan.FromMinutes(-90).ToHumanString(); // "-1h 30m"
```

### Methods

| Method | Returns | Description |
| --- | --- | --- |
| `Min(TimeSpan other)` | `TimeSpan` | The smaller of the two spans. |
| `Max(TimeSpan other)` | `TimeSpan` | The larger of the two spans. |
| `ToHumanString()` | `string` | Compact two-unit format (`"1h 30m"`, `"45s"`, `"2d 5h"`). Negative spans get a `-` prefix. |

---
