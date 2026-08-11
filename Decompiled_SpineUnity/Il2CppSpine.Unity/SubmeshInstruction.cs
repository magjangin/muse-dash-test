using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;
using UnityEngine;

namespace Il2CppSpine.Unity;

public sealed class SubmeshInstruction : Il2CppSystem.ValueType
{
	private static readonly System.IntPtr NativeFieldInfoPtr_skeleton;

	private static readonly System.IntPtr NativeFieldInfoPtr_startSlot;

	private static readonly System.IntPtr NativeFieldInfoPtr_endSlot;

	private static readonly System.IntPtr NativeFieldInfoPtr_material;

	private static readonly System.IntPtr NativeFieldInfoPtr_forceSeparate;

	private static readonly System.IntPtr NativeFieldInfoPtr_preActiveClippingSlotSource;

	private static readonly System.IntPtr NativeFieldInfoPtr_rawTriangleCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_rawVertexCount;

	private static readonly System.IntPtr NativeFieldInfoPtr_rawFirstVertexIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_hasClipping;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	public unsafe Skeleton skeleton
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeleton);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skeleton>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_skeleton)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton));
		}
	}

	public unsafe int startSlot
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_startSlot);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_startSlot)) = num;
		}
	}

	public unsafe int endSlot
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_endSlot);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_endSlot)) = num;
		}
	}

	public unsafe Material material
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_material);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Material>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_material)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)material));
		}
	}

	public unsafe bool forceSeparate
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_forceSeparate);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_forceSeparate)) = flag;
		}
	}

	public unsafe int preActiveClippingSlotSource
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_preActiveClippingSlotSource);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_preActiveClippingSlotSource)) = num;
		}
	}

	public unsafe int rawTriangleCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rawTriangleCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rawTriangleCount)) = num;
		}
	}

	public unsafe int rawVertexCount
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rawVertexCount);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rawVertexCount)) = num;
		}
	}

	public unsafe int rawFirstVertexIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rawFirstVertexIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_rawFirstVertexIndex)) = num;
		}
	}

	public unsafe bool hasClipping
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasClipping);
			return *(bool*)num;
		}
		set
		{
			*(bool*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_hasClipping)) = flag;
		}
	}

	static SubmeshInstruction()
	{
		Il2CppClassPointerStore<SubmeshInstruction>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine.Unity", "SubmeshInstruction");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<SubmeshInstruction>.NativeClassPtr);
		NativeFieldInfoPtr_skeleton = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SubmeshInstruction>.NativeClassPtr, "skeleton");
		NativeFieldInfoPtr_startSlot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SubmeshInstruction>.NativeClassPtr, "startSlot");
		NativeFieldInfoPtr_endSlot = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SubmeshInstruction>.NativeClassPtr, "endSlot");
		NativeFieldInfoPtr_material = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SubmeshInstruction>.NativeClassPtr, "material");
		NativeFieldInfoPtr_forceSeparate = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SubmeshInstruction>.NativeClassPtr, "forceSeparate");
		NativeFieldInfoPtr_preActiveClippingSlotSource = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SubmeshInstruction>.NativeClassPtr, "preActiveClippingSlotSource");
		NativeFieldInfoPtr_rawTriangleCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SubmeshInstruction>.NativeClassPtr, "rawTriangleCount");
		NativeFieldInfoPtr_rawVertexCount = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SubmeshInstruction>.NativeClassPtr, "rawVertexCount");
		NativeFieldInfoPtr_rawFirstVertexIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SubmeshInstruction>.NativeClassPtr, "rawFirstVertexIndex");
		NativeFieldInfoPtr_hasClipping = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<SubmeshInstruction>.NativeClassPtr, "hasClipping");
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<SubmeshInstruction>.NativeClassPtr, 100664381);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 793269, XrefRangeEnd = 793294, MetadataInitTokenRva = 46402876L, MetadataInitFlagRva = 59848242L)]
	public unsafe override string ToString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ToString_Public_Virtual_String_0, IL2CPP.il2cpp_object_unbox(IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this)), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public SubmeshInstruction(System.IntPtr pointer)
		: base(pointer)
	{
	}

	public SubmeshInstruction()
		: base(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<SubmeshInstruction>.NativeClassPtr))
	{
	}
}
