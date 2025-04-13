#pragma once
#include <string>
#include <windows.h>
#include <vcclr.h>
#include <msclr/marshal_cppstd.h> // For string conversion

using namespace System;
using namespace System::Runtime::InteropServices;
using namespace msclr::interop;

namespace MachinesWrapper {

    public ref class MachineDriverWrapper
    {
    private:
        static HMODULE hModule = nullptr;

        // Function pointers (corrected Assemble signature)
        static std::string* (__cdecl* CreateSmallWheel_Native)();
        static std::string* (__cdecl* CreateBigWheel_Native)();
        static std::string* (__cdecl* CreateDoor_Native)(int);
        static std::string* (__cdecl* CreateGlass_Native)(int);
        static std::string* (__cdecl* CreateMotorPower_Native)(int);
        static std::string* (__cdecl* CreateNormalWheel_Native)();
        static std::string* (__cdecl* CreateThinWheel_Native)();
        static void(__cdecl* Assemble_Native)(std::string); // Fixed signature

        static void Initialize()
        {
            if (!hModule)
            {
                hModule = LoadLibrary(L"MachineDriver.dll");
                if (!hModule) throw gcnew Exception("DLL not found!");

                // Assign ALL function pointers with their mangled names
                CreateSmallWheel_Native = reinterpret_cast<decltype(CreateSmallWheel_Native)>(
                    GetProcAddress(hModule, "?CreateSmallWheel@@YAPEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@XZ")
                    );

                CreateBigWheel_Native = reinterpret_cast<decltype(CreateBigWheel_Native)>(
                    GetProcAddress(hModule, "?CreateBigWheel@@YAPEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@XZ")
                    );

                CreateDoor_Native = reinterpret_cast<decltype(CreateDoor_Native)>(
                    GetProcAddress(hModule, "?CreateDoor@@YAPEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@H@Z")
                    );

                CreateGlass_Native = reinterpret_cast<decltype(CreateGlass_Native)>(
                    GetProcAddress(hModule, "?CreateGlass@@YAPEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@H@Z")
                    );

                CreateMotorPower_Native = reinterpret_cast<decltype(CreateMotorPower_Native)>(
                    GetProcAddress(hModule, "?CreateMotorPower@@YAPEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@H@Z")
                    );

                CreateNormalWheel_Native = reinterpret_cast<decltype(CreateNormalWheel_Native)>(
                    GetProcAddress(hModule, "?CreateNormalWheel@@YAPEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@XZ")
                    );

                CreateThinWheel_Native = reinterpret_cast<decltype(CreateThinWheel_Native)>(
                    GetProcAddress(hModule, "?CreateThinWheel@@YAPEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@XZ")
                    );

                Assemble_Native = reinterpret_cast<decltype(Assemble_Native)>(
                    GetProcAddress(hModule, "?Assemble@@YAXV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@Z")
                    );

                // Verify ALL functions were found
                if (!CreateSmallWheel_Native || !CreateBigWheel_Native || !CreateDoor_Native ||
                    !CreateGlass_Native || !CreateMotorPower_Native || !CreateNormalWheel_Native ||
                    !CreateThinWheel_Native || !Assemble_Native)
                {
                    FreeLibrary(hModule);
                    hModule = nullptr;
                    throw gcnew Exception("One or more functions not found in DLL!");
                }
            }
        }

    public:
        // Wheel functions
        static String^ CreateSmallWheel()
        {
            Initialize();
            if (!CreateSmallWheel_Native) throw gcnew Exception("Function not found");
            std::string* result = CreateSmallWheel_Native();
            String^ managed = gcnew String(result->c_str());
            delete result;
            return managed;
        }

        static String^ CreateBigWheel()
        {
            Initialize();
            std::string* result = CreateBigWheel_Native();
            String^ managed = gcnew String(result->c_str());
            delete result;
            return managed;
        }

        // Door/Glass/Motor functions
        static String^ CreateDoor(int type)
        {
            Initialize();
            std::string* result = CreateDoor_Native(type);
            String^ managed = gcnew String(result->c_str());
            delete result;
            return managed;
        }

        static String^ CreateGlass(int type)
        {
            Initialize();
            std::string* result = CreateGlass_Native(type);
            String^ managed = gcnew String(result->c_str());
            delete result;
            return managed;
        }

        static String^ CreateMotorPower(int type)
        {
            Initialize();
            std::string* result = CreateMotorPower_Native(type);
            String^ managed = gcnew String(result->c_str());
            delete result;
            return managed;
        }

        // Wheel variants
        static String^ CreateNormalWheel()
        {
            Initialize();
            std::string* result = CreateNormalWheel_Native();
            String^ managed = gcnew String(result->c_str());
            delete result;
            return managed;
        }

        static String^ CreateThinWheel()
        {
            Initialize();
            std::string* result = CreateThinWheel_Native();
            String^ managed = gcnew String(result->c_str());
            delete result;
            return managed;
        }

        // Assembly function (fixed)
        static void Assemble(String^ data)
        {
            Initialize();
            if (!Assemble_Native) throw gcnew Exception("Assemble function missing");

            // Convert .NET string to std::string
            std::string nativeStr = marshal_as<std::string>(data);

            // Call native function with by-value parameter
            Assemble_Native(nativeStr);
        }
    };
}