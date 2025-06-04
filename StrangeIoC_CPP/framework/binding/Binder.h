#ifndef STRANGE_FRAMEWORK_BINDING_BINDER_H
#define STRANGE_FRAMEWORK_BINDING_BINDER_H

#include <memory>
#include <string>
#include <unordered_map>
#include "Binding.h"

namespace strange {
namespace framework {

class Binder {
private:
    std::unordered_map<std::string, std::string> bindings_;
public:
    std::shared_ptr<Binding> Bind(const std::string& key);
    void Register(const Binding& binding);
    std::string Get(const std::string& key) const;
    void Unbind(const std::string& key);
};

}
}

#endif
