cmake_minimum_required(VERSION 3.10)
project(MyUnityPlugin)

# Set output directory for compiled libraries
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)

# Include Conan for dependency management
include(${CMAKE_BINARY_DIR}/conanbuildinfo.cmake)
conan_basic_setup()

# Add your C source files
add_library(my_plugin SHARED src/my_plugin.c include/my_plugin.h)

# Platform-specific configurations
if (WIN32)
    # Windows-specific settings
    set(CMAKE_WINDOWS_EXPORT_ALL_SYMBOLS ON)  # Export all symbols for DLL
    set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)
    set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)
    set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_BINARY_DIR}/bin)
elseif (ANDROID)
    # Android-specific settings
    set(CMAKE_ANDROID_ARCH_ABI "arm64-v8a")
elseif (APPLE)
    # macOS/iOS-specific settings
    set(CMAKE_OSX_ARCHITECTURES "x86_64;arm64")
elseif (EMSCRIPTEN)
    # WebAssembly-specific settings (Emscripten)
    set(CMAKE_EXECUTABLE_SUFFIX ".js")
    set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -s WASM=1 -s SIDE_MODULE=1 -s EXPORTED_FUNCTIONS='[\"_Add\"]' -s EXTRA_EXPORTED_RUNTIME_METHODS='[\"ccall\", \"cwrap\"]'")
    set(CMAKE_CXX_FLAGS "${CMAKE_CXX_FLAGS} -s WASM=1 -s SIDE_MODULE=1 -s EXPORTED_FUNCTIONS='[\"_Add\"]' -s EXTRA_EXPORTED_RUNTIME_METHODS='[\"ccall\", \"cwrap\"]'")
elseif (UNIX AND NOT APPLE)
    # Linux-specific settings
    set(CMAKE_C_FLAGS "${CMAKE_C_FLAGS} -fPIC")
endif()

# Install the library for packaging
install(TARGETS my_plugin
        LIBRARY DESTINATION lib
        ARCHIVE DESTINATION lib
        RUNTIME DESTINATION bin)

# Include headers for packaging
install(FILES include/my_plugin.h DESTINATION include)

# Use CPack for packaging
include(InstallRequiredSystemLibraries)
set(CPACK_GENERATOR "ZIP")
set(CPACK_PACKAGE_NAME "MyUnityPlugin")
set(CPACK_PACKAGE_VERSION "1.0.0")
set(CPACK_PACKAGE_VENDOR "MyCompany")
include(CPack)