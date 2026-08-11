using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class BoneData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_index;

	private static readonly System.IntPtr NativeFieldInfoPtr_name;

	private static readonly System.IntPtr NativeFieldInfoPtr_parent;

	private static readonly System.IntPtr NativeFieldInfoPtr_length;

	private static readonly System.IntPtr NativeFieldInfoPtr_x;

	private static readonly System.IntPtr NativeFieldInfoPtr_y;

	private static readonly System.IntPtr NativeFieldInfoPtr_rotation;

	private static readonly System.IntPtr NativeFieldInfoPtr_scaleX;

	private static readonly System.IntPtr NativeFieldInfoPtr_scaleY;

	private static readonly System.IntPtr NativeFieldInfoPtr_shearX;

	private static readonly System.IntPtr NativeFieldInfoPtr_shearY;

	private static readonly System.IntPtr NativeFieldInfoPtr_transformMode;

	private static readonly System.IntPtr NativeFieldInfoPtr_skinRequired;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Name_Public_get_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_String_BoneData_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	public unsafe int index
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_index);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_index)) = num;
		}
	}

	public unsafe string name
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_name);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_name)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe BoneData parent
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_parent);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<BoneData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_parent)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)boneData));
		}
	}

	public unsafe float length
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_length);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_length)) = num;
		}
	}

	public unsafe float x
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_x);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_x)) = num;
		}
	}

	public unsafe float y
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_y);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_y)) = num;
		}
	}

	public unsafe float rotation
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rotation);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rotation)) = num;
		}
	}

	public unsafe float scaleX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scaleX);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scaleX)) = num;
		}
	}

	public unsafe float scaleY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scaleY);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_scaleY)) = num;
		}
	}

	public unsafe float shearX
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_shearX);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_shearX)) = num;
		}
	}

	public unsafe float shearY
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_shearY);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_shearY)) = num;
		}
	}

	public unsafe TransformMode transformMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transformMode);
			return *(TransformMode*)num;
		}
		set
		{
			*(TransformMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_transformMode)) = transformMode;
		}
	}

	public unsafe bool skinRequired
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skinRequired);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skinRequired)) = flag;
		}
	}

	public unsafe string Name
	{
		[CallerCount(8)]
		[CachedScanResults(RefRangeStart = 34063, RefRangeEnd = 34071, XrefRangeStart = 34063, XrefRangeEnd = 34071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Name_Public_get_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return IL2CPP.Il2CppStringToManaged(intPtr);
		}
	}

	static BoneData()
	{
		Il2CppClassPointerStore<BoneData>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "BoneData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<BoneData>.NativeClassPtr);
		NativeFieldInfoPtr_index = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneData>.NativeClassPtr, "index");
		NativeFieldInfoPtr_name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneData>.NativeClassPtr, "name");
		NativeFieldInfoPtr_parent = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneData>.NativeClassPtr, "parent");
		NativeFieldInfoPtr_length = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneData>.NativeClassPtr, "length");
		NativeFieldInfoPtr_x = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneData>.NativeClassPtr, "x");
		NativeFieldInfoPtr_y = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneData>.NativeClassPtr, "y");
		NativeFieldInfoPtr_rotation = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneData>.NativeClassPtr, "rotation");
		NativeFieldInfoPtr_scaleX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneData>.NativeClassPtr, "scaleX");
		NativeFieldInfoPtr_scaleY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneData>.NativeClassPtr, "scaleY");
		NativeFieldInfoPtr_shearX = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneData>.NativeClassPtr, "shearX");
		NativeFieldInfoPtr_shearY = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneData>.NativeClassPtr, "shearY");
		NativeFieldInfoPtr_transformMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneData>.NativeClassPtr, "transformMode");
		NativeFieldInfoPtr_skinRequired = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<BoneData>.NativeClassPtr, "skinRequired");
		NativeMethodInfoPtr_get_Name_Public_get_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneData>.NativeClassPtr, 100663620);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_String_BoneData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneData>.NativeClassPtr, 100663621);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<BoneData>.NativeClassPtr, 100663622);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 783710, RefRangeEnd = 783712, XrefRangeStart = 783692, XrefRangeEnd = 783710, MetadataInitTokenRva = 46329164L, MetadataInitFlagRva = 59849652L)]
	public unsafe BoneData(int index, string name, BoneData parent)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<BoneData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&index);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)parent);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_String_BoneData_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(8)]
	[CachedScanResults(RefRangeStart = 34063, RefRangeEnd = 34071, XrefRangeStart = 34063, XrefRangeEnd = 34071, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ToString_Public_Virtual_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public BoneData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
