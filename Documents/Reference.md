# 汎用モジュール

単体で使用できる便利なクラスや構造体です。

## ユーティリティ

### `Angle`構造体

[この記事](https://qiita.com/yutorisan/items/63679fc1babb142e5b01)参照。

### `SerializeInterface<TInterface>`クラス

インスペクタでインターフェイスを引き渡すことを擬似的に実現することができます。

[Odin](https://assetstore.unity.com/packages/tools/utilities/odin-inspector-and-serializer-89041?locale=ja-JP)を導入せずに、`MonoBehaviour`同士をインターフェイス経由でアクセスさせたい場合に有効です。

#### 型引数

|  型引数名  |  説明   |
| --- | --- |
|  `TInterface`   |  公開するインターフェイスの型。   |

#### コンストラクタ

|  オーバーロード   |  説明   |
| --- | --- |
|  `SerializeInterface<TInterface>()`   |  デフォルトコンストラクタ   |

#### プロパティ

|  名前   |  説明   |
| --- | --- |
|   `Interface`  |  インターフェイスを取得します。   |

#### メソッド

なし

### `UniqueRandom`クラス

特定の範囲内の数値をランダムかつ重複せずに出力するモジュールです。

例えば、1~9のうち３つの異なる数値をランダムに取り出すことができます。

#### 使用例

```csharp
//1~9の範囲内で毎回異なる整数値をランダムに出力するモジュールを生成
UniqueRandom random = new UniqueRandom(1, 10);

random.Pick(); //3
random.Pick(); //9
random.Pick(); //1
```

#### コンストラクタ

| オーバーロード | 説明 |
| --- | --- |
| `UniqueRandom(int, int)` | 出力する範囲を指定してモジュールを新規作成します。例えば、`1, 10`と指定する場合、1~9の範囲内で重複しないランダムな数値を出力するモジュールが生成されます。 |
|`UniqueRandom(int, int, bool)`|上記に加え、すべての値を取り出しきったあとにさらに値を取り出そうとしたときの挙動を指定します。第3引数に`true`を指定すると、すべての値を取り出しきったあとに更に値を取り出すとき、重複情報がリセットされます。|

#### プロパティ

| 名前 | 説明 |
| --- | --- |
| `IsAutoReset` | すべての値を取り出しきったとき、重複情報をリセットするかどうか。 |

#### メソッド

| 名前 | 説明 |
| --- | --- |
| `Pick()` | 指定の範囲内で重複しないランダムな値を取り出します。<br>すべての値を取り出しきった状態で、かつ`IsAutoReset`が`false`の場合、`EveryValuePickupedException`がスローされます。 |
| `Reset()` | 重複情報をリセットします。 |

### `UniReadOnly<T>`クラス

`MonoBehaviour`なクラスではコンストラクタが利用できないため、`readonly`なフィールドを設けることが難しいという問題があります。`UniReadOnly<T>`クラスは、このような問題を解決するために、対象のオブジェクトが読み取り専用となるように保護することができます。

#### 使用例

```csharp
public class SomeMonoBehaviour : MonoBehaviour
{
    private readonly UniReadOnly<object> readonlyObject = new UniReadOnly<object>();

    void Start()
    {
        //１回だけ初期化可能
        readonlyObject.Initialize(new object());
    }

    void SomeMethod()
    {
        //値の取り出し
        object obj = readonlyObject.Value;
        //２回目以降は初期化すると例外発生
        readonlyObject.Initialize(new object()); //AlreadyInitializedException
    }
}
```

#### コンストラクタ

| オーバーロード | 説明 |
| --- | --- |
|  `UniReadOnly<T>()`  |  デフォルトコンストラクタ   |
| `UniReadOnly<T>(bool)` | 上書きしようとしたときに例外をスローするのではなく無視する（何もしない）挙動にする場合は`true`を指定します |

#### プロパティ

| 名前 | 説明 |
| --- | --- |
| `Value`  | ラップされた中身の値を取得します。 |
| `IsInitialized` | 初期化されているかどうかを取得します。 |
| `IsOverwriteIgnoreMode` | 上書きしようとしたときに例外をスローするのではなく無視する設定かどうかを取得します。 |

#### メソッド

| 名前 | 説明 |
| --- | --- |
|  `Initialize(T)`   | 値を初期化します。<br>このメソッドは1回だけ使用することを想定しています。2回以上呼び出すと`AlreadyInitializedException`がスローされます。<br>(`IsOverwriteIgnoreMode`が`true`の場合は何もしません。) |


## コレクション

### `IQueue<T>`インターフェイス

先入れ先出しのキューを表すインターフェイスです。

#### 定義

```csharp
UnityUtility.Collections.IQueue<T> : System.Collections.Generic.IReadOnlyCollection<T>, System.Collections.Generic.IEnumerable<T>
```

#### メソッド

| 名前 | 説明 |
| --- | --- |
| `Enqueue(T)` | 値をエンキューします。 |
| `Dequeue()` | 値をデキューします。 |
| `Peel()` | 現在の値を取り出さずに参照します。 |

### `Queue<T>`クラス

`System.Collections.Generic.Queue<T>`と同等の機能をもち、`IQueue<T>`インターフェイスを実装したクラスです。

#### 定義

```csharp
UnityUtility.Collections.Queue<T> : UnityUtility.Collections.IQueue<T>
```

内部に`System.Collections.Generic.Queue<T>`をもち、すべてのインターフェイスにおいて`System.Collections.Generic.Queue<T>`にそのままアクセスしているため、説明は割愛します。

### `FixedCapacityQueue<T>`クラス

要素数が固定のQueueです

#### 定義

```csharp
UnityUtility.Collections.FixedCapacityQueue<T> : UnityUtility.Collections.IQueue<T>
```

#### コンストラクタ

| オーバーロード | 説明 |
| --- | --- |
| `FixedCapacityQueue<T>(IQueue<T>, int, CapacityOverQueueBehaviour)` | 内部に使用するキュー、固定長、固定長を超えてエンキューした場合の挙動を指定して、インスタンスを生成します。    |
| `FixedCapacityQueue<T>(int, CapacityOverQueueBehaviour)` | 固定長の長さを指定して、インスタンスを生成します。内部で使用するキューは`Queue<T>`となります。 |
| `FixedCapacityQueue<T>(IReadOnlyCollection<T>, int, CapacityOverQueueBehaviour)` | ソースコレクションを基に、インスタンスを生成します。固定長の長さはソースコレクションの要素数となります。 |

#### プロパティ

| 名前 | 説明 |
| --- | --- |
| `Count` | 要素数を取得します。 |
| `Capacity` | 固定長を取得します。 |
| `HasVacancy` | 固定長に対して、まだエンキューできる空きがあるかどうかを取得します。 |
| `CapacityOverBehaviour` | 空きがない状態でエンキューしたときの挙動の設定を取得します。 |

#### メソッド

| 名前 | 説明 |
| --- | --- |
| `Enqueue(T)` | 値をエンキューします。<br>固定長に対して空きがない場合、`CapacityOverBehaviour`に従って挙動が決定します。 |
| `Dequeue()` | 値をデキューします。 |
| `Peel()` | 現在の値を取り出さずに参照します。 |

### `ReactiveQueue<T>`クラス

変更通知を購読できるキューです。

#### 定義

```csharp
UnityUtility.Collections.ReactiveQueue<T> : UnityUtility.Collections.Queue<T>, UnityUtility.Collections.IReactiveQueue<T>
```

#### コンストラクタ

| オーバーロード | 説明 |
| --- | --- |
| `ReactiveQueue<T>()` | 要素が空のインスタンスを新規作成します。 |
|`ReactiveQueue<T>(int)`| 要素の初期量を指定して、インスタンスを新規作成します。 |
|`ReactiveQueue<T>(IEnumerable<T>)`| ソースとなるコレクションを指定して、インスタンスを新規作成します。 |

#### プロパティ

なし

#### メソッド

| 名前 | 説明 |
| --- | --- |
|`Enqueue(T)`|要素をエンキューします。|
|`Dequeue()`|要素をデキューします。|
|`Peek()`|要素をピークします。|
|`Clear()`|要素をすべてクリアします。|
|`ObserveEnqueue()`|エンキューされた要素を購読します。|
|`ObserveDequeue()`|デキューされた要素を購読します。|
|`ObserveAdd()`|要素の追加を購読します。<br>エンキューされたときに通知が発行されます。|
|`ObserveCountChanged(bool)`|要素数の変更を購読します。<br>エンキューやデキューなどで要素数が変更された際に通知が発行されます。|
|`ObserveMove()`|要素の移動を購読します。<br>デキューやエンキューにより要素が移動した際に通知が発行されます。|
|`ObserveRemove()`|要素の削除を購読します。<br>デキューによって要素が削除された際に通知が発行されます。|
|`ObserveReplace()`|要素の入れ替えを購読します。<br>キューでは、要素の入れ替えができないため、値が発行されることはありません。|
|`ObserveReset()`|要素のクリアを購読します。|

### `FixedCapacityReactiveQueue<T>`クラス

要素数が固定の`ReactiveQueue<T>`です。

このクラスは、`FixedCapacityQueue<T>`の内部で使用する`IQeuue<T>`に`ReactiveQueue<T>`を適用したものです。`IReactiveQueue<T>`インターフェイスメンバの実装は、すべて`ReactiveQueue<T>`にそのまま接続しているため、詳細な説明は割愛します。

#### 定義

```csharp
UnityUtility.Collections.FixedCapacityReactiveQueue<T> : UnityUtility.Collections.FixedCapacityQueue<T>, UnityUtility.Collections.IReactiveQueue<T>
```

#### コンストラクタ

| オーバーロード | 説明 |
| --- | --- |
| `FixedCapacityReactiveQueue<T>(int, CapacityOverQueueBehaviour)` | 固定長を指定して、インスタンスを新規作成します。 |
| `FixedCapacityReactiveQueue<T>(IReadOnlyCollection<T>, CapacityOverQueueBehaviour)` |ソースコレクションを指定して、インスタンスを新規作成します。|


# LINQオペレータ

`System.Linq`名前空間にはない新しい独自のLINQオペレータです。
これらのオペレータは`UnityUtility.Linq`名前空間に存在します。

## Combine

2つの異なるシーケンスを合成して新しいシーケンスを生成します。
シーケンスの要素数が異なる場合、要素数が少ないシーケンスの不足要素は`default`として合成されます。

### 定義

```csharp
IEnumerable<TResult> Combine<TSource1, TSource2, TResult>(this IEnumerable<TSource1> source1, IEnumerable<TSource2> source2, Func<TSource1, TSource2, TResult> combiner) 
```

### 型引数

| 型引数名 | 説明 |
| --- | --- |
| `TResult` | 生成された新しいシーケンスの型 |
| `TSource1` | 1つ目の合成元となるシーケンスの型 |
| `TSource2`    | 2つ目の合成元となるシーケンスの型 |

### 引数

| 型 | 引数名 | 説明 |
| --- | --- | --- |
| `IEnumerable<TSource1>` | `source1` | 1つ目の合成元シーケンス |
| `IEnumerable<TSource2>` | `source2` | 2つ目の合成元シーケンス |
| `Func<TSource1, TSource2, TResult>` | `combiner` | 1つ目と2つ目のシーケンスの合成方法 |

### 使用例

```csharp
//1, 2, 3, 4, 5
IEnumerable<int> intSq = Enumerable.Range(1, 5);
//a, b, c
IEnumerable<string> strSq = new List<string>(){"a", "b", "c"};

//1a, 2b, 3c, 4, 5
IEnumerable<string> combinedSq = intSq.Combine(strSq, (intEl, strEl) => intEl + strEl);
```

## Debug

シーケンスに含まれるすべての要素を`Debug.Log`で出力します。

### 定義

```csharp
//そのまま出力するオーバーロード
IEnumerable<T> Debug<T>(this IEnumerable<T> source, string label = null)
//変換して出力するオーバーロード
IEnumerable<TSource> Debug<TSource, TSelector>(this IEnumerable<TSource> source, Func<TSource, TSelector> selector, string label = null)
```

### 型引数

| 型引数名  | 説明 |
| --- | --- |
| `TSource` | ソースシーケンスの型 |
| `TSelector` | ソースシーケンスを変換して出力する場合、変換後の型 |

### 引数

そのまま出力するオーバーロード

| 型 | 引数名 | 説明 |
| --- | --- | --- |
| `IEnumerable<TSource>` | `source` | ソースシーケンス |
| `string` | `label` | 出力の際に付加するラベル文字列 |

変換して出力するオーバーロード

| 型 | 引数名 | 説明 |
| --- | --- | --- |
| `IEnumerable<TSource>` | `source` | ソースシーケンス |
| `Func<TSource, TSelector>` | `selector` | 出力前の変換方法 |
| `string` | `label` | 出力の際に付加するラベル文字列 |

### 使用例

```csharp
//1,2,3,4,5の順にConsoleに出力される
Enumerable.Range(1, 5).Debug();
```

## Do

シーケンスに影響を与えずに、LINQメソッドチェーンの任意の箇所で任意の処理を実行します。

### 定義

```csharp
IEnumerable<T> Do<T>(this IEnumerable<T> source, Action<T> action)
```

### 型引数

| 型引数名 | 説明 |
| --- | --- |
| `T` | ソースシーケンスの型 |

### 引数

| 型 | 引数名 | 説明 |
| --- | --- | --- |
| `IEnumerable<T>` | `source` | ソースシーケンス |
| `Action<T>` | `action` | 実行する処理 |

### 使用例

```csharp
IEnumerable<int> sq = 
    Enumerable.Range(1, 5)
              .Do(i => SomeMethod(i)) //1,2,3,4,5に対してSomeMethodを実行
              .Select(i => i * 2); //2,4,6,8,10
```

## ExcludeNull

`.Where(n => n != null)`の短縮形です。

### 定義

```csharp
IEnumerable<T> ExcludeNull<T>(this IEnumerable<T> source)
```

### 型引数

| 型引数名 | 説明 |
| --- | --- |
| `T` | ソースシーケンスの型 |

### 引数

| 型 | 引数名 | 説明 |
| --- | --- | --- |
| `IEnumerable<T>` | `source` | ソースシーケンス |

### 使用例

```csharp
new List<object>(){ new object(), null, new object() }
    .ExcludeNull(); // object, object
```

## FirstOrAny / LastOrAny

シーケンスの最初から順に調べて、条件に合致する最初の要素を返します。条件に合致する要素がない場合、指定した任意の要素を返します。

`FirstOrDefault` / `LastOrDefault`は条件に合致する要素がない場合に`default`を返しますが、このオペレータは指定した任意の要素を返すことが特徴です。

### 定義

```csharp
T FirstOrAny<T>(this IEnumerable<T> source, Predicate<T> predicate, T notFoundValue);
T LastOrAny<T>(this IEnumerable<T> source, Predicate<T> predicate, T notFoundValue);
```

### 型引数

| 型引数名 | 説明 |
| --- | --- |
| `T` | ソースシーケンスの型 |

### 引数

| 型 | 引数名 | 説明 |
| --- | --- | --- |
| `IEnumerable<T>` | `source` | ソースシーケンス |
| `Predicate<T>` | `predicate` | 検索条件 |
| `T` | `notFoundValue` | 条件に合致する要素が見つからないときに返す値 |

### 使用例

```csharp
IEnumerable<string> animalList1 = new List<string>()
{
    "rabbit", 
    "cat",
    "elephant",
    "mouse"
};

IEnumerable<string> animalList2 = new List<string>()
{
    "horse",
    "deer",
    "dog"
};

//animalList1の中から、最初に見つかった7文字以上の要素
string over7CharacterAnimal1 = animalList1.FirstOrAny(animal => animal.Length > 7, "none"); //→elephant

//animalList2の中から、最初に見つかった7文字以上の要素
string over7CharacterAnimal2 = animalList2.FirstOrAny(animal => animal.Length > 7, "none"); //→none
```

## MaxBy / MinBy

比較する方法を指定して、オブジェクトの最大値/最小値をすべて取得します。

`Max`、`Min`はすでに比較可能なオブジェクト集合に対しての最大値、最小値を求めるのに対して、`MaxBy`, `MinBy`は比較する方法を指定することで、任意のオブジェクトに対して最大値、最小値を取得できます。

### 定義

```csharp
IEnumerable<TSource> MaxBy<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> selector) where TCompare : IComparable<TCompare>, IEquatable<TCompare>;
IEnumerable<TSource> MinBy<TSource, TCompare>(this IEnumerable<TSource> source, Func<TSource, TCompare> selector) where TCompare : IComparable<TCompare>, IEquatable<TCompare>;
```

### 型引数

| 型引数名 | 説明 |
| --- | --- |
| `TSource` | ソースシーケンスの型 |
| `TCompare` | 比較に使用する値の型<br>`TCompare`は`IComparable<TCompare>`および`IEqutable<TCompare>`を実装している必要があります。 |

### 引数

| 型 | 引数名 | 説明 |
| --- | --- | --- |
| `IEnumerable<TSource>` | `source` | ソースシーケンス |
| `Func<Tsource, TCompare>` | `selector` | 比較に使用する値を指定するselector |

### 使用例

```csharp
IEnumerable<TimeSpan> timeSpans = new List<TimeSpan>()
{
    TimeSpan.FromMinutes(10),   //① 0h 10m 00s
    TimeSpan.FromHours(1),      //② 1h 00m 00s
    TimeSpan.FromSeconds(650),  //③ 0h 10m 50s
    TimeSpan.FromMinutes(3),    //④ 0h 03m 00s
};

//「分」の値が最も高い要素をすべて取得する → ①, ③
IEnumerable<TimeSpan> maxMinutes = timeSpans.MaxBy(span => span.Minutes);
//「分」の値が最も低い要素をすべて取得する → ②
IEnumerable<TimeSpan> minMinutes = timeSpans.MinBy(span => span.Minutes);
```

## MaxByFirst / MinByFirst

`MaxBy().First()` / `MinBy().First()`と同義です。

`MaxBy`, `MinBy`は該当する要素をすべて列挙するのに対して、`MaxByFirst`, `MinByFirst`は該当する要素の中から最初に見つかったオブジェクト単体を返します。

### 定義

```csharp
TSource MaxByFirst<TSource, TComapre>(this IEnumerable<TSource> source, Func<TSource, TComapre> selector) where TComapre : IComparable<TComapre>, IEquatable<TComapre>;
TSource MinByFirst<TSource, TComapre>(this IEnumerable<TSource> source, Func<TSource, TComapre> selector) where TComapre : IComparable<TComapre>, IEquatable<TComapre>
```

定義以降は割愛します。

## ProcessIf

シーケンスの要素のうち、条件が一致する要素に任意の処理を加えます。

### 定義

```csharp
IEnumerable<T> ProcessIf<T>(this IEnumerable<T> source, Func<T, T> process, Predicate<T> predicate);
```

### 型引数

| 型引数名 | 説明 |
| --- | --- |
| `T` | ソースシーケンスの型 |

### 引数

| 型 | 引数名 | 説明 |
| --- | --- | --- |
| `IEnumerable<T>` | `source` | ソースシーケンス |
| `Func<T, T>` | `process` | 条件に合致した場合に実行する処理 |
| `Predicate<T>` | `predicate` | 処理を実行する条件 |

### 使用例

```csharp
//1~10のうち偶数のものを2倍する。
// →1,4,3,8,5,12,7,16,9,20
Enumerable.Range(1, 10)
          .ProcessIf(n => n * 2,
                     n => n % 2 == 0);
```

## WithIndex

シーケンスの各要素にインデックス情報を付加します。

### 定義

```csharp
IWithIndexEnumerable<T> WithIndex<T>(this IEnumerable<T> source);
```

`IWithIndexEnumerable<T>`は、`IEnumerable<WithIndex<T>>`と同義であり、`WithIndex<T>`は`T`型の`Value`プロパティと、インデックス番号を表す`int`型の`Index`プロパティを持つ構造体です。

### 型引数

| 型引数名 | 説明 |
| --- | --- |
| `T` | ソースシーケンスの型 |

### 引数

| 型 | 引数名 | 説明 |
| --- | --- | --- |
| `IEnumrable<T>` | source | ソースコレクション |

### 使用例

```csharp
//適当な数列
IEnumerable<int> numbers = new List<int>() { 5, 9, 1, 4, 6, 2, 0, 7 };

//数列の中で最大値とそのインデックスを取得する
//※WithIndex<T>は分解メソッドを実装している
var (value, index) =
    numbers.WithIndex()
           .MaxByFirst(withindex => withindex.Value);

//max value=9, index=1
print($"max value={value}, index={index}");
```

## DictionaryWhere

辞書型の`Value`に対して任意の条件でフィルタリングします。

### 定義

```csharp
IEnumerable<KeyValuePair<TKey, TValue>> DictionaryWhere<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> dic, Predicate<TValue> predicate);
```

### 型引数

| 型引数名 | 説明 |
| --- | --- |
| `TKey` | Keyの型 |
| `TValue` | Valueの型 |

### 引数

| 型 | 引数名 | 説明 |
| --- | --- | --- |
| `IEnumerable<KeyValuePair<TKey, TValue>>` | `dic` | ソースとなる辞書型 |
| `Predicate<TValue>` | `predicate` | Valueに対する絞り込み条件 |

### 使用例

```csharp
Dictionary<string, int> priceTable = new Dictionary<string, int>()
{
    ["apple"] = 300,
    ["banana"] = 50,
    ["peach"] = 1200,
    ["muscat"] = 2000,
    ["strawberry"] = 400
};

//値段が1000以上のものだけ抽出
// →peach:1200, muscat:2000
priceTable.DictionaryWhere(price => price > 1000)
          .Debug(); 
               
```

## DictionarySelect

辞書型の`Value`に対して任意の射影処理を行います。

### 定義

```csharp
IEnumerable<KeyValuePair<TKey, TValueResult>> DictionarySelect<TKey, TValueSource, TValueResult>(this IEnumerable<KeyValuePair<TKey, TValueSource>> dic, Func<TValueSource, TValueResult> selector);
```

### 型引数

| 型引数名 | 説明 |
| --- | --- |
| `TKey` | Keyの型 |
| `TValueSource` | ソースとなる辞書型のValueの型 |
| `TValueResult` | 戻り値の辞書型のValueの型 |

### 引数

| 型 | 引数名 | 説明 |
| --- | --- | --- |
| `IEnumerable<TKey, TValueSource>` | `dic` | ソースとなる辞書型 |
| `Func<TValueSource, TValueResult>` | `selector` | 射影処理 |

### 使用例

```csharp
Dictionary<string, int> priceTable = new Dictionary<string, int>()
{
    ["apple"] = 300,
    ["banana"] = 50,
    ["peach"] = 1200,
    ["muscat"] = 2000,
    ["strawberry"] = 400
};

//値段をすべて1割引したものを出力する
// →apple:270, banana:45, peach:1080, muscat:1800, strawberry:360
priceTable.DictionarySelect(price => price * 0.9)
          .Debug();
```

## DictionaryCombine

Keyの型が同一な2つの辞書型を任意の方法で合成し、1つの新しい辞書型を生成します。

### 定義

```csharp
IEnumerable<KeyValuePair<TKey, TResultValue>> DictionaryCombine<TKey, TSourceValue1, TSourceValue2, TResultValue>(this IEnumerable<KeyValuePair<TKey, TSourceValue1>> source1, IEnumerable<KeyValuePair<TKey, TSourceValue2>> source2, Func<TSourceValue1, TSourceValue2, TResultValue> resultSelector);

IEnumerable<KeyValuePair<TKey, TResultValue>> DictionaryCombine<TKey, TSourceValue1, TSourceValue2, TResultValue>(this IEnumerable<KeyValuePair<TKey, TSourceValue1>> source1, IEnumerable<KeyValuePair<TKey, TSourceValue2>> source2, Func<TSourceValue1, TSourceValue2, TResultValue> resultSelector, Func<TSourceValue1> defaultValue1Selector, Func<TSourceValue2> defaultValue2Selector);
```

### 型引数

| 型引数名 | 説明 |
| --- | --- |
| `TKey` | Keyの型 |
| `TSourceValue1` | 1つ目の辞書型のValueの型 |
| `TSourceValue2` | 2つ目の辞書型のValueの型 |
| `TResultValue` | 合成後の辞書型のValueの型 |

### 引数

| 型 | 引数名 | 説明 |
| --- | --- | --- |
| `IEnumerable<KeyValuePair<TKey, TSourceValue1>>` | `source1` | 1つ目の合成元辞書型 |
| `IEnumerable<KeyValuePair<TKey, TSourceValue2>>` | `source2` | 2つ目の合成元辞書型 |
| `Func<TSourceValue1, TSourceValue2, TResultValue>` | `resultSelector` | Valueの合成方法 |
| `Func<TSourceValue1>` | `defaultValue1Selector` | 1つ目の辞書型に該当するKeyが存在しないときに使用する値の指定方法 |
| `Func<TSourceValue2>` | `defaultValue2Selector` | 1つ目の辞書型にあって2つ目の辞書型にないKeyあるとき、合成に使用する値の指定方法 |

### 使用例

```csharp
//A店の価格表
Dictionary<string, int> storeAPriceTable = new Dictionary<string, int>()
{
    ["apple"] = 300,
    ["banana"] = 50,
    ["peach"] = 1200,
    ["muscat"] = 2000,
    ["strawberry"] = 400,
    ["grape"] = 700
};
//B店の価格表
Dictionary<string, int> storeBPriceTable = new Dictionary<string, int>()
{
    ["tomato"] = 200,
    ["banana"] = 80,
    ["apple"] = 350,
    ["muscat"] = 1400,
    ["strawberry"] = 500,
    ["peach"] = 800
};

//商品ごとにA店とB店の平均価格を取得する
IEnumerable<KeyValuePair<string, double>> averagePriceTable =
    storeAPriceTable.DictionaryCombine(
        storeBPriceTable,
        //両方とも同一商品があった場合は単純平均を取得
        (priceA, priceB) => (priceA + priceB) * 0.5,
        //Bにしかない商品はBの価格を取得
        priceB => priceB,
        //Aにしかない商品はAの価格を取得
        priceA => priceA);

//取得した平均価格を出力
// →apple:325, banana:65, peach:1000, muscat:1700,
//  strawberry:450, grape:700, tomato:200
averagePriceTable.Debug();
```

## DictionaryZip

Keyの型が同一な2つの辞書型を任意の方法で合成し、1つの新しい辞書型を生成します。

`DictionaryCombine`との違いは、どちらかに存在しないKeyがあったとき、そのKeyの要素は無視される点にあります。

### 型引数

| 型引数名 | 説明 |
| --- | --- |
| `TKey` | Keyの型 |
| `TSourceValue1` | 1つ目の辞書型のValueの型 |
| `TSourceValue2` | 2つ目の辞書型のValueの型 |
| `TResultValue` | 合成後の辞書型のValueの型 |

### 引数

| 型 | 引数名 | 説明 |
| --- | --- | --- |
| `IEnumerable<KeyValuePair<TKey, TSourceValue1>>` | `source1` | 1つ目の合成元辞書型 |
| `IEnumerable<KeyValuePair<TKey, TSourceValue2>>` | `source2` | 2つ目の合成元辞書型 |
| `Func<TSourceValue1, TSourceValue2, TResultValue>` | `resultSelector` | Valueの合成方法 |

### 使用例

```csharp
//A店の価格表
Dictionary<string, int> storeAPriceTable = new Dictionary<string, int>()
{
    ["apple"] = 300,
    ["banana"] = 50,
    ["peach"] = 1200,
    ["muscat"] = 2000,
    ["strawberry"] = 400,
    ["grape"] = 700
};
//B店の価格表
Dictionary<string, int> storeBPriceTable = new Dictionary<string, int>()
{
    ["tomato"] = 200,
    ["banana"] = 80,
    ["apple"] = 350,
    ["muscat"] = 1400,
    ["strawberry"] = 500,
    ["peach"] = 800
};

//商品ごとにA店とB店の平均価格を取得する
IEnumerable<KeyValuePair<string, double>> averagePriceTable =
    storeAPriceTable.DictionaryZip(
        storeBPriceTable,
        //両方とも同一商品があった場合は単純平均を取得
        //(片方にしかない商品は無視する)
        (priceA, priceB) => (priceA + priceB) * 0.5);

//取得した平均価格を出力
// →apple:325, banana:65, peach:1000, muscat:1700,
//  strawberry:450
averagePriceTable.Debug();
```

## ExcludeNullValue

辞書型でValueが`null`のものを排除します。

### 定義

```csharp
IEnumerable<KeyValuePair<TKey, TValue>> ExcludeNullValue<TKey, TValue>(this IEnumerable<KeyValuePair<TKey, TValue>> source);
```

### 型引数

| 型引数名 | 説明 |
| --- | --- |
| `TKey` | ソースとなる辞書型のKeyの型 |
| `TValue` | ソースとなる辞書型のValueの型 |

### 引数

| 型 | 引数名 | 説明 |
| --- | --- | --- |
| `IEnumerable<KeyValuePair<TKey, TValue>>` | `source` | ソースとなる辞書型 |

### 使用例

```csharp
Dictionary<int, object> dictionary = new Dictionary<int, object>()
{
    [1] = new object(),
    [2] = null,
    [3] = new object()
};

//Valueがnullの要素を排除する
// →1:object, 3:object
dictionary.ExcludeNullValue().Debug();
```

## ToDictionary

`IEnumerable<KeyValuePair<TKey, TValue>>`から`Dictionary<TKey, TValue>`を生成します。

### 使用例

```csharp
Dictionary<int, object> dictionary = new Dictionary<int, object>()
{
    [1] = new object(),
    [2] = null,
    [3] = new object()
};

//オペレータを挟んで一旦IEnumerableとなった辞書型をDictionaryに戻す
Dictionary<int, object> nullExcludedTable = 
    dictionary.ExcludeNullValue().ToDictionary();
```