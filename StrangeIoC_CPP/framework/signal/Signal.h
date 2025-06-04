#ifndef STRANGE_FRAMEWORK_SIGNAL_SIGNAL_H
#define STRANGE_FRAMEWORK_SIGNAL_SIGNAL_H

#include <functional>
#include <vector>

namespace strange {
namespace framework {

template<typename... Args>
class Signal {
public:
    using Callback = std::function<void(Args...)>;
    void AddListener(const Callback& cb) { listeners_.push_back(cb); }
    void Dispatch(Args... args) {
        for (auto& cb : listeners_) {
            cb(args...);
        }
    }
private:
    std::vector<Callback> listeners_;
};

}
}

#endif
