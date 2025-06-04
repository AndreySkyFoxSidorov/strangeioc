using NUnit.Framework;
using strange.extensions.pool.impl;
using strange.extensions.pool.api;
using strange.framework.api;

namespace Tests;

class TestObject : IPoolable
{
    public bool Restored { get; private set; }
    public void Restore() => Restored = true;
    public void Retain() { }
    public void Release() { }
    public bool retain { get; private set; }
}

class SimpleProvider : IInstanceProvider
{
    public T GetInstance<T>() => (T)System.Activator.CreateInstance(typeof(T));
    public object GetInstance(System.Type key) => System.Activator.CreateInstance(key);
}

public class PoolTests
{
    Pool<TestObject> pool = null!;

    [SetUp]
    public void Setup()
    {
        pool = new Pool<TestObject>();
        pool.instanceProvider = new SimpleProvider();
    }

    [Test]
    public void GetInstanceReturnsNewObjectWhenEmpty()
    {
        var obj = pool.GetInstance();
        Assert.NotNull(obj);
        Assert.AreEqual(0, pool.available);
    }

    [Test]
    public void ReturnInstanceRestoresAndStoresObject()
    {
        var obj = pool.GetInstance();
        pool.ReturnInstance(obj);
        Assert.IsTrue(obj.Restored);
        Assert.AreEqual(1, pool.available);
    }

    [Test]
    public void OverflowReturnsNullOrThrows()
    {
        pool.size = 1;
        pool.overflowBehavior = PoolOverflowBehavior.IGNORE;
        var first = pool.GetInstance();
        var second = pool.GetInstance();
        Assert.IsNull(second);
        pool.ReturnInstance(first);
        pool.overflowBehavior = PoolOverflowBehavior.EXCEPTION;
        pool.GetInstance();
        Assert.Throws<PoolException>(() => pool.GetInstance());
    }
}
