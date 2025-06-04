#ifndef STRANGE_FRAMEWORK_DISPATCHER_EVENTDISPATCHER_H
#define STRANGE_FRAMEWORK_DISPATCHER_EVENTDISPATCHER_H

#include <functional>
#include <unordered_map>
#include <vector>
#include <string>
#include <algorithm>

namespace strange {
namespace framework {

class EventDispatcher {
public:
    using Callback = std::function<void()>;
    void AddListener(const std::string& type, Callback cb) {
        listeners_[type].push_back(cb);
    }

    void AddOnce(const std::string& type, Callback cb) {
        once_listeners_[type].push_back(cb);
    }

    void RemoveListener(const std::string& type, Callback cb) {
        auto remove_cb = [&](std::vector<Callback>& list){
            list.erase(std::remove_if(list.begin(), list.end(),
                [&](const Callback& existing){
                    return existing.target<void()>() == cb.target<void()>();
                }), list.end());
        };
        auto it = listeners_.find(type);
        if (it != listeners_.end()) remove_cb(it->second);
        it = once_listeners_.find(type);
        if (it != once_listeners_.end()) remove_cb(it->second);
    }

    void Dispatch(const std::string& type) {
        auto it = listeners_.find(type);
        if (it != listeners_.end()) {
            for (auto& cb : it->second) {
                cb();
            }
        }
        it = once_listeners_.find(type);
        if (it != once_listeners_.end()) {
            for (auto& cb : it->second) {
                cb();
            }
            once_listeners_.erase(it);
        }
    }
private:
    std::unordered_map<std::string, std::vector<Callback>> listeners_;
    std::unordered_map<std::string, std::vector<Callback>> once_listeners_;
};

}
}

#endif
