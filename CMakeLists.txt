cmake_minimum_required(VERSION 3.16)
project(MyUnityPlugin)

# Include the Conan-generated toolchain file
include(conan_toolchain.cmake)

# Specify the source files for the library
add_library(my_plugin SHARED src/my_plugin.c)

# Add the "include" directory to the include path for this library
target_include_directories(my_plugin PUBLIC ${CMAKE_SOURCE_DIR}/include)

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
if (WIN32)
    install(TARGETS my_plugin
            RUNTIME DESTINATION Plugins/Windows
            LIBRARY DESTINATION Plugins/Windows
            ARCHIVE DESTINATION Plugins/Windows)
elseif (ANDROID)
    install(TARGETS my_plugin
            LIBRARY DESTINATION Plugins/Android/libs/${CMAKE_ANDROID_ARCH_ABI})
elseif (APPLE)
    install(TARGETS my_plugin
            LIBRARY DESTINATION Plugins/macOS
            BUNDLE DESTINATION Plugins/macOS)
elseif (UNIX AND NOT APPLE)
    install(TARGETS my_plugin
            LIBRARY DESTINATION Plugins/Linux)
endif()

# Include headers for packaging
install(DIRECTORY include/ DESTINATION Plugins/include)

# Install C# scripts
install(DIRECTORY unity/ DESTINATION Runtime FILES_MATCHING PATTERN "*.cs")

# Install documentation
install(FILES README.md DESTINATION Documentation)

# Install samples
# install(DIRECTORY samples/ DESTINATION Samples)

# Use CPack for packaging
include(InstallRequiredSystemLibraries)
set(CPACK_GENERATOR "ZIP")
set(CPACK_PACKAGE_NAME "MyUnityPlugin")
set(CPACK_PACKAGE_VERSION "1.0.0")
set(CPACK_PACKAGE_VENDOR "MyCompany")
set(CPACK_PACKAGE_DESCRIPTION "A cross-platform Unity plugin")
set(CPACK_PACKAGE_CONTACT "support@mycompany.com")
include(CPack)