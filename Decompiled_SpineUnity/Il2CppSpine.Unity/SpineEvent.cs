using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppSpine.Unity;

public class SpineEvent : SpineAttributeBase
{
	private static readonly IntPtr NativeFieldInfoPtr_audioOnly;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_String_Boolean_Boolean_Boolean_0;

	public unsafe bool audioOnly
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_audioOnly);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_audioOnly)) = flag;
		}
	}

	static SpineEvent()
	{
		Il2CppClassPointerStore<SpineEvent>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SpineEvent");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SpineEvent>.NativeClassPtr);
		NativeFieldInfoPtr_audioOnly = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineEvent>.NativeClassPtr, "audioOnly");
		NativeMethodInfoPtr__ctor_Public_Void_String_String_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineEvent>.NativeClassPtr, 100664393);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793408, XrefRangeEnd = 793416, MetadataInitTokenRva = 46303204L, MetadataInitFlagRva = 59848235L)]
	public unsafe SpineEvent(string startsWith = "", string dataField = "", bool includeNone = true, bool fallbackToTextField = false, bool audioOnly = false)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SpineEvent>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(startsWith);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(dataField);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &includeNone;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &fallbackToTextField;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &audioOnly;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_String_Boolean_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SpineEvent(IntPtr pointer)
		: base(pointer)
	{
	}
}
