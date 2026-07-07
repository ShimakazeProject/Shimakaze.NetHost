using System;
using System.Collections.Generic;

namespace Shimakaze.Marshals;

internal sealed class CombineDisposable(params IEnumerable<IDisposable> disposables) : IDisposable
{
    public static IDisposable Default { get; } = new CombineDisposable();

    public void Dispose()
    {
        foreach (var item in disposables)
            item.Dispose();
    }
}