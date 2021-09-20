# UnityUtility

An open source library that includes self-made collections, self-made LINQ operators, and self-made modules that are useful in Unity development.

This is the so-called "Oreore Library" that summarizes the code that the author (@yutorisan) felt reusable in Unity development.
The GitHub release is self-sufficient, so please use it as a reference if you like.
Issues and pull requests (if any) are welcome.

## Examples

Here are some of the features included in UnityUtility.

### `Angle` module

Compared to handling angles with primitive types such as `float`, you can handle them more concisely without worrying about the radian method and degree method.
You can also get various information about the angle and operate it.

```csharp
//60° angle
Angle angle60 = Angle.FromDegree(60);
//An angle obtained by multiplying 60° by 10
Angle angle600 = angle60 * 10;
//Normalize 600° → 60°
Angle normalizedAngle = angle600.Normalize();
//Acquiring cosine
float cos60 = angle60.Cos;
```

### `IQueue<T>` interface

`System.Collections.Generic` contains an undefined` IQueue<T> `interface.
You can take advantage of this to implement your own queue structure collection class.

For example, this library contains the `ReactiveQueue<T>` class created by implementing `IQueue<T>`.

### `Combine` operator

`Combine` is LINQ's new proprietary operator.

You can combine the two IEnumerable sequences in any way you like. The difference from the `Zip` operator is that when the number of elements in the composition source is different, it is not adjusted to the smaller one, but to the larger one.

```csharp
//1, 2, 3, 4, 5
IEnumerable<int> intSq = Enumerable.Range(1, 5);
//a, b, c
IEnumerable<string> strSq = new List<string>(){"a", "b", "c"};

//1a, 2b, 3c, 4, 5
IEnumerable<string> combinedSq = intSq.Combine(strSq, (intEl, strEl) => intEl + strEl);
```

All features are listed on the [Wiki](https://github.com/yutorisan/UnityUtility/wiki)(Japanese).
## Requirements

To use this library, you need to have [UniRx](https://github.com/neuecc/UniRx) installed in your Unity project.

## Operating environment

We have confirmed the operation in the following environment.

- Visual Studio for Mac 8.10.8 build 0
- Unity 2021.1.17f


## Introduction method

To install this library in your Unity project, follow the steps below.

1. Store `UnityUtility.dll` in the root of this repository in the` Assets/Plugins` directory of the Unity project. If you also store `UnityUtility.xml` at the same time, you can display quick hints on the development path in Visual Studio.
1. Open the Unity project and it will start compiling and become available.
## How to customize

You can fork or clone this repository to customize it.

1. Fork or clone the repository and save it locally.
2. Open `UnityUtility.sln`.
3. From "Add Reference ...", add the following reference settings.
     - UniRx.dll
       - If you have UniRx installed in your Unity project, it should be in Library / ScriptAssemblies
     - UnityEngine.CoreModule.dll
       - Should be in Unity.app/Contents/PlaybackEngines/MacStandaloneSupport/Variations/mono/Managed
4. Modify the code and build.
5. Import the output dll into your Unity project as described in "Installation Method".


## How to use

See [Wiki](https://github.com/yutorisan/UnityUtility/wiki)(Japanese).

## License

MIT

## Right notation

UniRx Copyright (c) 2018 Yoshifumi Kawai
https://github.com/neuecc/UniRx/blob/master/LICENSE
