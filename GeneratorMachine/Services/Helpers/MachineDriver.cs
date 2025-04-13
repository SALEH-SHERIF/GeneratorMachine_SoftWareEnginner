using System.Runtime.InteropServices;

namespace GeneratorMachine.Services.Helpers
{
    public static class MachineDriver
    {
        const string Driver = "MachineDriver.dll";

        // CreateSmallWheel (note the typo in mangled name)
        [DllImport(Driver, EntryPoint = "?CreateSmallWheel@@YAPEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateSmallWheel();

        [DllImport(Driver, EntryPoint = "?CreateBigWheel@@YAPEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateBigWheel();

        [DllImport(Driver, EntryPoint = "?CreateDoor@@YAPEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@H@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateDoor(int type);

        [DllImport(Driver, EntryPoint = "?CreateGlass@@YAPEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@H@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateGlass(int type);

        [DllImport(Driver, EntryPoint = "?CreateMotorPower@@YAPEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@H@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateMotorPower(int type);

        [DllImport(Driver, EntryPoint = "?CreateNormalWheel@@YAPEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateNormalWheel();

        [DllImport(Driver, EntryPoint = "?CreateThinWheel@@YAPEAV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@XZ", CallingConvention = CallingConvention.Cdecl)]
        public static extern IntPtr CreateThinWheel(); 
        
        [DllImport(Driver, EntryPoint = "?Assemble@@YAXV?$basic_string@DU?$char_traits@D@std@@V?$allocator@D@2@@std@@@Z", CallingConvention = CallingConvention.Cdecl)]
        public static extern void Assemble(IntPtr data);

        public static string PtrToString(IntPtr ptr)
        {
            return Marshal.PtrToStringAnsi(ptr);
        }

    }
}
