#ifndef STRANGE_FRAMEWORK_SIGNAL_SIGNAL_H
#define STRANGE_FRAMEWORK_SIGNAL_SIGNAL_H

#include <functional>
#include <vector>
#include <algorithm>

namespace strange {
namespace framework {

template<typename... Args>
class Signal {
public:
    using Callback = std::function<void(Args...)>;

    void AddListener(const Callback& cb) { listeners_.push_back(cb); }

    void AddOnce(const Callback& cb) { once_listeners_.push_back(cb); }

    void RemoveListener(const Callback& cb) {
        auto remove_cb = [&](std::vector<Callback>& list){
            list.erase(std::remove_if(list.begin(), list.end(),
                [&](const Callback& existing){
                    return existing.target_type() == cb.target_type() &&
                           existing.template target<void(Args...)>() == cb.template target<void(Args...)>();
                }), list.end());
        };
        remove_cb(listeners_);
        remove_cb(once_listeners_);
    }

    void Dispatch(Args... args) {
        for (auto& cb : listeners_) {
            cb(args...);
        }
        for (auto& cb : once_listeners_) {
            cb(args...);
        }
        once_listeners_.clear();
    }

private:
    std::vector<Callback> listeners_;
    std::vector<Callback> once_listeners_;
};

}
}

#endif
