using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;

namespace Il2CppSpine.Unity;

public class SpineSlot : SpineAttributeBase
{
	private static readonly IntPtr NativeFieldInfoPtr_containsBoundingBoxes;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_String_String_Boolean_Boolean_Boolean_0;

	public unsafe bool containsBoundingBoxes
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_containsBoundingBoxes);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_containsBoundingBoxes)) = flag;
		}
	}

	static SpineSlot()
	{
		Il2CppClassPointerStore<SpineSlot>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SpineSlot");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SpineSlot>.NativeClassPtr);
		NativeFieldInfoPtr_containsBoundingBoxes = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineSlot>.NativeClassPtr, "containsBoundingBoxes");
		NativeMethodInfoPtr__ctor_Public_Void_String_String_Boolean_Boolean_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineSlot>.NativeClassPtr, 100664391);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793400, XrefRangeEnd = 793408, MetadataInitTokenRva = 46303204L, MetadataInitFlagRva = 59848235L)]
	public unsafe SpineSlot(string startsWith = "", string dataField = "", bool containsBoundingBoxes = false, bool includeNone = true, bool fallbackToTextField = false)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SpineSlot>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[5];
		*ptr = IL2CPP.ManagedStringToIl2Cpp(startsWith);
		*(IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(dataField);
		*(bool**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &containsBoundingBoxes;
		*(bool**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &includeNone;
		*(bool**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &fallbackToTextField;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_String_String_Boolean_Boolean_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SpineSlot(IntPtr pointer)
		: base(pointer)
	{
	}
}
