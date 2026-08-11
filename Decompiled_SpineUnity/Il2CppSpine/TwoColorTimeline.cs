using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;

namespace Il2CppSpine;

public class TwoColorTimeline : CurveTimeline
{
	private static readonly IntPtr NativeFieldInfoPtr_slotIndex;

	private static readonly IntPtr NativeFieldInfoPtr_frames;

	private static readonly IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_get_PropertyId_Public_Virtual_get_Int32_0;

	private static readonly IntPtr NativeMethodInfoPtr_SetFrame_Public_Void_Int32_Single_Single_Single_Single_Single_Single_Single_Single_0;

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

	static TwoColorTimeline()
	{
		Il2CppClassPointerStore<TwoColorTimeline>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "TwoColorTimeline");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<TwoColorTimeline>.NativeClassPtr);
		NativeFieldInfoPtr_slotIndex = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TwoColorTimeline>.NativeClassPtr, "slotIndex");
		NativeFieldInfoPtr_frames = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<TwoColorTimeline>.NativeClassPtr, "frames");
		NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TwoColorTimeline>.NativeClassPtr, 100663358);
		NativeMethodInfoPtr_get_PropertyId_Public_Virtual_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TwoColorTimeline>.NativeClassPtr, 100663359);
		NativeMethodInfoPtr_SetFrame_Public_Void_Int32_Single_Single_Single_Single_Single_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TwoColorTimeline>.NativeClassPtr, 100663360);
		NativeMethodInfoPtr_Apply_Public_Virtual_Void_Skeleton_Single_Single_ExposedList_1_Event_Single_MixBlend_MixDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<TwoColorTimeline>.NativeClassPtr, 100663361);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782123, RefRangeEnd = 782125, XrefRangeStart = 782118, XrefRangeEnd = 782123, MetadataInitTokenRva = 46257336L, MetadataInitFlagRva = 59827085L)]
	public unsafe TwoColorTimeline(int frameCount)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<TwoColorTimeline>.NativeClassPtr))
	{
		IntPtr* ptr = stackalloc IntPtr[1];
		*ptr = (nint)(&frameCount);
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782125, RefRangeEnd = 782127, XrefRangeStart = 782125, XrefRangeEnd = 782125, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetFrame(int frameIndex, float time, float r, float g, float b, float a, float r2, float g2, float b2)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		IntPtr* ptr = stackalloc IntPtr[9];
		*ptr = (nint)(&frameIndex);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(IntPtr)))) = &time;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(IntPtr)))) = &r;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(IntPtr)))) = &g;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(IntPtr)))) = &b;
		*(float**)((byte*)ptr + checked((nuint)5u * unchecked((nuint)sizeof(IntPtr)))) = &a;
		*(float**)((byte*)ptr + checked((nuint)6u * unchecked((nuint)sizeof(IntPtr)))) = &r2;
		*(float**)((byte*)ptr + checked((nuint)7u * unchecked((nuint)sizeof(IntPtr)))) = &g2;
		*(float**)((byte*)ptr + checked((nuint)8u * unchecked((nuint)sizeof(IntPtr)))) = &b2;
		Unsafe.SkipInit(out IntPtr intPtr2);
		IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetFrame_Public_Void_Int32_Single_Single_Single_Single_Single_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	[CachedScanResults(RefRangeStart = 0, RefRangeEnd = 0, XrefRangeStart = 782127, XrefRangeEnd = 782132, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	public TwoColorTimeline(IntPtr pointer)
		: base(pointer)
	{
	}
}
