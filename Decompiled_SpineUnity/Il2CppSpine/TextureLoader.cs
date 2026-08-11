using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppSpine;

public class TextureLoader : Il2CppObjectBase
{
	private static readonly IntPtr NativeMethodInfoPtr_Load_Public_Abstract_Virtual_New_Void_AtlasPage_String_0;

	static TextureLoader()
	{
		Il2CppClassPointerStore<TextureLoader>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "TextureLoader");
		NativeMethodInfoPtr_Load_Public_Abstract_Virtual_New_Void_AtlasPage_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TextureLoader>.NativeClassPtr, 100663509);
	}

	[CallerCount(0)]
	public unsafe virtual void Load(AtlasPage page, string path)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)page);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(path);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Load_Public_Abstract_Virtual_New_Void_AtlasPage_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public TextureLoader(IntPtr pointer)
		: base(pointer)
	{
	}
}
