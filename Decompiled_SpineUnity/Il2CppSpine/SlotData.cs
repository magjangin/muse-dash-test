using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class SlotData : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_index;

	private static readonly System.IntPtr NativeFieldInfoPtr_name;

	private static readonly System.IntPtr NativeFieldInfoPtr_boneData;

	private static readonly System.IntPtr NativeFieldInfoPtr_r;

	private static readonly System.IntPtr NativeFieldInfoPtr_g;

	private static readonly System.IntPtr NativeFieldInfoPtr_b;

	private static readonly System.IntPtr NativeFieldInfoPtr_a;

	private static readonly System.IntPtr NativeFieldInfoPtr_r2;

	private static readonly System.IntPtr NativeFieldInfoPtr_g2;

	private static readonly System.IntPtr NativeFieldInfoPtr_b2;

	private static readonly System.IntPtr NativeFieldInfoPtr_hasSecondColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_attachmentName;

	private static readonly System.IntPtr NativeFieldInfoPtr_blendMode;

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

	public unsafe BoneData boneData
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_boneData);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<BoneData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_boneData)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)boneData));
		}
	}

	public unsafe float r
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_r);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_r)) = num;
		}
	}

	public unsafe float g
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_g);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_g)) = num;
		}
	}

	public unsafe float b
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_b);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_b)) = num;
		}
	}

	public unsafe float a
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_a);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_a)) = num;
		}
	}

	public unsafe float r2
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_r2);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_r2)) = num;
		}
	}

	public unsafe float g2
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_g2);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_g2)) = num;
		}
	}

	public unsafe float b2
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_b2);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_b2)) = num;
		}
	}

	public unsafe bool hasSecondColor
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasSecondColor);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasSecondColor)) = flag;
		}
	}

	public unsafe string attachmentName
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachmentName);
			return IL2CPP.Il2CppStringToManaged(*(System.IntPtr*)num);
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachmentName)), IL2CPP.ManagedStringToIl2Cpp(text));
		}
	}

	public unsafe BlendMode blendMode
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_blendMode);
			return *(BlendMode*)num;
		}
		set
		{
			*(BlendMode*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_blendMode)) = blendMode;
		}
	}

	static SlotData()
	{
		Il2CppClassPointerStore<SlotData>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "SlotData");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SlotData>.NativeClassPtr);
		NativeFieldInfoPtr_index = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotData>.NativeClassPtr, "index");
		NativeFieldInfoPtr_name = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotData>.NativeClassPtr, "name");
		NativeFieldInfoPtr_boneData = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotData>.NativeClassPtr, "boneData");
		NativeFieldInfoPtr_r = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotData>.NativeClassPtr, "r");
		NativeFieldInfoPtr_g = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotData>.NativeClassPtr, "g");
		NativeFieldInfoPtr_b = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotData>.NativeClassPtr, "b");
		NativeFieldInfoPtr_a = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotData>.NativeClassPtr, "a");
		NativeFieldInfoPtr_r2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotData>.NativeClassPtr, "r2");
		NativeFieldInfoPtr_g2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotData>.NativeClassPtr, "g2");
		NativeFieldInfoPtr_b2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotData>.NativeClassPtr, "b2");
		NativeFieldInfoPtr_hasSecondColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotData>.NativeClassPtr, "hasSecondColor");
		NativeFieldInfoPtr_attachmentName = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotData>.NativeClassPtr, "attachmentName");
		NativeFieldInfoPtr_blendMode = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SlotData>.NativeClassPtr, "blendMode");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_String_BoneData_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotData>.NativeClassPtr, 100663830);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SlotData>.NativeClassPtr, 100663831);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 787926, RefRangeEnd = 787927, XrefRangeStart = 787901, XrefRangeEnd = 787926, MetadataInitTokenRva = 46284940L, MetadataInitFlagRva = 59867975L)]
	public unsafe SlotData(int index, string name, BoneData boneData)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SlotData>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&index);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(name);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)boneData);
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

	public SlotData(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
