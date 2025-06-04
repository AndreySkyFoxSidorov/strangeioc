#ifndef STRANGE_FRAMEWORK_INJECTOR_INJECTOR_H
#define STRANGE_FRAMEWORK_INJECTOR_INJECTOR_H

#include <memory>
#include <unordered_map>
#include <typeindex>
#include <functional>

namespace strange {
namespace framework {

class Injector {
public:
    template<typename T>
    void Bind(std::function<std::shared_ptr<T>()> factory) {
        creators_[std::type_index(typeid(T))] = [factory]() {
            return std::static_pointer_cast<void>(factory());
        };
    }

    template<typename T>
    std::shared_ptr<T> Get() {
        auto it = creators_.find(std::type_index(typeid(T)));
        if (it != creators_.end()) {
            return std::static_pointer_cast<T>(it->second());
        }
        return nullptr;
    }

    template<typename T>
    void Unbind() {
        creators_.erase(std::type_index(typeid(T)));
    }

    template<typename T>
    bool Has() const {
        return creators_.find(std::type_index(typeid(T))) != creators_.end();
    }

private:
    std::unordered_map<std::type_index, std::function<std::shared_ptr<void>()>> creators_;
};

}
}

#endif
