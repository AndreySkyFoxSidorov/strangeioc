#include "Binder.h"

namespace strange {
namespace framework {

std::shared_ptr<Binding> Binder::Bind(const std::string& key) {
    auto binding = std::make_shared<Binding>();
    binding->Bind(key);
    return binding;
}

void Binder::Register(const Binding& binding) {
    bindings_[binding.key()] = binding.value();
}

std::string Binder::Get(const std::string& key) const {
    auto it = bindings_.find(key);
    if (it != bindings_.end())
        return it->second;
    return std::string();
}

}
}
