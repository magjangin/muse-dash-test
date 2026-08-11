using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using UnityEngine;

namespace Il2CppSpine.Unity;

public class SpineAttributeBase : PropertyAttribute
{
	private static readonly IntPtr NativeFieldInfoPtr_dataField;

	private static readonly IntPtr NativeFieldInfoPtr_startsWith;

	private static readonly IntPtr NativeFieldInfoPtr_includeNone;

	private static readonly IntPtr NativeFieldInfoPtr_fallbackToTextField;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Protected_Void_0;

	public unsafe string dataField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dataField);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_dataField)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe string startsWith
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_startsWith);
			return IL2CPP.Il2CppStringToManaged(*(IntPtr*)num);
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_startsWith)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe bool includeNone
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_includeNone);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_includeNone)) = flag;
		}
	}

	public unsafe bool fallbackToTextField
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fallbackToTextField);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_fallbackToTextField)) = flag;
		}
	}

	static SpineAttributeBase()
	{
		Il2CppClassPointerStore<SpineAttributeBase>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SpineAttributeBase");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SpineAttributeBase>.NativeClassPtr);
		NativeFieldInfoPtr_dataField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineAttributeBase>.NativeClassPtr, "dataField");
		NativeFieldInfoPtr_startsWith = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineAttributeBase>.NativeClassPtr, "startsWith");
		NativeFieldInfoPtr_includeNone = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineAttributeBase>.NativeClassPtr, "includeNone");
		NativeFieldInfoPtr_fallbackToTextField = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SpineAttributeBase>.NativeClassPtr, "fallbackToTextField");
		NativeMethodInfoPtr__ctor_Protected_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SpineAttributeBase>.NativeClassPtr, 100664389);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793386, XrefRangeEnd = 793392, MetadataInitTokenRva = 46303204L, MetadataInitFlagRva = 59848235L)]
	public unsafe SpineAttributeBase()
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SpineAttributeBase>.NativeClassPtr))
	{
		IntPtr* ptr = null;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Protected_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public SpineAttributeBase(IntPtr pointer)
		: base(pointer)
	{
	}
}
