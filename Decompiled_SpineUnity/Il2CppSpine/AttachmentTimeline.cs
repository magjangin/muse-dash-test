using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class AttachmentTimeline : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_slotIndex;

	private static readonly System.IntPtr NativeFieldInfoPtr_frames;

	private static readonly System.IntPtr NativeFieldInfoPtr_attachmentNames;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_PropertyId_Public_Virtual_Final_New_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_FrameCount_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetFrame_Public_Void_Int32_Single_String_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Apply_Public_Virtual_Final_New_Void_Skeleton_Single_Single_ExposedList_1_Event_Single_MixBlend_MixDirection_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetAttachment_Private_Void_Skeleton_Slot_String_0;

	public unsafe int slotIndex
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slotIndex);
			return *(int*)num;
		}
		set
		{
			*(int*)((nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_slotIndex)) = num;
		}
	}

	public unsafe Il2CppStructArray<float> frames
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frames);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frames)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppStringArray attachmentNames
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachmentNames);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStringArray>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachmentNames)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe virtual int PropertyId
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_PropertyId_Public_Virtual_Final_New_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	public unsafe int FrameCount
	{
		[CallerCount(12)]
		[CachedScanResults(RefRangeStart = 442085, RefRangeEnd = 442097, XrefRangeStart = 442085, XrefRangeEnd = 442097, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_get_FrameCount_Public_get_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static AttachmentTimeline()
	{
		Il2CppClassPointerStore<AttachmentTimeline>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "AttachmentTimeline");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<AttachmentTimeline>.NativeClassPtr);
		NativeFieldInfoPtr_slotIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AttachmentTimeline>.NativeClassPtr, "slotIndex");
		NativeFieldInfoPtr_frames = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AttachmentTimeline>.NativeClassPtr, "frames");
		NativeFieldInfoPtr_attachmentNames = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<AttachmentTimeline>.NativeClassPtr, "attachmentNames");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentTimeline>.NativeClassPtr, 100663362);
		NativeMethodInfoPtr_get_PropertyId_Public_Virtual_Final_New_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentTimeline>.NativeClassPtr, 100663363);
		NativeMethodInfoPtr_get_FrameCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentTimeline>.NativeClassPtr, 100663364);
		NativeMethodInfoPtr_SetFrame_Public_Void_Int32_Single_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentTimeline>.NativeClassPtr, 100663365);
		NativeMethodInfoPtr_Apply_Public_Virtual_Final_New_Void_Skeleton_Single_Single_ExposedList_1_Event_Single_MixBlend_MixDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentTimeline>.NativeClassPtr, 100663366);
		NativeMethodInfoPtr_SetAttachment_Private_Void_Skeleton_Slot_String_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<AttachmentTimeline>.NativeClassPtr, 100663367);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782140, RefRangeEnd = 782142, XrefRangeStart = 782132, XrefRangeEnd = 782140, MetadataInitTokenRva = 47239336L, MetadataInitFlagRva = 59849647L)]
	public unsafe AttachmentTimeline(int frameCount)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<AttachmentTimeline>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&frameCount);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782151, RefRangeEnd = 782153, XrefRangeStart = 782142, XrefRangeEnd = 782151, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetFrame(int frameIndex, float time, string attachmentName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = (nint)(&frameIndex);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &time;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(attachmentName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetFrame_Public_Void_Int32_Single_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782153, XrefRangeEnd = 782158, MetadataInitTokenRva = 46278060L, MetadataInitFlagRva = 59849728L)]
	public unsafe virtual void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[7];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &lastTime;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &time;
		*(System.IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)firedEvents);
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &alpha;
		*(MixBlend**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(System.IntPtr)))) = &blend;
		*(MixDirection**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(System.IntPtr)))) = &direction;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_Apply_Public_Virtual_Final_New_Void_Skeleton_Single_Single_ExposedList_1_Event_Single_MixBlend_MixDirection_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782158, XrefRangeEnd = 782164, MetadataInitTokenRva = 46278060L, MetadataInitFlagRva = 59849728L)]
	public unsafe void SetAttachment(Skeleton skeleton, Slot slot, string attachmentName)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[3];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)slot);
		*(System.IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = IL2CPP.ManagedStringToIl2Cpp(attachmentName);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetAttachment_Private_Void_Skeleton_Slot_String_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public AttachmentTimeline(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
