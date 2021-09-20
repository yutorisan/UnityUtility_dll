# UnityUtility

Unity開発において便利な自作コレクション、自作LINQオペレータ、自作モジュールなどをまとめたオープンソースライブラリです。

作者(@yutorisan)がUnity開発において再利用可能と感じたコードをまとめているいわゆる「オレオレライブラリ」です。
GitHub公開は自己満ですのでよかったら参考程度にどうぞ。
Issueやプルリクは（もしあればですが）大歓迎です。

## 例

UnityUtilityに搭載された機能の一部を紹介します。

### `Angle`モジュール

角度を`float`等のプリミティブ型で扱う場合に比べ、弧度法・度数法を気にすることなく簡潔に扱うことができます。
角度に関する各種情報の取得、及び操作も用意されています。

```csharp
//60°の角度
Angle angle60 = Angle.FromDegree(60);
//60°を10倍した角度
Angle angle600 = angle60 * 10;
//600°を正規化 → 60°
Angle normalizedAngle = angle600.Normalize();
//余弦の取得
float cos60 = angle60.Cos;
```

### `IQueue<T>`インターフェイス

`System.Collections.Generic`には定義されていない`IQueue<T>`インターフェイスが含まれます。
これを利用して、独自のキュー構造コレクションクラスを実装することができます。

例えば、本ライブラリには`IQueue<T>`を実装して作られた`ReactiveQueue<T>`クラスが含まれています。

### `Combine`オペレータ

`Combine`はLINQの新しい独自オペレータです。

2つのIEnumerableシーケンスを、任意の方法で合成することができます。`Zip`オペレータと異なるのは、合成元の要素数が違った場合に、少ない方に合わせるのではなく**大きい方に合わせる**点です。

```csharp
//1, 2, 3, 4, 5
IEnumerable<int> intSq = Enumerable.Range(1, 5);
//a, b, c
IEnumerable<string> strSq = new List<string>(){"a", "b", "c"};

//1a, 2b, 3c, 4, 5
IEnumerable<string> combinedSq = intSq.Combine(strSq, (intEl, strEl) => intEl + strEl);
```

すべての機能は[Wiki](https://github.com/yutorisan/UnityUtility/wiki)に記載されています。

## 必要要件

本ライブラリを使用するには、Unityプロジェクトに[UniRx](https://github.com/neuecc/UniRx)を導入している必要があります。

## 動作環境

以下の環境で動作確認しています。

- Visual Studio for Mac 8.10.8 build 0
- Unity 2021.1.17f


## 導入方法

本ライブラリをUnityプロジェクトに導入するには、以下の手順を実行します。

1. 本リポジトリのルートにある`UnityUtility.dll`を、Unityプロジェクトの`Assets/Plugins`ディレクトリに格納します。`UnityUtility.xml`も同時に格納すると、Visual Studioでの開発路においてクイックヒントを表示することができます。
1. Unityプロジェクトを開くとコンパイルが始まり、使用可能になります。

## カスタマイズ方法

本リポジトリをフォークまたはクローンして、カスタマイズすることができます。

1. リポジトリをフォークまたはクローンして、ローカルに保存します。
2. `UnityUtility.sln`を開きます。
3. 「参照の追加…」から、以下の参照設定を追加します。
    - UniRx.dll
      - UnityプロジェクトにUniRxを導入していれば、Library/ScriptAssembliesにあるはず
    - UnityEngine.CoreModule.dll
      - Unity.app/Contents/PlaybackEngines/MacStandaloneSupport/Variations/mono/Managedにあるはず
4. コードを変更してビルドします。
5. 出力されたdllを「導入方法」の通りにUnityプロジェクトにインポートします。

## 使い方

[Wiki](https://github.com/yutorisan/UnityUtility/wiki)参照。

## ライセンス

MIT

## 権利表記

UniRx Copyright (c) 2018 Yoshifumi Kawai
https://github.com/neuecc/UniRx/blob/master/LICENSE
