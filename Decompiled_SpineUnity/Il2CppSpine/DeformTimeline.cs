using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppSpine;

public class DeformTimeline : CurveTimeline
{
	private static readonly IntPtr NativeFieldInfoPtr_slotIndex;

	private static readonly IntPtr NativeFieldInfoPtr_attachment;

	private static readonly IntPtr NativeFieldInfoPtr_frames;

	private static readonly IntPtr NativeFieldInfoPtr_frameVertices;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_PropertyId_Public_Virtual_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetFrame_Public_Void_Int32_Single_Il2CppStructArray_1_Single_0;

	private static readonly IntPtr NativeMethodInfoPtr_Apply_Public_Virtual_Void_Skeleton_Single_Single_ExposedList_1_Event_Single_MixBlend_MixDirection_0;

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

	public unsafe VertexAttachment attachment
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachment);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<VertexAttachment>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_attachment)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertexAttachment));
		}
	}

	public unsafe Il2CppStructArray<float> frames
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frames);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frames)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe Il2CppReferenceArray<Il2CppStructArray<float>> frameVertices
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frameVertices);
			IntPtr intPtr = *(IntPtr*)num;
			return (intPtr != (IntPtr)0) ? Il2CppObjectPool.Get<Il2CppReferenceArray<Il2CppStructArray<float>>>(intPtr) : null;
		}
		set
		{
			IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_frameVertices)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe override int PropertyId
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IntPtr* ptr = null;
			Unsafe.SkipInit(out IntPtr intPtr2);
			IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_PropertyId_Public_Virtual_get_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static DeformTimeline()
	{
		Il2CppClassPointerStore<DeformTimeline>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "DeformTimeline");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<DeformTimeline>.NativeClassPtr);
		NativeFieldInfoPtr_slotIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeformTimeline>.NativeClassPtr, "slotIndex");
		NativeFieldInfoPtr_attachment = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeformTimeline>.NativeClassPtr, "attachment");
		NativeFieldInfoPtr_frames = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeformTimeline>.NativeClassPtr, "frames");
		NativeFieldInfoPtr_frameVertices = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<DeformTimeline>.NativeClassPtr, "frameVertices");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformTimeline>.NativeClassPtr, 100663368);
		NativeMethodInfoPtr_get_PropertyId_Public_Virtual_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformTimeline>.NativeClassPtr, 100663369);
		NativeMethodInfoPtr_SetFrame_Public_Void_Int32_Single_Il2CppStructArray_1_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformTimeline>.NativeClassPtr, 100663370);
		NativeMethodInfoPtr_Apply_Public_Virtual_Void_Skeleton_Single_Single_ExposedList_1_Event_Single_MixBlend_MixDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<DeformTimeline>.NativeClassPtr, 100663371);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782172, RefRangeEnd = 782174, XrefRangeStart = 782164, XrefRangeEnd = 782172, MetadataInitTokenRva = 46366920L, MetadataInitFlagRva = 59849661L)]
	public unsafe DeformTimeline(int frameCount)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<DeformTimeline>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&frameCount);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782183, RefRangeEnd = 782185, XrefRangeStart = 782174, XrefRangeEnd = 782183, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetFrame(int frameIndex, float time, Il2CppStructArray<float> vertices)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[3];
		*ptr = (nint)(&frameIndex);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &time;
		*(IntPtr*)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)vertices);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetFrame_Public_Void_Int32_Single_Il2CppStructArray_1_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782185, XrefRangeEnd = 782194, MetadataInitTokenRva = 46366892L, MetadataInitFlagRva = 59849662L)]
	public unsafe override void Apply(Skeleton skeleton, float lastTime, float time, ExposedList<Event> firedEvents, float alpha, MixBlend blend, MixDirection direction)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[7];
		*ptr = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)skeleton);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &lastTime;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &time;
		*(IntPtr*)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)firedEvents);
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &alpha;
		*(MixBlend**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &blend;
		*(MixDirection**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = &direction;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Apply_Public_Virtual_Void_Skeleton_Single_Single_ExposedList_1_Event_Single_MixBlend_MixDirection_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	public DeformTimeline(IntPtr pointer)
		: base(pointer)
	{
	}
}
