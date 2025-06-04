#include "Binding.h"

namespace strange {
namespace framework {

IBinding& Binding::Bind(const std::string& key) {
    key_ = key;
    return *this;
}

IBinding& Binding::To(const std::string& value) {
    value_ = value;
    return *this;
}

}
}
