#ifndef STRANGE_FRAMEWORK_BINDING_IBINDING_H
#define STRANGE_FRAMEWORK_BINDING_IBINDING_H

#include <string>

namespace strange {
namespace framework {

class IBinding {
public:
    virtual ~IBinding() = default;
    virtual IBinding& Bind(const std::string& key) = 0;
    virtual IBinding& To(const std::string& value) = 0;
};

}
}

#endif
