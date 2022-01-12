#if !NET5_0_OR_GREATER
// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.

//.net4.xでinitアクセサを利用するために必要
//.net5に移行したら不要
//参考：https://blog.xin9le.net/entry/2021/05/05/205421

namespace System.Runtime.CompilerServices
{
    internal sealed class IsExternalInit
    { }
}
#endif
