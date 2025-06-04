#ifndef STRANGE_FRAMEWORK_DISPATCHER_EVENTDISPATCHER_H
#define STRANGE_FRAMEWORK_DISPATCHER_EVENTDISPATCHER_H

#include <functional>
#include <unordered_map>
#include <vector>
#include <string>

namespace strange {
namespace framework {

class EventDispatcher {
public:
    using Callback = std::function<void()>;
    void AddListener(const std::string& type, Callback cb) {
        listeners_[type].push_back(cb);
    }
    void Dispatch(const std::string& type) {
        auto it = listeners_.find(type);
        if (it != listeners_.end()) {
            for (auto& cb : it->second) {
                cb();
            }
        }
    }
private:
    std::unordered_map<std::string, std::vector<Callback>> listeners_;
};

}
}

#endif
