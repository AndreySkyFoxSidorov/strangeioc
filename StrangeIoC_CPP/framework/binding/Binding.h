#ifndef STRANGE_FRAMEWORK_BINDING_BINDING_H
#define STRANGE_FRAMEWORK_BINDING_BINDING_H

#include "IBinding.h"

namespace strange {
namespace framework {

class Binding : public IBinding {
private:
    std::string key_;
    std::string value_;
public:
    Binding() = default;
    IBinding& Bind(const std::string& key) override;
    IBinding& To(const std::string& value) override;
    const std::string& key() const { return key_; }
    const std::string& value() const { return value_; }
};

}
}

#endif
