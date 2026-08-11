using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class Slot : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_data;

	private static readonly System.IntPtr NativeFieldInfoPtr_bone;

	private static readonly System.IntPtr NativeFieldInfoPtr_r;

	private static readonly System.IntPtr NativeFieldInfoPtr_g;

	private static readonly System.IntPtr NativeFieldInfoPtr_b;

	private static readonly System.IntPtr NativeFieldInfoPtr_a;

	private static readonly System.IntPtr NativeFieldInfoPtr_r2;

	private static readonly System.IntPtr NativeFieldInfoPtr_g2;

	private static readonly System.IntPtr NativeFieldInfoPtr_b2;

	private static readonly System.IntPtr NativeFieldInfoPtr_hasSecondColor;

	private static readonly System.IntPtr NativeFieldInfoPtr_attachment;

	private static readonly System.IntPtr NativeFieldInfoPtr_attachmentTime;

	private static readonly System.IntPtr NativeFieldInfoPtr_deform;

	private static readonly System.IntPtr NativeFieldInfoPtr_attachmentState;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_SlotData_Bone_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Skeleton_Public_get_Skeleton_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampColor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_HasSecondColor_Public_get_Boolean_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ClampSecondColor_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Attachment_Public_get_Attachment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_set_Attachment_Public_set_Void_Attachment_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_Deform_Public_get_ExposedList_1_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetToSetupPose_Public_Void_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_ToString_Public_Virtual_String_0;

	public unsafe SlotData data
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_data);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<SlotData>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_data)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slotData));
		}
	}

	public unsafe Bone bone
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bone);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Bone>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_bone)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone));
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

	public unsafe Attachment attachment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachment);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Attachment>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)attachment));
		}
	}

	public unsafe float attachmentTime
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachmentTime);
			return *(float*)num;
		}
		set
		{
			*(float*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachmentTime)) = num;
		}
	}

	public unsafe ExposedList<float> deform
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_deform);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_deform)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)exposedList));
		}
	}

	public unsafe int attachmentState
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachmentState);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachmentState)) = num;
		}
	}

	public unsafe Skeleton Skeleton
	{
		[CallerCount(2)]
		[CachedScanResults(RefRangeStart = 588531, RefRangeEnd = 588533, XrefRangeStart = 588531, XrefRangeEnd = 588533, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Skeleton_Public_get_Skeleton_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Skeleton>(intPtr) : null;
		}
	}

	public unsafe bool HasSecondColor
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_HasSecondColor_Public_get_Boolean_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(bool*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe Attachment Attachment
	{
		[CallerCount(4)]
		[CachedScanResults(RefRangeStart = 37591, RefRangeEnd = 37595, XrefRangeStart = 37591, XrefRangeEnd = 37595, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Attachment_Public_get_Attachment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Attachment>(intPtr) : null;
		}
		[CallerCount(11)]
		[CachedScanResults(RefRangeStart = 787883, RefRangeEnd = 787894, XrefRangeStart = 787879, XrefRangeEnd = 787883, MetadataInitTokenRva = 46285152L, MetadataInitFlagRva = 59867974L)]
		set
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = stackalloc System.IntPtr[1];
			*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)value);
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_set_Attachment_Public_set_Void_Attachment_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		}
	}

	public unsafe ExposedList<float> Deform
	{
		[CallerCount(1)]
		[CachedScanResults(RefRangeStart = 51755, RefRangeEnd = 51756, XrefRangeStart = 51755, XrefRangeEnd = 51756, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_Deform_Public_get_ExposedList_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<ExposedList<float>>(intPtr) : null;
		}
	}

	static Slot()
	{
		Il2CppClassPointerStore<Slot>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "Slot");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<Slot>.NativeClassPtr);
		NativeFieldInfoPtr_data = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slot>.NativeClassPtr, "data");
		NativeFieldInfoPtr_bone = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slot>.NativeClassPtr, "bone");
		NativeFieldInfoPtr_r = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slot>.NativeClassPtr, "r");
		NativeFieldInfoPtr_g = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slot>.NativeClassPtr, "g");
		NativeFieldInfoPtr_b = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slot>.NativeClassPtr, "b");
		NativeFieldInfoPtr_a = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slot>.NativeClassPtr, "a");
		NativeFieldInfoPtr_r2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slot>.NativeClassPtr, "r2");
		NativeFieldInfoPtr_g2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slot>.NativeClassPtr, "g2");
		NativeFieldInfoPtr_b2 = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slot>.NativeClassPtr, "b2");
		NativeFieldInfoPtr_hasSecondColor = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slot>.NativeClassPtr, "hasSecondColor");
		NativeFieldInfoPtr_attachment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slot>.NativeClassPtr, "attachment");
		NativeFieldInfoPtr_attachmentTime = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slot>.NativeClassPtr, "attachmentTime");
		NativeFieldInfoPtr_deform = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slot>.NativeClassPtr, "deform");
		NativeFieldInfoPtr_attachmentState = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<Slot>.NativeClassPtr, "attachmentState");
		NativeMethodInfoPtr__ctor_Public_Void_SlotData_Bone_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slot>.NativeClassPtr, 100663820);
		NativeMethodInfoPtr_get_Skeleton_Public_get_Skeleton_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slot>.NativeClassPtr, 100663821);
		NativeMethodInfoPtr_ClampColor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slot>.NativeClassPtr, 100663822);
		NativeMethodInfoPtr_get_HasSecondColor_Public_get_Boolean_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slot>.NativeClassPtr, 100663823);
		NativeMethodInfoPtr_ClampSecondColor_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slot>.NativeClassPtr, 100663824);
		NativeMethodInfoPtr_get_Attachment_Public_get_Attachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slot>.NativeClassPtr, 100663825);
		NativeMethodInfoPtr_set_Attachment_Public_set_Void_Attachment_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slot>.NativeClassPtr, 100663826);
		NativeMethodInfoPtr_get_Deform_Public_get_ExposedList_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slot>.NativeClassPtr, 100663827);
		NativeMethodInfoPtr_SetToSetupPose_Public_Void_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slot>.NativeClassPtr, 100663828);
		NativeMethodInfoPtr_ToString_Public_Virtual_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<Slot>.NativeClassPtr, 100663829);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 787862, RefRangeEnd = 787863, XrefRangeStart = 787834, XrefRangeEnd = 787862, MetadataInitTokenRva = 46285112L, MetadataInitFlagRva = 59867971L)]
	public unsafe Slot(SlotData data, Bone bone)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<Slot>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)data);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)bone);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_SlotData_Bone_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 787870, RefRangeEnd = 787872, XrefRangeStart = 787863, XrefRangeEnd = 787870, MetadataInitTokenRva = 46285000L, MetadataInitFlagRva = 59867972L)]
	public unsafe void ClampColor()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampColor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(1)]
	[CachedScanResults(RefRangeStart = 787878, RefRangeEnd = 787879, XrefRangeStart = 787872, XrefRangeEnd = 787878, MetadataInitTokenRva = 46285072L, MetadataInitFlagRva = 59867973L)]
	public unsafe void ClampSecondColor()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_ClampSecondColor_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 787899, RefRangeEnd = 787901, XrefRangeStart = 787894, XrefRangeEnd = 787899, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetToSetupPose()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetToSetupPose_Public_Void_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(25)]
	[CachedScanResults(RefRangeStart = 534078, RefRangeEnd = 534103, XrefRangeStart = 534078, XrefRangeEnd = 534103, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe override string ToString()
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = null;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_ToString_Public_Virtual_String_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return IL2CPP.Il2CppStringToManaged(intPtr);
	}

	public Slot(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
