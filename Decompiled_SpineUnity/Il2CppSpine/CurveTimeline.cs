using System;
using System.Runtime.CompilerServices;
using Il2CppInterop.Common.Attributes;
using Il2CppInterop.Runtime;
using Il2CppInterop.Runtime.InteropTypes;
using Il2CppInterop.Runtime.InteropTypes.Arrays;
using Il2CppInterop.Runtime.Runtime;
using Il2CppSystem;

namespace Il2CppSpine;

public class CurveTimeline : Il2CppSystem.Object
{
	private static readonly System.IntPtr NativeFieldInfoPtr_curves;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_FrameCount_Public_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr__ctor_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_Apply_Public_Abstract_Virtual_New_Void_Skeleton_Single_Single_ExposedList_1_Event_Single_MixBlend_MixDirection_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_get_PropertyId_Public_Abstract_Virtual_New_get_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetStepped_Public_Void_Int32_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_SetCurve_Public_Void_Int32_Single_Single_Single_Single_0;

	private static readonly System.IntPtr NativeMethodInfoPtr_GetCurvePercent_Public_Single_Int32_Single_0;

	public unsafe Il2CppStructArray<float> curves
	{
		get
		{
			nint num = (nint)IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this) + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_curves);
			System.IntPtr intPtr = *(System.IntPtr*)num;
			return (intPtr != (System.IntPtr)0) ? Il2CppObjectPool.Get<Il2CppStructArray<float>>(intPtr) : null;
		}
		set
		{
			System.IntPtr num = IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			IL2CPP.il2cpp_gc_wbarrier_set_field(num, (System.IntPtr)((nint)num + (int)IL2CPP.il2cpp_field_get_offset(NativeFieldInfoPtr_curves)), IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)val));
		}
	}

	public unsafe int FrameCount
	{
		[CallerCount(9)]
		[CachedScanResults(RefRangeStart = 782007, RefRangeEnd = 782016, XrefRangeStart = 782007, XrefRangeEnd = 782007, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
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

	public unsafe virtual int PropertyId
	{
		[CallerCount(0)]
		get
		{
			IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
			System.IntPtr* ptr = null;
			Unsafe.SkipInit(out System.IntPtr intPtr2);
			System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_get_PropertyId_Public_Abstract_Virtual_New_get_Int32_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
			Il2CppException.RaiseExceptionIfNecessary(intPtr2);
			return *(int*)IL2CPP.il2cpp_object_unbox(intPtr);
		}
	}

	static CurveTimeline()
	{
		Il2CppClassPointerStore<CurveTimeline>.NativeClassPtr = IL2CPP.GetIl2CppClass("spine-unity.dll", "Spine", "CurveTimeline");
		IL2CPP.il2cpp_runtime_class_init(Il2CppClassPointerStore<CurveTimeline>.NativeClassPtr);
		NativeFieldInfoPtr_curves = IL2CPP.GetIl2CppField(Il2CppClassPointerStore<CurveTimeline>.NativeClassPtr, "curves");
		NativeMethodInfoPtr_get_FrameCount_Public_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurveTimeline>.NativeClassPtr, 100663333);
		NativeMethodInfoPtr__ctor_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurveTimeline>.NativeClassPtr, 100663334);
		NativeMethodInfoPtr_Apply_Public_Abstract_Virtual_New_Void_Skeleton_Single_Single_ExposedList_1_Event_Single_MixBlend_MixDirection_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurveTimeline>.NativeClassPtr, 100663335);
		NativeMethodInfoPtr_get_PropertyId_Public_Abstract_Virtual_New_get_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurveTimeline>.NativeClassPtr, 100663336);
		NativeMethodInfoPtr_SetStepped_Public_Void_Int32_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurveTimeline>.NativeClassPtr, 100663337);
		NativeMethodInfoPtr_SetCurve_Public_Void_Int32_Single_Single_Single_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurveTimeline>.NativeClassPtr, 100663338);
		NativeMethodInfoPtr_GetCurvePercent_Public_Single_Int32_Single_0 = IL2CPP.GetIl2CppMethodByToken(Il2CppClassPointerStore<CurveTimeline>.NativeClassPtr, 100663339);
	}

	[CallerCount(12)]
	[CachedScanResults(RefRangeStart = 782027, RefRangeEnd = 782039, XrefRangeStart = 782016, XrefRangeEnd = 782027, MetadataInitTokenRva = 47211400L, MetadataInitFlagRva = 59849659L)]
	public unsafe CurveTimeline(int frameCount)
		: this(IL2CPP.il2cpp_object_new(Il2CppClassPointerStore<CurveTimeline>.NativeClassPtr))
	{
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&frameCount);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr__ctor_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
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
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(IL2CPP.il2cpp_object_get_virtual_method(IL2CPP.Il2CppObjectBaseToPtr((Il2CppObjectBase)(object)this), NativeMethodInfoPtr_Apply_Public_Abstract_Virtual_New_Void_Skeleton_Single_Single_ExposedList_1_Event_Single_MixBlend_MixDirection_0), IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(2)]
	[CachedScanResults(RefRangeStart = 782039, RefRangeEnd = 782041, XrefRangeStart = 782039, XrefRangeEnd = 782039, MetadataInitTokenRva = 0L, MetadataInitFlagRva = 0L)]
	public unsafe void SetStepped(int frameIndex)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[1];
		*ptr = (nint)(&frameIndex);
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetStepped_Public_Void_Int32_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(0)]
	public unsafe void SetCurve(int frameIndex, float cx1, float cy1, float cx2, float cy2)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[5];
		*ptr = (nint)(&frameIndex);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &cx1;
		*(float**)((byte*)ptr + checked((nuint)2u * unchecked((nuint)sizeof(System.IntPtr)))) = &cy1;
		*(float**)((byte*)ptr + checked((nuint)3u * unchecked((nuint)sizeof(System.IntPtr)))) = &cx2;
		*(float**)((byte*)ptr + checked((nuint)4u * unchecked((nuint)sizeof(System.IntPtr)))) = &cy2;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_SetCurve_Public_Void_Int32_Single_Single_Single_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
	}

	[CallerCount(13)]
	[CachedScanResults(RefRangeStart = 782045, RefRangeEnd = 782058, XrefRangeStart = 782041, XrefRangeEnd = 782045, MetadataInitTokenRva = 47211368L, MetadataInitFlagRva = 59849660L)]
	public unsafe float GetCurvePercent(int frameIndex, float percent)
	{
		IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this);
		System.IntPtr* ptr = stackalloc System.IntPtr[2];
		*ptr = (nint)(&frameIndex);
		*(float**)((byte*)ptr + checked((nuint)1u * unchecked((nuint)sizeof(System.IntPtr)))) = &percent;
		Unsafe.SkipInit(out System.IntPtr intPtr2);
		System.IntPtr intPtr = IL2CPP.il2cpp_runtime_invoke(NativeMethodInfoPtr_GetCurvePercent_Public_Single_Int32_Single_0, IL2CPP.Il2CppObjectBaseToPtrNotNull((Il2CppObjectBase)(object)this), (void**)ptr, ref intPtr2);
		Il2CppException.RaiseExceptionIfNecessary(intPtr2);
		return *(float*)IL2CPP.il2cpp_object_unbox(intPtr);
	}

	public CurveTimeline(System.IntPtr pointer)
		: base(pointer)
	{
	}
}
