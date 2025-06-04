cmake_minimum_required(VERSION 3.10)
project(StrangeIoC_CPP)
add_library(StrangeIoC_CPP
    framework/binding/Binding.cpp
    framework/binding/Binder.cpp
)
target_include_directories(StrangeIoC_CPP PUBLIC ${CMAKE_CURRENT_SOURCE_DIR})
add_executable(StrangeIoC_CPP_tests tests/main.cpp)
target_link_libraries(StrangeIoC_CPP_tests StrangeIoC_CPP)
enable_testing()
add_test(NAME StrangeIoC_CPP_tests COMMAND StrangeIoC_CPP_tests)
