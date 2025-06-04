#include <cassert>
#include "../framework/binding/Binder.h"
#include "../framework/signal/Signal.h"
#include "../framework/injector/Injector.h"

using namespace strange::framework;

int main() {
    // Test Binder
    Binder binder;
    auto binding = binder.Bind("test");
    binding->To("value");
    binder.Register(*binding);
    assert(binder.Get("test") == "value");

    // Test Signal
    Signal<int> sig;
    int val = 0;
    sig.AddListener([&](int v){ val = v; });
    sig.Dispatch(42);
    assert(val == 42);

    // Test Injector
    Injector inj;
    inj.Bind<int>([](){ return std::make_shared<int>(7); });
    auto instance = inj.Get<int>();
    assert(instance && *instance == 7);

    return 0;
}
