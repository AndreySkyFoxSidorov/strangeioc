#include <cassert>
#include "../framework/binding/Binder.h"
#include "../framework/signal/Signal.h"
#include "../framework/injector/Injector.h"
#include "../framework/dispatcher/EventDispatcher.h"

using namespace strange::framework;

int main() {
    // Test Binder
    Binder binder;
    auto binding = binder.Bind("test");
    binding->To("value");
    binder.Register(*binding);
    assert(binder.Get("test") == "value");
    binder.Unbind("test");
    assert(binder.Get("test").empty());

    // Test Signal
    Signal<int> sig;
    int val = 0;
    auto listener = [&](int v){ val = v; };
    sig.AddListener(listener);
    sig.AddOnce([&](int v){ val = v + 1; });
    sig.Dispatch(41);
    assert(val == 42);
    sig.RemoveListener(listener);

    // Test Injector
    Injector inj;
    inj.Bind<int>([](){ return std::make_shared<int>(7); });
    assert(inj.Has<int>());
    auto instance = inj.Get<int>();
    assert(instance && *instance == 7);
    inj.Unbind<int>();
    assert(!inj.Has<int>());

    // Test EventDispatcher
    EventDispatcher disp;
    bool called = false;
    disp.AddListener("go", [&](){ called = true; });
    disp.AddOnce("go", [&](){ called = true; });
    disp.Dispatch("go");
    assert(called);
    called = false;
    disp.Dispatch("go");
    assert(called);

    return 0;
}
