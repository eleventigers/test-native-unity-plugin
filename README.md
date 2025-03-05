# MyUnityPlugin

A cross-platform Unity plugin built with CMake, Conan, and CPack.

## Prerequisites
- CMake (>= 3.10)
- Conan (>= 1.50)
- Unity (>= 2021.3)

## Build Instructions
```
mkdir build
cd build
conan install ..
cmake ..
cmake --build .
cpack .
```